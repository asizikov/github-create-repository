using GitHub.Automation.Configuration;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.Automation.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => TestAsync()).Wait();
        }

        public static async Task TestAsync()
        {
            var owner = "#";
            var client = new GitHubClient(new ProductHeaderValue("tool"));
            client.Credentials = new Credentials(owner, "#");
            var configuration = new ConfigurationProvider(owner, "github-create-repository-configuration");
            var config = await configuration.GetConfigurationAsync(client);
            System.Console.WriteLine(config.version);
            System.Console.ReadLine();
        }
    }
}
