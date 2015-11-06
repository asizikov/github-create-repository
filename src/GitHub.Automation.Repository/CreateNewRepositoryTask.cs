using System;
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
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            Client = client;
            Configuration = configuration;
        }

        public async Task<string> CreateAsync(string name)
        {
            var newRepo = new NewRepository(name) { AutoInit = true };

            var repository = await Client.Repository.Create(newRepo).ConfigureAwait(false);
            var newLabels =
                Configuration.repository.labels.Select(label => new NewLabel(label.name, label.color)).ToList();
            foreach (var newLabel in newLabels)
            {
                await Client.Issue.Labels.Create(repository.Owner.Login, repository.Name, newLabel).ConfigureAwait(false);
            }
            foreach (var branch in Configuration.repository.branches)
            {
                var master =
                    await
                        Client.GitDatabase.Reference.Get(repository.Owner.Login, repository.Name, "heads/master")
                            .ConfigureAwait(false);

                var reference = new NewReference($"refs/heads/{branch.name}", master.Object.Sha);
                var createdReference = await
                    Client.GitDatabase.Reference.Create(repository.Owner.Login, repository.Name, reference)
                        .ConfigureAwait(false);
            }

            return repository.HtmlUrl;
        }
    }
}