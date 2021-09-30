using System;
using System.Linq;
using WindowsDisplayAPI.DisplayConfig;

namespace Artemis.Plugins.DataModelExpansions.DisplaySettings.Custom
{
    public enum CustomDisplayMode
    {
        Single,                                     // OK
        ExtendedHorizontal,                         // OK
        ExtendedAll,                                // OK
        ExtendedHorizontalDuplicatedVertical,       // OK
        Tv,                                         // OK
        ExtendedSingle,
        DuplicatedSingle,
        Unknown
    }


    public class CustomDisplayModeDetector
    {
        private const string TvDeviceName= "LG TV";
        private const string MonitorDeviceName = "VG27A";
        
        public static CustomDisplayMode GetCurrentMode()
        {
            // TODO: Use individual devices to get configs.
            try
            {
                var paths = PathInfo.GetActivePaths();

                if (paths.Count(p => p.IsInUse) == 3) // Hay tres en uso
                    return CustomDisplayMode.ExtendedAll;

                if (paths.Count(p => p.IsInUse) == 2 && paths.Count(p => p.IsCloneMember == false && p.IsInUse && p.TargetsInfo[0].DisplayTarget.FriendlyName == MonitorDeviceName && p.IsInUse && p.TargetsInfo.Length == 1) == 2)
                    return CustomDisplayMode.ExtendedHorizontal;

                if (paths.Count(p => p.IsInUse) == 1 && paths.Count(p => p.IsCloneMember == false && p.IsInUse && p.TargetsInfo[0].DisplayTarget.FriendlyName == MonitorDeviceName) == 1)
                    return CustomDisplayMode.Single;

                if (paths.Count(p => p.IsInUse) == 1 && paths.Count(p => p.IsCloneMember == false && p.IsInUse && p.TargetsInfo[0].DisplayTarget.FriendlyName == TvDeviceName) == 1)
                    return CustomDisplayMode.Tv;

                if (paths.Count(p => p.IsInUse) == 2 && paths.Count(p => p.IsCloneMember == false && p.IsInUse && p.TargetsInfo.Length == 2) == 1 && paths.Count(p => p.IsCloneMember == false && p.IsInUse && p.TargetsInfo.Length == 1) == 1)
                    return CustomDisplayMode.ExtendedHorizontalDuplicatedVertical;

                return CustomDisplayMode.Unknown;

            }
            catch (Exception)
            {
                return CustomDisplayMode.Unknown;
            }
        }
    }
}
