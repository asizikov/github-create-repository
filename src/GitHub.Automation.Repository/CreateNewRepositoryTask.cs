using System.Linq;
using System.Threading.Tasks;
using GitHub.Automation.Configuration;
using Octokit;

namespace GitHub.Automation.Repository
{
    public class CreateNewRepositoryTask
    {
        private IGitHubClient Client { get; }
        private NewRepositoryConfiguration Configuration { get; }

        public CreateNewRepositoryTask(IGitHubClient client, NewRepositoryConfiguration configuration)
        {
            Client = client;
            Configuration = configuration;
        }

        public async Task<string> CreateAsync(string name, string owner)
        {
            var newRepo = new NewRepository(name) { AutoInit = true };

            var repository = await Client.Repository.Create(newRepo).ConfigureAwait(false);
            var newLabels =
                Configuration.repository.labels.Select(label => new NewLabel(label.name, label.color)).ToList();
            foreach (var newLabel in newLabels)
            {
                await Client.Issue.Labels.Create(owner, repository.Name, newLabel).ConfigureAwait(false);
            }

            return repository.HtmlUrl;
        }
    }
}