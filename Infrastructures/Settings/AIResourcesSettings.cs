namespace Infrastructures.Settings
{
    public class RoleResources
    {
        public List<string> Files { get; set; } = new();
        public List<string> Urls { get; set; } = new();
    }

    public class AIResourcesSettings
    {
        public Dictionary<string, RoleResources> Roles { get; set; } = new();
    }

}
