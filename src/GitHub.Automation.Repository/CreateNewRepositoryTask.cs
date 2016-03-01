using System;
using System.Linq;
using System.Threading.Tasks;
using GitHub.Automation.Repository.Configuration;
using JetBrains.Annotations;
using Octokit;

namespace GitHub.Automation.Repository
{
    public class CreateNewRepositoryTask
    {
        private IGitHubClient Client { get; }
        private NewRepositoryConfiguration Configuration { get; }

        public CreateNewRepositoryTask(
            [NotNull] IGitHubClient client,
            [NotNull] NewRepositoryConfiguration configuration)
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

        public async Task<string> CreateAsync([NotNull] string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            var repository = await CreateRepositoryAsync(name).ConfigureAwait(false);

            await CreateLabelsAsync(repository).ConfigureAwait(false);

            await CreateBranchesAsync(repository).ConfigureAwait(false);

            return repository.HtmlUrl;
        }

        private async Task CreateBranchesAsync(Octokit.Repository repository)
        {
            foreach (var branch in Configuration.Repository.Branches)
            {
                var master =
                    await
                        Client.Git.Reference.Get(repository.Owner.Login, repository.Name, "heads/master")
                            .ConfigureAwait(false);

                var reference = new NewReference($"refs/heads/{branch.Name}", master.Object.Sha);
                await
                    Client.Git.Reference.Create(repository.Owner.Login, repository.Name, reference)
                        .ConfigureAwait(false);
                if (branch.IsDefault)
                {
                    var repositoryUpdate = new RepositoryUpdate { Name = repository.Name, DefaultBranch = branch.Name };
                    await
                        Client.Repository.Edit(repository.Owner.Login, repository.Name, repositoryUpdate)
                            .ConfigureAwait(false);
                }
            }
        }

        private async Task CreateLabelsAsync(Octokit.Repository repository)
        {
            var newLabels =
                Configuration.Repository.Labels.Select(label => new NewLabel(label.Name, label.Color)).ToList();
            foreach (var newLabel in newLabels)
            {
                await
                    Client.Issue.Labels.Create(repository.Owner.Login, repository.Name, newLabel).ConfigureAwait(false);
            }
        }

        private async Task<Octokit.Repository> CreateRepositoryAsync(string name)
        {
            var newRepo = new NewRepository(name)
            {
                AutoInit = true,
                GitignoreTemplate = Configuration.Repository.GitIgnoreTemplate,
                Private = Configuration.Repository.IsPrivate
            };

            Octokit.Repository repository;
            if (string.IsNullOrEmpty(Configuration.Repository.Organization))
            {
                repository =
                    await Client.Repository.Create(Configuration.Repository.Organization, newRepo).ConfigureAwait(false);
            }
            else
            {
                repository = await Client.Repository.Create(newRepo).ConfigureAwait(false);
            }
            return repository;
        }
    }
}