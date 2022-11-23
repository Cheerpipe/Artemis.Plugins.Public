using Artemis.Core;

namespace Artemis.Plugins.Nodes.Extra.MathNodes;

[Node("Percentage of Node", "", "Mathematics", InputType = typeof(Numeric), OutputType = typeof(Numeric))]
public class PercentageOfNode : Node
{
    #region Properties & Fields

    public InputPin<Numeric> A { get; }
    public InputPin<Numeric> B { get; }

    public OutputPin<Numeric> Percentage { get; }
    public OutputPin<Numeric> Normalized { get; }

    #endregion

    #region Constructors

    public PercentageOfNode()
    {
        A = CreateInputPin<Numeric>("A");
        B = CreateInputPin<Numeric>("B");

        Percentage = CreateOutputPin<Numeric>("Percentaje");
        Normalized = CreateOutputPin<Numeric>("Normalized");
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public override void Evaluate()
    {
        float a = A.Value;
        float b = B.Value;
        Percentage.Value = a / b * 100;
        Normalized.Value = a / b;
    }

    #endregion
}