using Artemis.Core;

namespace Artemis.Plugins.Nodes.Extra.MathNodes;

[Node("Divide", "Divide the connected numeric values.", "Mathematics", InputType = typeof(Numeric), OutputType = typeof(Numeric))]
public class DivideNumericsNode : Node
{

    #region Properties & Fields

    public InputPin<Numeric> A { get; }
    public InputPin<Numeric> B { get; }

    public OutputPin<Numeric> Result { get; }

    #endregion

    #region Constructors

    public DivideNumericsNode()
    {
        A = CreateInputPin<Numeric>("A");
        B = CreateInputPin<Numeric>("B");

        Result = CreateOutputPin<Numeric>();
    }

    #endregion

    #region Methods

    public override void Evaluate()
    {
        Result.Value = (float)A.Value / (float)B.Value;
    }

    #endregion
}