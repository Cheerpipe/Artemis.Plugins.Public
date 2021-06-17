using Artemis.Core;
using Artemis.Core.Modules;
using System;

namespace Artemis.Plugins.DataModelExpansions.Profiles.DataModels
{
    public class ProfileInformationDataModel : DataModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsBeingEdited { get; set; }
        public bool IsMissingModule { get; set; }
        public bool HasActivationCondition { get; set; }
        public bool ActivationConditionMet { get; set; }

        public ProfileInformationDataModel(ProfileConfiguration profileConfiguration)
        {
            Id = profileConfiguration.ProfileId;
            Name = profileConfiguration.Name;
            Order = profileConfiguration.Order;
            IsActive = profileConfiguration.Profile != null;
            IsSuspended = profileConfiguration.IsSuspended;
            IsBeingEdited = profileConfiguration.IsBeingEdited;
            IsMissingModule = profileConfiguration.IsMissingModule;
            HasActivationCondition = profileConfiguration.ActivationCondition != null;
            ActivationConditionMet = profileConfiguration.ActivationConditionMet;
        }
    }
}