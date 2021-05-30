using FallGuys.LogParser.LogLine.Base;

namespace FallGuys.LogParser.LogLine
{
    public class UnknownLogLine : BaseLogLine
    {
        public UnknownLogLine(string line) : base(line)
        {
            Data["State"] = "Unknown";
            IsIndefined = true;
        }
    }
}
