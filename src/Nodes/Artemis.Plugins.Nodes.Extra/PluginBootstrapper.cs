using Artemis.Core;
using Artemis.Core.Services;
using Artemis.Plugins.Nodes.Extra.MathNodes;

namespace Artemis.Plugins.Nodes.MathExtra
{
    public class MathExtraPluginBootstrapper : PluginBootstrapper
    {
        public override void OnPluginEnabled(Plugin plugin)
        {
            INodeService _nodeService = plugin.Get<INodeService>();
            _nodeService.RegisterNodeType(plugin, typeof(DivideNumericsNode));
            _nodeService.RegisterNodeType(plugin, typeof(FullLerpNode));
            _nodeService.RegisterNodeType(plugin, typeof(MultiplyNode));
            _nodeService.RegisterNodeType(plugin, typeof(PercentageOfNode));
            _nodeService.RegisterNodeType(plugin, typeof(AbsNumericNode));
        }
    }
}
