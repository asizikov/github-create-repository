using GitHub.Automation.Configuration;
using Octokit;
using System.Threading.Tasks;

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
        public async Task<string> CreateAsync(string name)
        {
            var newRepo = new NewRepository(name)
            {
                AutoInit = true
            };

            var repository = await Client.Repository.Create(newRepo);
            return repository.HtmlUrl;
        }
    }
}
