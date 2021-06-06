using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SRTPluginProviderRE7
{
    public enum GameVersion
    {
        STEAM,
        WINDOWS,
        UNKNOWN
    }

    public static class GameHashes
    {
        public static GameVersion DetectVersion(string filePath)
        {
            if (filePath.Contains("Windows"))
            {
                return GameVersion.WINDOWS;
            }
            else
            {
                return GameVersion.STEAM;
            }
        }
    }
}