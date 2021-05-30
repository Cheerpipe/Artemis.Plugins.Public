using System;
using System.IO;

namespace FallGuys.Gsi
{
    public class Utils
    {
        public static string GetLogPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low", "Mediatonic", "FallGuys_client");
        }

        public static string GetLogFileName()
        {
            return "Player.log";
        }
    }
}
