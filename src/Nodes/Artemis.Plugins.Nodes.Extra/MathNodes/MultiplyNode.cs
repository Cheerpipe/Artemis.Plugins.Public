﻿using Artemis.Core;
using System.Linq;

namespace Artemis.Plugins.Nodes.Extra.MathNodes;

[Node("Multiply", "Multiply values", "Mathematics", InputType = typeof(Numeric), OutputType = typeof(Numeric))]
public class MultiplyNode : Node
{
    #region Properties & Fields

    public InputPinCollection<Numeric> Values { get; }
    public OutputPin<Numeric> Multiply { get; }

    #endregion

    #region Constructors

    public MultiplyNode()
    {
        Values = CreateInputPinCollection<Numeric>("Values", 2);
        Multiply = CreateOutputPin<Numeric>("Output");
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public override void Evaluate()
    {
        Multiply.Value = Values.Values.Count() > 0 ? 1 : 0;
        foreach (var value in Values.Values)
        {
            Multiply.Value *= (float)value;
        }
    }

    #endregion
}