namespace Logic.Nodes
{
    /// <summary>
    /// Basisknoten für Token
    /// </summary>
    /// <remarks></remarks>
    public abstract class TokenNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        internal TokenNode() { }

        /// <summary>
        /// Wertet diesen Knoten aus
        /// </summary>
        /// <returns>Der Wahrheitswert dieses Knotens</returns>
        public abstract bool Evaluate();
    }
}
