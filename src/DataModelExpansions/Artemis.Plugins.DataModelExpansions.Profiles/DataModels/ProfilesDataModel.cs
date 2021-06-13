using Artemis.Core.Modules;

namespace Artemis.Plugins.DataModelExpansions.Profiles.DataModels
{
    public class ProfilesDataModel : DataModel
    {
    }

    public class BasicCategoryInformation : DataModel
    {
        public string Name { get; set; }
        public bool IsSuspended { get; set; }
        public int Order { get; set; }
        public bool HasActiveProfiles { get; set; }
        public Profiles Profiles { get; set; } = new();
    }

    public class Profiles : DataModel { }

    public class BasicProfileInformation : DataModel
    {
        public string Name { get; set; }
        public bool IsSuspended { get; set; }
        public bool ActivationConditionMet { get; set; }
        public int Order { get; set; }
    }
}