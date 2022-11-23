using Artemis.Core;

namespace Artemis.Plugins.Nodes.Extra.MathNodes;

[Node("Linear interpolation", "Uses linear interpolation to find an equivalent value", "Mathematics", InputType = typeof(Numeric), OutputType = typeof(Numeric))]
public class FullLerpNode : Node
{
    #region Properties & Fields

    public InputPin<Numeric> x1 { get; }
    public InputPin<Numeric> x2 { get; }
    public InputPin<Numeric> x3 { get; }
    public InputPin<Numeric> y1 { get; }
    public InputPin<Numeric> y3 { get; }

    public OutputPin<Numeric> y2 { get; }

    #endregion

    #region Constructors

    public FullLerpNode()
    {
        x1 = CreateInputPin<Numeric>("x1");
        x2 = CreateInputPin<Numeric>("x2");
        x3 = CreateInputPin<Numeric>("x3");
        y1 = CreateInputPin<Numeric>("y1");
        y3 = CreateInputPin<Numeric>("y3");

        y2 = CreateOutputPin<Numeric>("y2");
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public override void Evaluate()
    {
        float fx1 = x1.Value;
        float fx2 = x2.Value;
        float fx3 = x3.Value;
        float fy1 = y1.Value;
        float fy3 = y3.Value;

        y2.Value = fy1 + ((fx2 - fx1) * (fy3 - fy1)) / (fx3 - fx1);
    }

    #endregion
}