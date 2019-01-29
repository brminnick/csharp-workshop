using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubActivityReport
{
    public class GraphQLRequest
    {
        public string Query { get; set; }
        public IDictionary<string, object> Variables { get; } = new Dictionary<string, object>();

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

            var client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("IssueQueryDemo"))
            {
                Credentials = new Octokit.Credentials(key)
            };

            // Next: Run the issue query.
            var issueAndPRQuery = new GraphQLRequest
            {
                Query = Queries.IssueQuery
            };
            issueAndPRQuery.Variables["repo_name"] = "docs";

            var postBody = issueAndPRQuery.ToJsonText();
            var response = await client.Connection.Post<string>(new Uri("https://api.github.com/graphql"),
                postBody, "application/json", "application/json");

            JObject results = JObject.Parse(response.HttpResponse.Body.ToString());
            Console.WriteLine(results);

            // Find PRs:
            issueAndPRQuery.Query = Queries.PullRequestQuery;
            issueAndPRQuery.Variables["repo_name"] = "docs";

            postBody = issueAndPRQuery.ToJsonText();
            response = await client.Connection.Post<string>(new Uri("https://api.github.com/graphql"),
                postBody, "application/json", "application/json");

            results = JObject.Parse(response.HttpResponse.Body.ToString());
            Console.WriteLine(results);

            // TODO as a lab:
            // 1. Find the latest 50 Open issues in the dotnet/docs repo.
            // 2. Find the latest 50 in dotnet-api-docs
            // 3. Do the same for PRs in: dotnet/docs, dotnet/dotnet-api-docs, dotnet/samples
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
