using FallGuys.LogParser.Abstract;
using FallGuys.LogParser.Enums.Context;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FallGuys.LogParser.LogLine.Base
{
    public class BaseLogLine : AbstractLogLine
    {
        private const string contextPattern = @"\[(.*?)\]";

        public TimeSpan Time { get; protected set; }
        public LogLineContext Context { get; protected set; }
        public string Payload { get; protected set; }
        public bool IsValid { get; protected set; }
        public Dictionary<string, string> Data { get; protected set; }
        public string Line { get; protected set; }
        public bool IsIndefined { get; protected set; }

        public BaseLogLine(string line)
        {
            Line = line;
            Parse(Line);
            Data = new Dictionary<string, string>();
        }

        public BaseLogLine(BaseLogLine logline)
        {
            Time = logline.Time;
            Context = logline.Context;
            Payload = logline.Payload;
            IsValid = logline.IsValid;
            Line = logline.Line;
            Data = new Dictionary<string, string>();
            PopulateDataList();
        }

        private void PopulateDataList()
        {

        }

        public override void Parse(string line)
        {
            TimeSpan time = TimeSpan.Zero;

            // Check for a valid log line
            if (line.Length < 13)
            {
                IsValid = false;
            }
            else if (!TimeSpan.TryParse(line.Substring(0, 12), out time))
            {
                IsValid = false;
            }
            else
            {
                IsValid = true;
            }

            if (IsValid)
            {
                // Set Time
                Time = time;

                // Set Context
                Regex contextRegex = new(contextPattern);
                Group contextResult = contextRegex.Match(line).Groups[1];
                string contextString = contextResult.Value;
                int contextStringOffset = 0;
                if (Enum.TryParse(contextString, out LogLineContext gameContext) && Enum.IsDefined(typeof(LogLineContext), contextString))
                {
                    Context = gameContext;
                    contextStringOffset = contextResult.Length + 1;
                }
                else
                {
                    Context = LogLineContext.Unknown;
                }

                // Set Payload
                Payload = line.Remove(0, contextResult.Index + contextStringOffset).Trim();
            }
            else
            {
                Time = TimeSpan.Zero;
                Context = LogLineContext.Unknown;
                Payload = line.Trim();
            }
        }

        public override string ToString()
        {
            return $"Valid: {{{IsValid}}} Time: {{{Time}}} Context: {{{Context}}} Payload {{{Payload}}}";
        }

        protected override void ParsePayoad(string line) { }
    }
}
