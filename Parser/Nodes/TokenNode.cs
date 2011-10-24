namespace Logic.Nodes
{
    public abstract class TokenNode
    {
        /// <summary>
        /// Wertet diesen Knoten aus
        /// </summary>
        /// <returns>Der Wahrheitswert dieses Knotens</returns>
        public abstract bool Evaluate();
    }
}
