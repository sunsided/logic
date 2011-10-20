namespace Logic.TokenInterpreter.TokenTypes
{
    /// <summary>
    /// Basisklasse für Token
    /// </summary>
    public abstract class TokenNode
    {
        /// <summary>
        /// Wertet diesen Knoten aus
        /// </summary>
        /// <returns>Der aktuelle oder ermittelte Wahrheitswert des Knotens</returns>
        public abstract bool Evaluate();
    }
}
