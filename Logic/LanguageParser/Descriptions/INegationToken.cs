namespace Logic.LanguageParser.Descriptions
{
    public interface INegationToken
    {
        /// <summary>
        /// Gibt an, ob es sich um eine Prefix-Negation handelt
        /// </summary>
        bool IsPrefix { get; }
    }
}