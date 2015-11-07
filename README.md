# github-create-repository

Some code to automate new repository creation and configuration

[![Build status](https://ci.appveyor.com/api/projects/status/4sfsdv24y3x0u8kd?svg=true)](https://ci.appveyor.com/project/asizikov/github-create-repository)

```csharp
internal class Program
{
    private static void Main(string[] args)
    {
        Task.Run(() => TestAsync()).Wait();
    }

    public static async Task TestAsync()
    {
        var owner = "#";
        var password = "#";  //you can use access token (https://github.com/settings/tokens)
        var client = new GitHubClient(new ProductHeaderValue("tool"));
        client.Credentials = new Credentials(owner, password);
        var configuration = new ConfigurationProvider(owner, "github-create-repository-configuration");
        var config = await configuration.GetConfigurationAsync(client);
        System.Console.WriteLine(config.version);

        var task = new CreateNewRepositoryTask(client, config);
        var repository = await task.CreateAsync("hello-new-repo");
        System.Console.WriteLine("Browse the repository at: " + repository);
        System.Console.ReadLine();
    }
}
```