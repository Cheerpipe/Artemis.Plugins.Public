using Artemis.Core;
using Artemis.Core.Nodes;
using Artemis.Core.Services;
using Artemis.Plugins.Nodes.Extra.MathNodes;

namespace Artemis.Plugins.Nodes.MathExtra
{
    public class MathExtraNodeProvider : NodeProvider
    {
        public override void Enable()
        {
            RegisterNodeType<DivideNumericsNode>();
            RegisterNodeType<FullLerpNode>();
            RegisterNodeType<MultiplyNode>();
            RegisterNodeType<PercentageOfNode>();
            RegisterNodeType<AbsNumericNode>();
        }

        public override void Disable()
        {
        }
    }
}
