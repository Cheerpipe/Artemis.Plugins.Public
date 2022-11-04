using Artemis.Plugins.DataModelExpansions.Teams.Enums;
using Microsoft.Win32;
using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace Artemis.Plugins.DataModelExpansions.Teams.TeamsPresence
{
    public class CameraStatusChangedEventArgs : EventArgs
    {
        public CameraStatus Status { get; set; }
        public string AppName { get; set; }
    }

    public class CameraDetectionService
    {
        private const string SubKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam";
        private const string AppNamePattern = @"_[\d|\w]{13}$";

        public event EventHandler<CameraStatusChangedEventArgs> StatusChanged;

        private int PollingRate;
        private string ActiveAppName = "";

        private bool Stopped = false;

        public CameraDetectionService(int pollingRate)
        {
            PollingRate = pollingRate;
        }

        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(ThreadedWork));
            thread.IsBackground = true;
            thread.Name = "My Worker.";
            thread.Start();
        }

        private void ThreadedWork()
        {

            Stopped = false;
            var appNameRegex = new Regex(AppNamePattern);

            while (true)
            {
                var activeCameraApp = GetActiveCameraApp();

                if (activeCameraApp != null)
                    activeCameraApp = appNameRegex.Replace(activeCameraApp, "", 1);

                if (activeCameraApp != ActiveAppName)
                {
                    ActiveAppName = activeCameraApp;

                    StatusChanged?.Invoke(this, new CameraStatusChangedEventArgs()
                    {
                        Status = ActiveAppName == "" ? CameraStatus.Inactive : CameraStatus.Active,
                        AppName = ActiveAppName
                    });
                }

                Thread.Sleep(PollingRate);
                if (Stopped)
                    break;
            }
        }

        public void Stop()
        {
            Stopped = true;
        }

        private string GetActiveCameraApp()
        {
            var key = Registry.CurrentUser.OpenSubKey(SubKey);

            foreach (var app in key.GetSubKeyNames())
            {
                var lastUsedTimeStop = Registry.CurrentUser.OpenSubKey($@"{SubKey}\{app}")?.GetValue("LastUsedTimeStop");

                if (lastUsedTimeStop != null && (long)lastUsedTimeStop == 0)
                {
                    return app;
                }
            }

            return "";
        }
    }
}
