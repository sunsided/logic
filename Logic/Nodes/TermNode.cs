using System.Diagnostics.Contracts;

namespace Logic.Nodes
{
    /// <summary>
    /// Ein Term
    /// </summary>
    public sealed class TermNode : TokenNode
    {
        /// <summary>
        /// Der Match
        /// </summary>
        public TokenMatch Match { [Pure] get; set; }

        /// <summary>
        /// Der Wahrheitswert des Knotens
        /// </summary>
        public bool Value { [Pure] get; set; }

        /// <summary>
        /// Wertet diesen Knoten aus
        /// </summary>
        /// <returns>Der Wahrheitswert dieses Knotens</returns>
        public override bool Evaluate()
        {
            return Value;
        }
    }
}
