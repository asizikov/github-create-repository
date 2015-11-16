using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Octokit;

namespace GitHub.Automation.Repository.Configuration
{
    public class ConfigurationProvider
    {
        private string Version { get; }
        private string Owner { get; }
        private string Repository { get; }

        public ConfigurationProvider(string owner, string repository)
        {
            Owner = owner;
            Repository = repository;
            Version = "0.2";
        }

        public async Task<NewRepositoryConfiguration> GetConfigurationAsync(IGitHubClient client)
        {
            var contents =
                await
                    client.Repository.Content.GetAllContents(Owner, Repository, "configuration/configuration.json")
                        .ConfigureAwait(false);
            var configurationString = contents.First().Content;
            var result = JsonConvert.DeserializeObject<NewRepositoryConfiguration>(configurationString);
            if (Version != result.Version)
            {
                throw new NotSupportedException(
                    $"Configuration file version '{result.Version}' is not supported. Required version is '{Version}'");
            }
            return result;
        }
    }
}