using Artemis.Core;
using Serilog;
using System;
using Artemis.Core.Modules;
using Artemis.Core.Services;
using Artemis.Plugins.DataModelExpansions.Profiles.DataModels;
using Artemis.Core.DataModelExpansions;
using System.Linq;
using System.Collections.Generic;

namespace Artemis.Plugins.DataModelExpansions.Profiles
{
    public class ProfilesDataModelExpansion : Module<ProfilesDataModel>
    {
        private readonly IProfileService _profileService;

        public ProfilesDataModelExpansion(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public override List<IModuleActivationRequirement> ActivationRequirements => null;

        public override void Enable()
        {
            AddTimedUpdate(TimeSpan.FromSeconds(1), _ => UpdateProfilesDataDataModel(), "UpdateProfilesDataDataModel");
        }

        public override void Disable()
        {
        }

        public override void Update(double deltaTime) { }

        private void UpdateProfilesDataDataModel()
        {
            // Cleaning categories
            DataModel.DynamicChildren.Select(c => c.Key).Except(_profileService.ProfileCategories.Select(c => c.Name)).ToList().ForEach(k => DataModel.RemoveDynamicChildByKey(k));

            foreach (ProfileCategory profileCategory in _profileService.ProfileCategories)
            {
                // Set Category values
                string profileCategoryKey = profileCategory.Name;

                if (DataModel.TryGetDynamicChild(profileCategoryKey, out DynamicChild<BasicCategoryInformation> basicCategoryInformation))
                {
                    // Update
                    basicCategoryInformation.Value.Name = profileCategory.Name;
                    basicCategoryInformation.Value.Order = profileCategory.Order;
                    basicCategoryInformation.Value.IsSuspended = profileCategory.IsSuspended;
                }
                else
                {
                    // Create
                    basicCategoryInformation = DataModel.AddDynamicChild
                    (
                        profileCategoryKey,
                        new BasicCategoryInformation()
                        {
                            Name = profileCategory.Name,
                            Order = profileCategory.Order,
                            IsSuspended = profileCategory.IsSuspended,
                        }
                    );
                }

                // Cleaning categories profiles
                basicCategoryInformation.Value.Profiles.DynamicChildren.Select(c => c.Key).Except(profileCategory.ProfileConfigurations.Select(c => $"{c.Name} ({c.Entity.ProfileId})")).ToList().ForEach(k => basicCategoryInformation.Value.Profiles.RemoveDynamicChildByKey(k));

                // Set Profiles values
                bool hasActiveProfiles = false;
                foreach (ProfileConfiguration profileConfiguration in profileCategory.ProfileConfigurations)
                {
                    string profileConfigurationKey = $"{profileConfiguration.Name} ({profileConfiguration.Entity.ProfileId})";

                    if (basicCategoryInformation.Value.Profiles.TryGetDynamicChild(profileConfigurationKey, out DynamicChild<BasicProfileInformation> basicProfileInformation))
                    {
                        // Update
                        basicProfileInformation.Value.Name = profileConfiguration.Name;
                        basicProfileInformation.Value.Order = profileConfiguration.Order;
                        basicProfileInformation.Value.IsSuspended = profileConfiguration.IsSuspended;
                        basicProfileInformation.Value.ActivationConditionMet = profileConfiguration.ActivationConditionMet;
                    }
                    else
                    {
                        // Create
                        basicCategoryInformation.Value.Profiles.AddDynamicChild
                        (
                            profileConfigurationKey,
                            new BasicProfileInformation()
                            {
                                Name = profileConfiguration.Name,
                                Order = profileConfiguration.Order,
                                IsSuspended = profileConfiguration.IsSuspended,
                                ActivationConditionMet = profileConfiguration.ActivationConditionMet,
                            }
                        );
                    }
                    hasActiveProfiles = hasActiveProfiles || (!profileConfiguration.IsSuspended && profileConfiguration.ActivationConditionMet);
                }
                basicCategoryInformation.Value.HasActiveProfiles = hasActiveProfiles;
            }
        }
    }
}