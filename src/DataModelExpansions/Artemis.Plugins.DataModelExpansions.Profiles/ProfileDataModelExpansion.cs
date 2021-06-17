using Artemis.Core;
using Serilog;
using System;
using Artemis.Core.Modules;
using Artemis.Core.Services;
using Artemis.Plugins.DataModelExpansions.Profiles.DataModels;
using System.Linq;
using System.Collections.Generic;

namespace Artemis.Plugins.DataModelExpansions.Profiles
{
    public class DynamicChildComparer : IEqualityComparer<DynamicChild>
    {
        public bool Equals(DynamicChild x, DynamicChild y)
        {
            if (x.Key == y.Key)
                return true;
            return false;
        }

        public int GetHashCode(DynamicChild obj)
        {
            return obj.GetHashCode();
        }
    }
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
            UpdateProfilesDataDataModel();
            AddTimedUpdate(TimeSpan.FromSeconds(1), _ => UpdateProfilesDataDataModel(), "UpdateProfilesDataDataModel");
        }

        public override void Disable()
        {
        }

        public override void Update(double deltaTime) { }


        private void UpdateProfilesDataDataModel()
        {
            // Cleaning categories

            DataModel.DynamicChildren.Where(child => !_profileService.ProfileCategories.Any(cat => cat.Name == child.Key)).ToList().ForEach(k => DataModel.RemoveDynamicChild(k.Value));

            foreach (ProfileCategory profileCategory in _profileService.ProfileCategories)
            {
                // Set Category values
                string profileCategoryKey = profileCategory.Name;

                if (DataModel.TryGetDynamicChild(profileCategoryKey, out DynamicChild<ProfileCategoryDataModel> profileCategoryDataModelchild))
                {
                    // Update
                    profileCategoryDataModelchild.Value = new ProfileCategoryDataModel(profileCategory);
                }
                else
                {
                    // Create
                    profileCategoryDataModelchild = DataModel.AddDynamicChild
                    (
                        profileCategoryKey,
                        new ProfileCategoryDataModel(profileCategory)
                    );
                }

                // Cleaning categories profiles
                profileCategoryDataModelchild.Value.Profiles.DynamicChildren.Where(child => !profileCategory.ProfileConfigurations.Any(prof => prof.ProfileId.ToString() == child.Key)).ToList().ForEach(k => profileCategoryDataModelchild.Value.Profiles.RemoveDynamicChild(k.Value));

                // Set Profiles values
                foreach (ProfileConfiguration profileConfiguration in profileCategory.ProfileConfigurations)
                {
                    string profileConfigurationKey = profileConfiguration.ProfileId.ToString();

                    if (profileCategoryDataModelchild.Value.Profiles.TryGetDynamicChild(profileConfigurationKey, out DynamicChild<ProfileInformationDataModel> profileInformationDataModelChild))
                    {
                        // Update
                        profileInformationDataModelChild.Value = new ProfileInformationDataModel(profileConfiguration);
                    }
                    else
                    {
                        // Create
                        profileCategoryDataModelchild.Value.Profiles.AddDynamicChild
                        (
                            profileConfigurationKey,
                            new ProfileInformationDataModel(profileConfiguration),
                           profileConfiguration.Name
                        );
                    }
                }
            }
        }
    }
}