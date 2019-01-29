using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    static class Queries
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

            var client = new GitHubClient(new Octokit.ProductHeaderValue("IssueQueryDemo"))
            {
                Credentials = new Octokit.Credentials(key)
            };

            // Next: Run the issue query.
            await SimpleRunQuery(client);

            await IssuesThenPRsQuery(client);

            await PrintInOrderFinished(client);

            Console.ReadLine();
        }

        static async Task SimpleRunQuery(GitHubClient client)
        {
            JObject results = await RunQuery(client, Queries.IssueQuery, "docs");
            Console.WriteLine(results);

            results = await RunQuery(client, Queries.IssueQuery, "dotnet-api-docs");
            Console.WriteLine(results);

            // Find PRs:
            results = await RunQuery(client, Queries.PullRequestQuery, "samples");
            Console.WriteLine(results);

            results = await RunQuery(client, Queries.PullRequestQuery, "dotnet-api-docs");
            Console.WriteLine(results);

            results = await RunQuery(client, Queries.PullRequestQuery, "docs");
            Console.WriteLine(results);
        }

        static async Task IssuesThenPRsQuery(GitHubClient client)
        {
            var docsIssueTask =  RunQuery(client, Queries.IssueQuery, "docs");

            var apidocsIssueTask = RunQuery(client, Queries.IssueQuery, "dotnet-api-docs");

            // Find PRs:
            var samplesPRTask = RunQuery(client, Queries.PullRequestQuery, "samples");
            var apiDocsPRTask = RunQuery(client, Queries.PullRequestQuery, "dotnet-api-docs");
            var docsPRTask = RunQuery(client, Queries.PullRequestQuery, "docs");

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
                RunQuery(client, Queries.IssueQuery, "docs"),
                RunQuery(client, Queries.IssueQuery, "dotnet-api-docs"),
                RunQuery(client, Queries.PullRequestQuery, "samples"),
                RunQuery(client, Queries.PullRequestQuery, "dotnet-api-docs"),
                RunQuery(client, Queries.PullRequestQuery, "docs")
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
            if (client == null)
                throw new ArgumentNullException(paramName: nameof(client), "bad null client");
            if (string.IsNullOrWhiteSpace(queryText))
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(repoName))
                throw new ArgumentNullException();

            return runQueryImpl();

            async Task<JObject> runQueryImpl()
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
        }

        static string GetEnvVariable(string item, string error, string defaultValue)
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
    }
}
