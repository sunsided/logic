using System;
using System.Diagnostics.Contracts;

namespace Logic.LanguageParser
{
    /// <summary>
    /// Ein Token
    /// </summary>
    public sealed class Token
    {
        /// <summary>
        /// Der Ausdruck
        /// </summary>
        public string Value { [Pure] get; private set; }

        /// <summary>
        /// Das zugehörige Beschreibung
        /// </summary>
        public TokenDescription OriginDescription { [Pure] get; private set; }

        /// <summary>
        /// Der Index in der Eingangsgleichung
        /// </summary>
        public int Index { [Pure] get; private set; }

        /// <summary>
        /// Die Länge des Tokens
        /// </summary>
        public int Length { [Pure] get { return Value.Length; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="value">The sub expression.</param>
        /// <param name="originDescription">The origin description.</param>
        /// <param name="index">The index.</param>
        public Token(string value, TokenDescription originDescription, int index)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(value), "Unterausdruck darf nicht null sein");
            Contract.Requires(originDescription != null, "Stammausdruck darf nicht null sein");
            Contract.Requires(index >= 0, "Index darf nicht kleiner als 0 sein");

            OriginDescription = originDescription;
            Value = value;
            Index = index;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Value + " --> " + OriginDescription.Description;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ OriginDescription.GetHashCode();
        }
    }
}
