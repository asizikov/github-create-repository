using System.Linq;
using System.Threading.Tasks;
using Octokit;
using Newtonsoft.Json;

namespace GitHub.Automation.Configuration
{
    public class ConfigurationProvider
    {

        private string Owner {get; }
        private string Repository { get; }
        public ConfigurationProvider(string owner, string repository)
        {
            Owner = owner;
            Repository = repository;
        }

        public async Task<NewRepositoryConfiguration> GetConfigurationAsync(IGitHubClient client)
        {
            var contents =
                await
                    client.Repository.Content.GetAllContents(Owner, Repository, "configuration/configuration.json")
                        .ConfigureAwait(false);
            var configurationString = contents.First().Content;
            var result = JsonConvert.DeserializeObject<NewRepositoryConfiguration>(configurationString);

            return result;
        }
    }
}