using System.Diagnostics.Contracts;

namespace Logic
{
    public interface ITokenDescription
    {
        /// <summary>
        /// Beschreibungstext
        /// </summary>
        string Description { [Pure] get; }

        /// <summary>
        /// Typ des Tokens
        /// </summary>
        TokenType Type { [Pure] get; }

        /// <summary>
        /// Bezieht einen Override
        /// </summary>
        /// <param name="newDescription">Die neue Beschreibung</param>
        /// <returns>Der Override</returns>
        ITokenDescription GetOverride(TokenDescription newDescription);
    }
}