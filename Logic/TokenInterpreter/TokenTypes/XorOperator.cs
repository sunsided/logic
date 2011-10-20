using System.Diagnostics.Contracts;

namespace Logic.TokenInterpreter.TokenTypes
{
    /// <summary>
    /// XOR-Operator
    /// </summary>
    public sealed class XorOperator : BinaryOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XorOperator"/> class.
        /// </summary>
        public XorOperator(TokenNode leftNode, TokenNode rightNode)
            : base(6, leftNode, rightNode)
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
            return LeftNode.Evaluate() ^ RightNode.Evaluate();
        }
    }
}
