using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Logic
{
    /// <summary>
    /// Die Beschreibung eines Tokens
    /// </summary>
    [DebuggerDisplay("Override {OriginalDescription.Description} -> {NewDescription.Description}")]
    internal sealed class TokenDescriptionOverride : ITokenDescription
    {
        /// <summary>
        /// Gets the original description.
        /// </summary>
        /// <remarks></remarks>
        public ITokenDescription OriginalDescription { [Pure] get; private set; }

        /// <summary>
        /// Gets the new description.
        /// </summary>
        /// <remarks></remarks>
        public ITokenDescription NewDescription { [Pure] get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenDescriptionOverride"/> class.
        /// </summary>
        /// <param name="oldTokenDescription">The old token description.</param>
        /// <param name="newTokenDescription">The new token description.</param>
        /// <remarks></remarks>
        public TokenDescriptionOverride(ITokenDescription oldTokenDescription, ITokenDescription newTokenDescription)
        {
            Contract.Requires(oldTokenDescription != null);
            Contract.Requires(newTokenDescription != null);

            OriginalDescription = oldTokenDescription;
            NewDescription = newTokenDescription;
        }

        /// <summary>
        /// Beschreibungstext
        /// </summary>
        string ITokenDescription.Description
        {
            get {return NewDescription.Description;}
        }

        /// <summary>
        /// Typ des Tokens
        /// </summary>
        TokenType ITokenDescription.Type
        {
            get { return NewDescription.Type; }
        }

        /// <summary>
        /// Bezieht einen Override
        /// </summary>
        /// <param name="newDescription">Die neue Beschreibung</param>
        /// <returns>Der Override</returns>
        public ITokenDescription GetOverride(TokenDescription newDescription)
        {
            return new TokenDescriptionOverride(this, newDescription);
        }
    }
}
