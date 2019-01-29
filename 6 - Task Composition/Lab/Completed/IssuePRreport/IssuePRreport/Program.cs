using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubActivityReport
{
    public class GraphQLRequest
    {
        public string query { get; set; }
        public IDictionary<string, object> variables { get; } = new Dictionary<string, object>();

        public string ToJsonText() =>
            JsonConvert.SerializeObject(this);
    }

    class Queries
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
            var key = GetEnvVariable("GitHubKey",
            "You must store you GitHub key in the 'GitHubKey' environment variable",
            "");

            var client = new GitHubClient(new Octokit.ProductHeaderValue("IssueQueryDemo"))
            {
                Credentials = new Octokit.Credentials(key)
            };

            // Next: Run the issue query.
            //await SimpleRunQuery(client);

            // await IssuesThenPRsQuery(client);

            await PrintInOrderFinished(client);
        }

        private static async Task SimpleRunQuery(GitHubClient client)
        {
            JObject results = await runQuery(client, Queries.IssueQuery, "docs");
            Console.WriteLine(results);

            results = await runQuery(client, Queries.IssueQuery, "dotnet-api-docs");
            Console.WriteLine(results);

            // Find PRs:
            results = await runQuery(client, Queries.PullRequestQuery, "samples");
            Console.WriteLine(results);

            results = await runQuery(client, Queries.PullRequestQuery, "dotnet-api-docs");
            Console.WriteLine(results);

            results = await runQuery(client, Queries.PullRequestQuery, "docs");
            Console.WriteLine(results);
        }

        private static async Task IssuesThenPRsQuery(GitHubClient client)
        {
            var docsIssueTask =  runQuery(client, Queries.IssueQuery, "docs");

            var apidocsIssueTask = runQuery(client, Queries.IssueQuery, "dotnet-api-docs");

            // Find PRs:
            var samplesPRTask = runQuery(client, Queries.PullRequestQuery, "samples");
            var apiDocsPRTask = runQuery(client, Queries.PullRequestQuery, "dotnet-api-docs");
            var docsPRTask = runQuery(client, Queries.PullRequestQuery, "docs");

            writeData(await docsIssueTask);
            writeData(await apidocsIssueTask);
            writeData(await samplesPRTask);
            writeData(await apiDocsPRTask);
            writeData(await docsPRTask);

            void writeData(JObject data) => Console.WriteLine(data);
        }

        private static async Task PrintInOrderFinished(GitHubClient client)
        {
            List<Task<JObject>> queryTasks = new List<Task<JObject>>
            {
                runQuery(client, Queries.IssueQuery, "docs"),
                runQuery(client, Queries.IssueQuery, "dotnet-api-docs"),
                runQuery(client, Queries.PullRequestQuery, "samples"),
                runQuery(client, Queries.PullRequestQuery, "dotnet-api-docs"),
                runQuery(client, Queries.PullRequestQuery, "docs")
            };

            while (queryTasks.Any())
            {
                var finished = await Task.WhenAny(queryTasks);
                writeData(await finished);
                queryTasks.Remove(finished);
            }

            void writeData(JObject data) => Console.WriteLine(data);
        }

        private static async Task<JObject> runQuery(GitHubClient client, string queryText, string repoName)
        {
            var issueAndPRQuery = new GraphQLRequest
            {
                query = queryText
            };
            issueAndPRQuery.variables["repo_name"] = repoName;

            var postBody = issueAndPRQuery.ToJsonText();
            var response = await client.Connection.Post<string>(new Uri("https://api.github.com/graphql"),
                postBody, "application/json", "application/json");

            JObject results = JObject.Parse(response.HttpResponse.Body.ToString());
            return results;
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
    }
}
