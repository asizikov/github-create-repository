using System.Threading.Tasks;
using GitHub.Automation.Configuration;
using GitHub.Automation.Repository;
using Octokit;

namespace GitHub.Automation.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task.Run(() => TestAsync()).Wait();
        }

        public static async Task TestAsync()
        {
            var owner = "#";
            var password = "#";
            var client = new GitHubClient(new ProductHeaderValue("tool"));
            client.Credentials = new Credentials(owner, password);
            var configuration = new ConfigurationProvider(owner, "github-create-repository-configuration");
            var config = await configuration.GetConfigurationAsync(client);
            System.Console.WriteLine(config.version);

            var task = new CreateNewRepositoryTask(client, config);
            var repository = await task.CreateAsync("hello-new-repo");
            System.Console.WriteLine("Browse the repository at: " + repository);
            System.Console.ReadLine();
        }
    }
}