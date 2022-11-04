﻿// Based on https://github.com/pathartl/TeamsPresence

using Artemis.Plugins.DataModelExpansions.Teams.Enums;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;


namespace Artemis.Plugins.DataModelExpansions.Teams.TeamsPresence
{
    public class TeamsStateReader
    {
        public event EventHandler<TeamsStatus> StatusChanged;
        public event EventHandler<TeamsActivity> ActivityChanged;

        private TeamsStatus CurrentStatus;
        private TeamsActivity CurrentActivity;
        private bool Started = false;
        private bool Stopped = false;

        private Stopwatch Stopwatch { get; set; }
        private string LogPath { get; set; }

        public TeamsStateReader()
        {
            LogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "Teams");
            Stopwatch = new Stopwatch();
        }

        public TeamsStateReader(string logPath)
        {
            LogPath = logPath;
            Stopwatch = new Stopwatch();
        }

        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(ThreadedWork));
            thread.IsBackground = true;
            thread.Start();
        }

        private void ThreadedWork()
        {
            Stopped = false;
            Stopwatch.Start();

            var lockMe = new object();
            using (var latch = new ManualResetEvent(true))
            using (var fs = new FileStream(Path.Combine(LogPath, "logs.txt"), FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            using (var fsw = new FileSystemWatcher(LogPath))
            {
                fsw.Changed += (s, e) =>
                {
                    lock (lockMe)
                    {
                        if (e.FullPath != Path.Combine(LogPath, "logs.txt")) return;
                        latch.Set();
                    }
                };

                using (var sr = new StreamReader(fs))
                {
                    while (true)
                    {
                        Thread.Sleep(100);

                        // Throttle based on the stopwatch so we're not sending
                        // tons of updates to Home Assistant.
                        if (Stopwatch.Elapsed.TotalSeconds > 2)
                        {
                            Stopwatch.Stop();

                            if (Started == false)
                            {
                                Started = true;
                                StatusChanged?.Invoke(this, CurrentStatus);
                                ActivityChanged?.Invoke(this, CurrentActivity);
                            }
                        }

                        latch.WaitOne();
                        lock (lockMe)
                        {
                            String line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                LogFileChanged(line, Stopwatch.IsRunning);
                            }
                            latch.Set();
                        }
                        if (Stopped)
                            break;
                    }
                }
            }
        }


        public void Stop()
        {
            Stopwatch.Stop();
            Stopped = true;
        }

        private void LogFileChanged(string line, bool throttled)
        {
            string statusPattern = @"StatusIndicatorStateService: Added (\w+)";
            string activityPattern = @"name: desktop_call_state_change_send, isOngoing: (\w+)";

            RegexOptions options = RegexOptions.Multiline;

            TeamsStatus status = TeamsStatus.Unknown;
            TeamsActivity activity = TeamsActivity.Unknown;

            foreach (Match m in Regex.Matches(line, statusPattern, options))
            {
                if (m.Groups[1].Value != "NewActivity")
                {
                    Enum.TryParse<TeamsStatus>(m.Groups[1].Value, out status);

                    CurrentStatus = status;

                    if (!throttled)
                        StatusChanged?.Invoke(this, status);
                }
            }

            foreach (Match m in Regex.Matches(line, activityPattern, options))
            {
                activity = m.Groups[1].Value == "true" ? TeamsActivity.InACall : TeamsActivity.NotInACall;

                CurrentActivity = activity;

                if (!throttled)
                    ActivityChanged?.Invoke(this, activity);
            }
        }
    }
}
