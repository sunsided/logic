namespace Logic.TokenInterpreter.TokenTypes
{
    /// <summary>
    /// Ausdruck
    /// </summary>
    public sealed class Term : TokenNode
    {
        /// <summary>
        /// Der Wert des Ausdruckes
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// Liefert den Wert
        /// </summary>
        /// <returns>Der Wert von <see cref="Value"/></returns>
        public override bool Evaluate()
        {
            return Value;
        }
    }
}
