using System.Diagnostics.Contracts;

namespace Logic.TokenInterpreter.TokenTypes
{
    /// <summary>
    /// NAND-Operator
    /// </summary>
    public sealed class NandOperator : BinaryOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NandOperator"/> class.
        /// </summary>
        public NandOperator(TokenNode leftNode, TokenNode rightNode)
            : base(9, leftNode, rightNode)
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
            return !(LeftNode.Evaluate() && RightNode.Evaluate());
        }
    }
}
