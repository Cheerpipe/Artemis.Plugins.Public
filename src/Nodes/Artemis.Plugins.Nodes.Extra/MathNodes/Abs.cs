using Artemis.Core;
using System;

namespace Artemis.Plugins.Nodes.Extra.MathNodes;

[Node("Abs", "Return absolute value", "Mathematics", InputType = typeof(Numeric), OutputType = typeof(Numeric))]
public class AbsNumericNode : Node
{
    #region Properties & Fields

    public InputPin<Numeric> Input { get; }
    public OutputPin<Numeric> Output { get; }

    #endregion

    #region Constructors

    public AbsNumericNode()
    {
        Input = CreateInputPin<Numeric>();
        Output = CreateOutputPin<Numeric>();
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public override void Evaluate()
    {

        Output.Value = Math.Abs(Input.Value);
    }

    #endregion
}