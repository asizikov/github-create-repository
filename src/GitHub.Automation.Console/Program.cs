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
            var client = new GitHubClient(new ProductHeaderValue("tool"));
            client.Credentials = new Credentials("#", "#");
            var configuration = new ConfigurationProvider();
            var config = await configuration.GetConfigurationAsync(client);
        }
    }
}
