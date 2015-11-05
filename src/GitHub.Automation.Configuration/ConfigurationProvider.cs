using System.Linq;
using System.Threading.Tasks;
using Octokit;
using Newtonsoft.Json;

namespace GitHub.Automation.Configuration
{
    public class ConfigurationProvider
    {
        private const string owner = "#";
        private const string repo = "github-create-repository-configuration";

        public async Task<NewRepositoryConfiguration> GetConfigurationAsync(IGitHubClient client)
        {
            var contents =
                await
                    client.Repository.Content.GetAllContents(owner, repo, "configuration/configuration.json")
                        .ConfigureAwait(false);
            var configurationString = contents.First().Content;
            var result = JsonConvert.DeserializeObject<NewRepositoryConfiguration>(configurationString);

            return result;
        }
    }
}