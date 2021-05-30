using FallGuys.LogFileWatcher.Abstract;
using FallGuys.LogFileWatcher.Events;
using System;
using System.IO;
using System.Threading;

namespace FallGuys.LogFileWatcher
{
    public class ThreadedLogFileWatcher: AbstractLogFileWatcher
    {
        private readonly string _logFilePath;
        private readonly string _logFileName;
        private readonly string _logFullName;
        private Thread _watchThread;
        private bool _running;

        public override event NewLineEventHandler NewLine;
        public override event EventHandler Started;
        public override event EventHandler Stopped;

        public ThreadedLogFileWatcher(string logFilePath, string logFileName)
        {
            _logFilePath = logFilePath;
            _logFileName = logFileName;
            _logFullName = Path.Combine(logFilePath, logFileName);
        }

        public string LogFilePath => _logFilePath;
        public string LogFileName => _logFileName;
        public string LogFullName => _logFullName;
        public bool Running => _running;

        private void Watch()
        {
            using FileStream fs = new(_logFullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader sr = new(fs);
            string streamLine = "";
            while (_running)
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
            _running = true;
            Started?.Invoke(this, new EventArgs());
        }

        public override void Stop()
        {
            _running = false;
            Stopped?.Invoke(this, new EventArgs());
        }
    }
}