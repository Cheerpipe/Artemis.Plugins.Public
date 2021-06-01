using FallGuys.LogFileWatcher;
using FallGuys.LogFileWatcher.Abstract;
using FallGuys.LogFileWatcher.Events;
using FallGuys.LogParser.Events;
using FallGuys.LogParser.LogLine.Factory;
using System;
using System.IO;
using System.Timers;

namespace FallGuys.LogParser
{
    public class LogParser
    {
        private readonly string _logFilePath;
        private readonly string _logFullPath;
        private readonly AbstractLogFileWatcher _fileWatcher;
        private readonly Timer _checkNewFileTimer;
        private long _logFileSize;
        public event LogLineParsedEventHandler LogLineParsed;
        public event EventHandler LogFileRestarted;
        public event EventHandler ParserStopped;
        public event EventHandler ParserStarted;
        public event EventHandler ParserRestarted;
        private bool _isRunning;

        public LogParser(string logFilePath, string logFileName)
        {
            _logFilePath = logFilePath;
            LogFileName = logFileName;
            _logFullPath = Path.Combine(logFilePath, logFileName);

            _checkNewFileTimer = new Timer(1000);
            _checkNewFileTimer.Elapsed += CheckNewFileTimer_Elapsed;

            _fileWatcher = new ThreadedLogFileWatcher(_logFilePath, logFileName);
            _fileWatcher.NewLine += FileWatcher_NewLine;
        }

        public string LogFilePath => _logFilePath;
        public string LogFileName { get; }

        public string LogFullPath => _logFullPath;
        public long LogFileSize => _logFileSize;

        private void FileWatcher_NewLine(object sender, LogLineArgs e)
        {
            var line = LogLineFactory.CreateLogLine(e.Line);

            if (line == null)
                return;

            LogLineParsed?.Invoke(this, new LogLineParsedArgs(line));
        }

        public void Start()
        {
            _fileWatcher.Start();
            FileInfo fi = new(_logFullPath);
            _logFileSize = fi.Length;
            _checkNewFileTimer.Start();
            _isRunning = true;
        }

        public void Stop()
        {
            _checkNewFileTimer.Stop();
            _fileWatcher.Stop();
            _isRunning = false;
        }

        public bool IsRunning => _isRunning;

        private void CheckNewFileTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            FileInfo fi = new(_logFullPath);
            long newFileSize = fi.Length;

            if (newFileSize == 0)
                return;

            if (newFileSize < _logFileSize)
            {
                LogFileRestarted?.Invoke(this, new EventArgs());
                Stop();
                ParserStopped?.Invoke(this, new EventArgs());
                Start();
                ParserStarted?.Invoke(this, new EventArgs());
                ParserRestarted?.Invoke(this, new EventArgs());
            }
            else
                _logFileSize = newFileSize;
        }
    }
}
