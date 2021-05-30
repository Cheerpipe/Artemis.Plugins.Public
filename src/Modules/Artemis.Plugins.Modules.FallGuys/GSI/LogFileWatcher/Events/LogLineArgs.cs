using System;

namespace FallGuys.LogFileWatcher.Events
{
    public delegate void NewLineEventHandler(object sender, LogLineArgs e);

    public class LogLineArgs : EventArgs
    {
        internal LogLineArgs(string line)
        {
            Line = line;
        }
        public string Line { get; }
    }
}
