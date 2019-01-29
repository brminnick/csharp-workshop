using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;

namespace GitHubActivityReport
{
    class GraphQLRequest
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("variables")]
        public IDictionary<string, object> Variables { get; } = new Dictionary<string, object>();

        public string ToJsonText() =>
            JsonConvert.SerializeObject(this);
    }

    static class GraphQLQueries
    {
        internal const string IssueQuery =
@"query ($repo_name: String!) {
  repository(owner: ""dotnet"", name: $repo_name) {
    issues(last: 50)
      {
        nodes {
          title
          number
          createdAt
        }
      }
    }
  }
";
        internal const string PullRequestQuery =
@"query($repo_name:String!) {
  repository(owner: ""dotnet"", name: $repo_name) {
    pullRequests(last: 50) {
      nodes {
        title
        number
        createdAt
      }
    }
  }
}";
        internal const string PagedIssueQuery =
@"query ($repo_name: String!,  $start_cursor:String) {
  repository(owner: ""dotnet"", name: $repo_name) {
    issues(last: 25, before: $start_cursor)
     {
        totalCount
        pageInfo {
          hasPreviousPage
          startCursor
        }
        nodes {
          title
          number
          createdAt
        }
      }
    }
  }
";
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            //Follow these steps to create a GitHub Access Token https://help.github.com/articles/creating-a-personal-access-token-for-the-command-line/#creating-a-token
            //Select the following permissions for your GitHub Access Token:
            // - repo:status
            // - public_repo          
            var key = GetEnvVariable("GitHubKey",
            "You must store you GitHub key in the 'GitHubKey' environment variable",
            "");

            var client = new GitHubClient(new ProductHeaderValue("IssueQueryDemo"))
            {
                Credentials = new Credentials(key)
            };

            // Next: Run the issue query.
            //await SimpleRunQuery(client);

            // await IssuesThenPRsQuery(client);

            //await PrintInOrderFinished(client);

            try
            {
                var results = await RunPagedQuery(client, GraphQLQueries.PagedIssueQuery, "docs");
                Console.WriteLine(results);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Work has been cancelled");
            }

            Console.ReadLine();
        }

        static async Task SimpleRunQuery(GitHubClient client)
        {
            JObject results = await RunQuery(client, GraphQLQueries.IssueQuery, "docs");
            Console.WriteLine(results);

            results = await RunQuery(client, GraphQLQueries.IssueQuery, "dotnet-api-docs");
            Console.WriteLine(results);

            // Find PRs:
            results = await RunQuery(client, GraphQLQueries.PullRequestQuery, "samples");
            Console.WriteLine(results);

            results = await RunQuery(client, GraphQLQueries.PullRequestQuery, "dotnet-api-docs");
            Console.WriteLine(results);

            results = await RunQuery(client, GraphQLQueries.PullRequestQuery, "docs");
            Console.WriteLine(results);
        }

        static async Task IssuesThenPRsQuery(GitHubClient client)
        {
            var docsIssueTask = RunQuery(client, GraphQLQueries.IssueQuery, "docs");

            var apidocsIssueTask = RunQuery(client, GraphQLQueries.IssueQuery, "dotnet-api-docs");

            // Find PRs:
            var samplesPRTask = RunQuery(client, GraphQLQueries.PullRequestQuery, "samples");
            var apiDocsPRTask = RunQuery(client, GraphQLQueries.PullRequestQuery, "dotnet-api-docs");
            var docsPRTask = RunQuery(client, GraphQLQueries.PullRequestQuery, "docs");

            writeData(await docsIssueTask);
            writeData(await apidocsIssueTask);
            writeData(await samplesPRTask);
            writeData(await apiDocsPRTask);
            writeData(await docsPRTask);

            void writeData(JObject data) => Console.WriteLine(data);
        }

        static async Task PrintInOrderFinished(GitHubClient client)
        {
            List<Task<JObject>> queryTasks = new List<Task<JObject>>
            {
                RunQuery(client, GraphQLQueries.IssueQuery, "docs"),
                RunQuery(client, GraphQLQueries.IssueQuery, "dotnet-api-docs"),
                RunQuery(client, GraphQLQueries.PullRequestQuery, "samples"),
                RunQuery(client, GraphQLQueries.PullRequestQuery, "dotnet-api-docs"),
                RunQuery(client, GraphQLQueries.PullRequestQuery, "docs")
            };

            while (queryTasks.Any())
            {
                var finished = await Task.WhenAny(queryTasks);
                writeData(await finished);
                queryTasks.Remove(finished);
            }

            void writeData(JObject data) => Console.WriteLine(data);
        }

        static async Task<JObject> RunQuery(GitHubClient client, string queryText, string repoName)
        {
            var issueAndPRQuery = new GraphQLRequest
            {
                Query = queryText
            };
            issueAndPRQuery.Variables["repo_name"] = repoName;

            var postBody = issueAndPRQuery.ToJsonText();
            var response = await client.Connection.Post<string>(new Uri("https://api.github.com/graphql"),
                postBody, "application/json", "application/json");

            JObject results = JObject.Parse(response.HttpResponse.Body.ToString());

            return results;
        }

        static async Task<JArray> RunPagedQuery(GitHubClient client, string queryText, string repoName)
        {
            var issueAndPRQuery = new GraphQLRequest
            {
                Query = queryText
            };
            issueAndPRQuery.Variables["repo_name"] = repoName;

            JArray finalResults = new JArray();
            bool hasMorePages = true;
            int pagesReturned = 0;

            // Stop with 10 pages, because these are big repos:
            while (hasMorePages && (pagesReturned++ < 10))
            {
                var postBody = issueAndPRQuery.ToJsonText();
                var response = await client.Connection.Post<string>(new Uri("https://api.github.com/graphql"),
                    postBody, "application/json", "application/json");

                JObject results = JObject.Parse(response.HttpResponse.Body.ToString());

                hasMorePages = (bool)results["data"]["repository"]["issues"]["pageInfo"]["hasPreviousPage"];
                issueAndPRQuery.Variables["start_cursor"] = results["data"]["repository"]["issues"]["pageInfo"]["startCursor"].ToString();

                finalResults.Merge(results["data"]["repository"]["issues"]["nodes"]);
            }
            return finalResults;
        }

        private static string GetEnvVariable(string item, string error, string defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(item);

            if (string.IsNullOrWhiteSpace(value))
            {
                if (!string.IsNullOrWhiteSpace(error))
                {
                    Console.WriteLine(error);
                    Environment.Exit(0);
                }

                if (!string.IsNullOrWhiteSpace(defaultValue))
                {
                    return defaultValue;
                }
            }

            return value;
        }

        class ProgressStatus : IProgress<int>
        {
            readonly Action<int> action;

            public ProgressStatus(Action<int> progressAction) =>
                action = progressAction;

            public void Report(int value) => action?.Invoke(value);
        }
    }
}
