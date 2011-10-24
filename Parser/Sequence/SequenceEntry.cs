using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Logic.Sequence
{
    /// <summary>
    /// Ein Sequenzeintrag
    /// </summary>
    [DebuggerDisplay("sequence of length {Count}")]
    public sealed class SequenceEntry : TokenSequenceEntry
    {
        /// <summary>
        /// Die Kindsequenz
        /// </summary>
        public IList<TokenSequenceEntry> ChildSequence { [Pure] get; private set; }

        /// <summary>
        /// Die Anzahl der Kindelemente
        /// </summary>
        public int Count { [Pure] get { return ChildSequence.Count; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceEntry"/> class.
        /// </summary>
        /// <param name="childSequence">The child sequence.</param>
        /// <remarks></remarks>
        public SequenceEntry(IList<TokenSequenceEntry> childSequence)
        {
            Contract.Requires(childSequence != null, "Kindsequenz darf nicht null sein");
            ChildSequence = childSequence;
        }

        /// <summary>
        /// Gets the <see cref="Logic.Sequence.TokenSequenceEntry"/> with the specified i.
        /// </summary>
        /// <remarks></remarks>
        public TokenSequenceEntry this[int i]
        {
            [Pure] get
            {
                Contract.Requires(i >= 0, "Index muss größer als 0 sein");
                Contract.Requires(i < Count, "Index muss größer als 0 sein");
                return ChildSequence[i];
            }
        }
    }
}
