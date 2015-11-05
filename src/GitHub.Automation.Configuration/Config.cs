using System.Collections.Generic;

namespace GitHub.Automation.Configuration
{
    public class Branch
    {
        public string name { get; set; }
        public bool @default { get; set; }
        public bool @protected { get; set; }
    }

    public class Label
    {
        public string name { get; set; }
        public string color { get; set; }
    }

    public class Config
    {
        public List<Branch> branches { get; set; }
        public List<Label> labels { get; set; }
    }

    public class NewRepositoryConfiguration
    {
        public double version { get; set; }
        public Config repository { get; set; }
    }
}