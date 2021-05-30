
namespace FallGuys.LogParser.Abstract
{
    public abstract class AbstractLogLine
    {
        public abstract void Parse(string line);
        protected abstract void ParsePayoad(string line);
    }
}
