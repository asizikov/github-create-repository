using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace GitHub.Automation.Configuration
{
    public class ConfigurationProvider
    {
        private const string owner = "owner";
        private const string repo = "repo";

        public async Task<NewRepositoryConfiguration> GetConfigurationAsync(IGitHubClient client)
        {
            var contents =
                await
                    client.Repository.Content.GetAllContents(owner, repo, "configuration/configuration.json")
                        .ConfigureAwait(false);
            var configurationString = contents.First().Content;
            //deserialize

            return null;
        }
    }
}