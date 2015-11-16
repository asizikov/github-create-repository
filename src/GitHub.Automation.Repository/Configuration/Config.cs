using System.Collections.Generic;

namespace GitHub.Automation.Repository.Configuration
{
    public class Branch
    {
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsProtected { get; set; }
    }

    public class Label
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }

    public class Config
    {
        public bool IsPrivate { get; set; }
        public string GitIgnoreTemplate { get; set; }
        public List<Branch> Branches { get; set; }
        public List<Label> Labels { get; set; }
        public string Organization { get; set; }
    }

    public class NewRepositoryConfiguration
    {
        public string Version { get; set; }
        public Config Repository { get; set; }
    }
}