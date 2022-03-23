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

                if (paths.Count(p => p.IsInUse) == 1 && paths[0].TargetsInfo.Count(i => i.DisplayTarget.FriendlyName == "VG27A") == 1 && paths[0].TargetsInfo.Count(i => i.DisplayTarget.FriendlyName == "LG TV") == 1)// Hay uno en uso con un monitor y la tv
                    return CustomDisplayMode.DuplicatedSingle;

                if (paths.Count(p => p.IsInUse) == 3)
                    return CustomDisplayMode.ExtendedAll;

                if (paths.Count(p => p.IsInUse) == 2 && paths.Count(p => p.IsCloneMember == false && p.IsInUse && p.TargetsInfo[0].DisplayTarget.FriendlyName == "VG27A" && p.IsInUse && p.TargetsInfo.Length == 1) == 2) //Hay dos en uso y cada uno tiene un único path que es un monitor
                    return CustomDisplayMode.ExtendedHorizontal;

                if (paths.Count(p => p.IsInUse) == 1 && paths.Count(p => p.IsCloneMember == false && p.IsInUse && p.TargetsInfo[0].DisplayTarget.FriendlyName == "VG27A") == 1) // Hay uno en uso y es un monitor
                    return CustomDisplayMode.Single;

                if (paths.Count(p => p.IsInUse) == 1 && paths.Count(p => p.IsCloneMember == false && p.IsInUse && p.TargetsInfo[0].DisplayTarget.FriendlyName == "LG TV") == 1) // Hay uno en uso y es la TV
                    return CustomDisplayMode.Tv;

                if ((paths.Count(p => p.IsInUse) == 2 && paths.Count(p => p.IsCloneMember == false && p.IsInUse && p.TargetsInfo[0].DisplayTarget.FriendlyName == "VG27A") == 1) && (paths.Count(p => p.IsInUse) == 2 && paths.Count(p => p.IsCloneMember == false && p.IsInUse && p.TargetsInfo[0].DisplayTarget.FriendlyName == "LG TV") == 1))
                    return CustomDisplayMode.ExtendedSingle;

                if (paths.Count(p => p.IsInUse) == 2 && paths.Count(p => p.IsCloneMember == false && p.IsInUse && p.TargetsInfo.Length == 2) == 1 && paths.Count(p => p.IsCloneMember == false && p.IsInUse && p.TargetsInfo.Length == 1) == 1) // Hay dos en uso y uno de los dos tiene dos monitores
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
