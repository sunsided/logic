using System.Diagnostics.Contracts;

namespace Logic.TokenInterpreter.TokenTypes
{
    /// <summary>
    /// NOT-Operator
    /// </summary>
    public sealed class NotOperator : UnaryOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndOperator"/> class.
        /// </summary>
        public NotOperator(TokenNode childNode)
            : base(childNode)
        {
            Contract.Requires(childNode != null, "Knoten darf nicht null sein");
        }

        /// <summary>
        /// Wertet diesen Knoten aus
        /// </summary>
        /// <returns>
        /// Der ermittelte Wahrheitswert des Knotens
        /// </returns>
        public override bool Evaluate()
        {
            return !(ChildNode.Evaluate());
        }
    }
}
