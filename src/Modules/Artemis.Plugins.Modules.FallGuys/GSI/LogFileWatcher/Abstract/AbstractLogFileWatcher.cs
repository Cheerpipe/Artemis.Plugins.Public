using FallGuys.LogFileWatcher.Events;
using FallGuys.LogFileWatcher.Interfaces;
using System;

namespace FallGuys.LogFileWatcher.Abstract
{
    public abstract class AbstractLogFileWatcher : ILogFileWatcher
    {
        public abstract void Start();
        public abstract void Stop();

        public abstract event NewLineEventHandler NewLine;
        public abstract event EventHandler Started;
        public abstract event EventHandler Stopped;
    }
}
