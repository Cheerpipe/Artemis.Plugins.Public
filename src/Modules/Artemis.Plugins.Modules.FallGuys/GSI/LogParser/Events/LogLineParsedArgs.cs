using FallGuys.LogParser.LogLine.Base;
using System;

namespace FallGuys.LogParser.Events
{
    public delegate void LogLineParsedEventHandler(object sender, LogLineParsedArgs e);

    public class LogLineParsedArgs : EventArgs
    {
        internal LogLineParsedArgs(BaseLogLine logLine)
        {
            LogLine = logLine;
        }

        public BaseLogLine LogLine { get; }
    }
}
