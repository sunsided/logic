using System.Diagnostics.Contracts;

namespace Logic.TokenInterpreter.TokenTypes
{
    /// <summary>
    /// UND-Operator
    /// </summary>
    public sealed class AndOperator : BinaryOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndOperator"/> class.
        /// </summary>
        public AndOperator(TokenNode leftNode, TokenNode rightNode)
            : base(10, leftNode, rightNode)
        {
            Contract.Requires(leftNode != null, "Knoten darf nicht null sein");
            Contract.Requires(rightNode != null, "Knoten darf nicht null sein");
        }

        /// <summary>
        /// Wertet diesen Knoten aus
        /// </summary>
        /// <returns>
        /// Der aktuelle oder ermittelte Wahrheitswert des Knotens
        /// </returns>
        public override bool Evaluate()
        {
            return LeftNode.Evaluate() && RightNode.Evaluate();
        }
    }
}
