using Artemis.Core;
using Artemis.Core.Modules;
using System;
using System.Linq;

namespace Artemis.Plugins.DataModelExpansions.Profiles.DataModels
{
    public class ProfileCategoryDataModel : DataModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsSuspended { get; set; }
        public bool HasActiveProfiles { get; set; }
        public ProfilesDataModel Profiles { get; set; } = new();

        public ProfileCategoryDataModel(ProfileCategory category)
        {
            Id = category.EntityId;
            Name = category.Name;
            Order = category.Order;
            IsSuspended = category.IsSuspended;
            HasActiveProfiles = category.ProfileConfigurations.Any(p => !p.IsSuspended && p.ActivationConditionMet);
        }
    }
 }