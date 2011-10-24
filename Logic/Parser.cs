using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Logic.Sequence;

namespace Logic
{
    /// <summary>
    /// Der Parser
    /// </summary>
    public sealed class Parser
    {
        /// <summary>
        /// Die Beschreibungen
        /// </summary>
        private readonly HashSet<TokenDescription> _descriptions = new HashSet<TokenDescription>();

        /// <summary>
        /// Die Anzahl der registrierten Beschreibungen
        /// </summary>
        public int Count { [Pure] get { return _descriptions.Count; } }

        /// <summary>
        /// Bezieht die Beschreibung mit dem gegebenen Tokentyp
        /// </summary>
        /// <param name="type">Der Tokentyp</param>
        /// <returns>Die Beschreibung oder <c>null</c>, wenn keine Beschreibung gefunden wurde</returns>
        public TokenDescription GetDescription(TokenType type)
        {
            return _descriptions.Where(description => description.Type == type).FirstOrDefault();
        }

        /// <summary>
        /// Fügt eine Beschreibung zum Satz hinzu
        /// </summary>
        /// <param name="description">Die hinzuzufügende Beschreibung</param>
        /// <returns>Die hinzuzufügende Beschreibung (method chaining)</returns>
        public TokenDescription AddDescription(TokenDescription description)
        {
            Contract.Requires(description != null, "Description darf nicht null sein");
            Contract.Ensures(Contract.Result<TokenDescription>() != null);

            // Vorraussetzen, dass Typ noch nicht existent
            Contract.Assume(GetDescription(description.Type) == null);

            _descriptions.Add(description);
            return description;
        }

        /// <summary>
        /// Fügt eine Beschreibung zum Satz hinzu
        /// </summary>
        /// <param name="descriptionText">Die hinzuzufügende Beschreibung</param>
        /// <param name="type">The type.</param>
        /// <returns>Die hinzuzufügende Beschreibung (method chaining)</returns>
        /// <remarks></remarks>
        public TokenDescription AddDescription(string descriptionText, TokenType type)
        {
            Contract.Requires(descriptionText != null, "Beschreibungstext darf nicht null sein");
            Contract.Ensures(Contract.Result<TokenDescription>() != null);

            // Vorraussetzen, dass Typ noch nicht existent
            Contract.Assume(GetDescription(type) == null);

            TokenDescription td = new TokenDescription(descriptionText, type);
            _descriptions.Add(td);
            return td;
        }

        /// <summary>
        /// Wertet eine Sequenz aus
        /// </summary>
        /// <param name="sequence">Die Sequenz</param>
        /// <returns>Die Liste der Token</returns>
        public IList<TokenSequenceEntry> Parse(string sequence)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(sequence), "Sequenz darf nicht leer sein");
            Contract.Ensures(Contract.Result<IList<TokenDescription>>() != null);
            List<TokenMatch> tokens = new List<TokenMatch>();

            // Vorbereitung
            IList<TokenMatch> localMatches = new List<TokenMatch>();

            // Sequenz durchlaufen, bis nichts mehr zu ermitteln ist
            int marchingIndex = 0;
            while (marchingIndex < sequence.Length)
            {
                // Alle Beschreibungen durchlaufen und Treffer ermitteln
                localMatches.Clear();
                foreach (var tokenDescription in _descriptions)
                {
                    TokenMatch localMatch;
                    if (!tokenDescription.IsMatch(sequence, marchingIndex, out localMatch)) continue;
                    localMatches.Add(localMatch);
                }

                // Sanity check
                Contract.Assume(!(localMatches.Count > 1), "Es wurde mehr als eine Tokenbeschreibung gefunden and Index " + marchingIndex);
                Contract.Assume(localMatches.Count == 1, "Es wurde keine Tokenbeschreibung gefunden an Index " + marchingIndex);

                // Token extrahieren
                TokenMatch match = localMatches[0];
                tokens.Add(match);

                // Hochzählen
                marchingIndex = match.Index + match.Length;
            }

            // Tokenmatch-Liste umwandeln in Sequenz
            return TokenMatchListToSequenceHierarchy(this, tokens);
        }

        #region Sequenzen

        /// <summary>
        /// Wandelt die Liste in eine teilhierarchische Sequenz um. Hierdurch werden Gruppierungen
        /// in Untersequenzen umgewandelt.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static IList<TokenSequenceEntry> TokenMatchListToSequenceHierarchy(Parser parser, IList<TokenMatch> list)
        {
            Contract.Requires(parser != null);
            Contract.Requires(list != null);
            Contract.Ensures(Contract.Result<IList<TokenSequenceEntry>>() != null);

            // Die aktuelle Sequenz
            IList<TokenSequenceEntry> sequence = new List<TokenSequenceEntry>();

            // Der Stack der Sequenzen für die Gruppierungen
            Stack<IList<TokenSequenceEntry>> groupStack = new Stack<IList<TokenSequenceEntry>>();

            // Alle Token durchlaufen
            for (int i = 0; i < list.Count; ++i)
            {
                TokenMatch match = list[i];

                // Gruppierungen überprüfen
                if (match.Type == TokenType.GroupStart)
                {
                    // Den aktuellen Eintrag (group start) ignorieren.
                    // Aktuelle Gruppe auf den Stack legen und neue Sequenz beginnen.
                    groupStack.Push(sequence);
                    sequence = new List<TokenSequenceEntry>();
                }
                else if (match.Type == TokenType.GroupEnd)
                {
                    // Sanity check:
                    if (groupStack.Count == 0) throw new ParenthesesMismatchException(ParenthesesMismatchException.ErrorType.TooManyClosing, "Syntaxfehler: Zu viele schließende Klammern.");

                    // Den aktuellen Eintrag (group end) ignorieren.
                    // Sequenzeintrag erzeugen, Elternsequenz vom Stack holen und Eintrag eintüten
                    TokenSequenceEntry entry = new SequenceEntry(sequence);
                    sequence = groupStack.Pop();
                    sequence.Add(entry);
                }
                else
                {
                    // Aktuellen Eintrag als TokenEntry eintüten
                    TokenEntry entry = new TokenEntry(match);
                    sequence.Add(entry);
                }
            }

            // Sanity check
            if (groupStack.Count != 0) throw new ParenthesesMismatchException(ParenthesesMismatchException.ErrorType.TooFewClosing, "Syntaxfehler: Schließende Klammer fehlt.");

            // Reverse-NOT entfernen
            ReverseNotToForwardNot(parser, ref sequence);
            return sequence;
        }

        /// <summary>
        /// Entfernt Reverse-NOT und ersetzt sie durch Forward-NOT. Auf diese Weise muss nur eine Art von NOT bei der
        /// Endverarbeitung beachtet werden.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="sequence">Die zu beackernde Sequenz</param>
        /// <remarks></remarks>
        private static void ReverseNotToForwardNot(Parser parser, ref IList<TokenSequenceEntry> sequence)
        {
            Contract.Requires(parser != null);
            Contract.Requires(sequence != null);
            Contract.Ensures(Contract.ValueAtReturn(out sequence) != null);

            // Sequenz durchlaufen. Wird eine Untersequenz gefunden, wird diese rekursiv ausgewertet.
            // Wird ein rückwärts wirkendes NOT gefunden, wird ein neues, vorwärtswirkendes
            // NOT erzeugt udn das alte Token überschrieben. Dieses Token wird vor den letzten Eintrag
            // gesetzt, das aktuelle Not wird entfernt.
            for (int s = 0; s < sequence.Count; ++s)
            {
                TokenSequenceEntry entry = sequence[s];

                // Als Untersequenz auswerten
                if (entry is SequenceEntry)
                {
                    IList<TokenSequenceEntry> subSequence = ((SequenceEntry)entry).ChildSequence;
                    ReverseNotToForwardNot(parser, ref subSequence);
                    continue;
                }

                // Als Token auswerten
                TokenEntry token = entry as TokenEntry;
                Contract.Assume(token != null);
                if (token.Match.Type == TokenType.NotReverse)
                {
                    TokenMatch currentMatch = token.Match;

                    // Neue Tokenbeschreibung erzwingen
                    TokenDescription notDescription = parser.GetDescription(TokenType.Not);
                    ITokenDescription newDescription = currentMatch.Description.GetOverride(notDescription);

                    // Neues Token generieren
                    TokenMatch newMatch = new TokenMatch(currentMatch.Index, currentMatch.Token, newDescription);

                    // Neues Token einsetzen
                    sequence.Insert(s - 1, new TokenEntry(newMatch));

                    // Aktuelles Token entfernen.
                    // Normalerweise müsste (s) entfernt werden - da jedoch durch die Insertoperation
                    // die Indizes um eines verschoben sind, gilt (s+1). Anschließend
                    // den Index von s um eines reduzieren, damit die Schleifenoperation nicht fehlschlägt.
                    sequence.RemoveAt((s--) + 1);
                }
            }
        }

        #endregion Sequenzen

        #region Code Contracts

        /// <summary>
        /// Vertragsinvariante
        /// </summary>
        [ContractInvariantMethod]
        // ReSharper disable UnusedMember.Local
        private void ContractInvariant()
        // ReSharper restore UnusedMember.Local
        {
            Contract.Invariant(_descriptions != null);
        }

        #endregion


    }
}
