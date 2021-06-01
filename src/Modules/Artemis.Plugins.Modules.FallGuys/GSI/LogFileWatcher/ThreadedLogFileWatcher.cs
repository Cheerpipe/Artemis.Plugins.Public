using FallGuys.LogFileWatcher.Abstract;
using FallGuys.LogFileWatcher.Events;
using System;
using System.IO;
using System.Threading;

namespace FallGuys.LogFileWatcher
{
    public class ThreadedLogFileWatcher: AbstractLogFileWatcher
    {
        private Thread _watchThread;

        public override event NewLineEventHandler NewLine;
        public override event EventHandler Started;
        public override event EventHandler Stopped;

        public ThreadedLogFileWatcher(string logFilePath, string logFileName)
        {
            LogFilePath = logFilePath;
            LogFileName = logFileName;
            LogFullName = Path.Combine(logFilePath, logFileName);
        }

        public string LogFilePath { get; }

        public string LogFileName { get; }

        public string LogFullName { get; }

        public bool Running { get; private set; }

        private void Watch()
        {
            using FileStream fs = new(LogFullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader sr = new(fs);
            string streamLine;
            while (Running)
            {
                streamLine = sr.ReadLine();
                if (streamLine != null)
                {
                    NewLine?.Invoke(this, new LogLineArgs(streamLine));
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }

        public override void Start()
        {
            _watchThread = new Thread(Watch);
            _watchThread.Start();
            Running = true;
            Started?.Invoke(this, new EventArgs());
        }

        public override void Stop()
        {
            Running = false;
            Stopped?.Invoke(this, new EventArgs());
        }
    }
}