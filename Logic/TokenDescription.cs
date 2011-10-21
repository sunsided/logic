using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

namespace Logic
{
    /// <summary>
    /// Die Beschreibung eines Tokens
    /// </summary>
    public sealed class TokenDescription
    {
        /// <summary>
        /// Die Liste der Schlüsselworte
        /// </summary>
        private readonly HashSet<string> _keywords = new HashSet<string>();

        /// <summary>
        /// Die Anzahl der Schlüsselworte
        /// </summary>
        public int KeywordCount { [Pure] get { return _keywords.Count; } }

        /// <summary>
        /// Die Liste der Schlüsselworte
        /// </summary>
        private readonly HashSet<string> _reservedWords = new HashSet<string>();

        /// <summary>
        /// Die Anzahl der reservierten Worte
        /// </summary>
        public int ReservedWordCount { [Pure] get { return _reservedWords.Count; } }

        /// <summary>
        /// Gibt an, ob der Wortesatz geändert wurde und damit die Regex neu erzeugt werden muss
        /// </summary>
        private bool _wordSetChanged;

        /// <summary>
        /// Die Regex
        /// </summary>
        private Regex _cachedRegex;

        /// <summary>
        /// Fügt ein Schlüsselwort hinzu
        /// </summary>
        /// <param name="keyword">Das Schlüsselwort</param>
        /// <param name="additionalKeywords">Zusätzliche Schlüsselworte</param>
        public void AddKeyword(string keyword, params string[] additionalKeywords)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(keyword), "Keyword darf nicht leer sein");
            Contract.Requires(additionalKeywords != null, "Zusätzliche Keywords dürfen nicht null sein");
            Contract.Requires(Contract.ForAll(additionalKeywords, w => !String.IsNullOrWhiteSpace(w)), "Zusätzliche Keywords dürfen nicht leer sein");

            // Schlüsselworte hinzufügen
            _wordSetChanged |= _keywords.Add(keyword);
            for (int a=additionalKeywords.Length-1; a>=0; --a)
            {
                _wordSetChanged |= _keywords.Add(additionalKeywords[a]);
            }
        }

        /// <summary>
        /// Ermittelt, ob diese Tokenbeschreibung auf die angegebene Sequenz passt
        /// </summary>
        /// <param name="sequence">Die Eingabesequenz</param>
        /// <returns><c>true</c> if the specified sequence is match; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public bool IsMatch(string sequence)
        {
            Contract.Requires(sequence != null);

            string ignored;
            return IsMatch(sequence, out ignored);
        }

        /// <summary>
        /// Ermittelt, ob diese Tokenbeschreibung auf die angegebene Sequenz passt
        /// </summary>
        /// <param name="sequence">Die Eingabesequenz</param>
        /// <param name="matchedKeyword">Das gefundene Keyword im Erfolgsfall, ansonsten <c>null</c></param>
        /// <returns></returns>
        public bool IsMatch(string sequence, out string matchedKeyword)
        {
            Contract.Requires(sequence != null);
            Contract.Ensures((Contract.ValueAtReturn(out matchedKeyword) == null && !Contract.Result<bool>()) || (Contract.ValueAtReturn(out matchedKeyword) != null && Contract.Result<bool>()));
            Contract.Assume(KeywordCount > 0, "Keine Schlüsselworte definiert");

            // Regex auswerten
            Regex regex = GetRegexFromWords();
            Match match = regex.Match(sequence);

            // Im Erfolgsfall das getroffene Wort ausgeben
            if (match.Success)
            {
                matchedKeyword = match.Groups["token"].Value;
                return true;
            }

            // Fehler.
            matchedKeyword = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public bool MatchAndTrim(ref string sequence)
        {
            Contract.Requires(sequence != null);
            string keyword;
            if (IsMatch(sequence, out keyword))
            {
                sequence = sequence.Substring(keyword.Length);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Fügt ein reserviertes Wort hinzu
        /// </summary>
        /// <param name="word">Das reservierte Wort</param>
        /// <param name="additionalWords">Zusätzliche reservierte Worte</param>
        public void AddReservedWord(string word, params string[] additionalWords)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(word), "Keyword darf nicht leer sein");
            Contract.Requires(additionalWords != null, "Zusätzliche Keywords dürfen nicht null sein");
            Contract.Requires(Contract.ForAll(additionalWords, w => !String.IsNullOrWhiteSpace(w)), "Zusätzliche Keywords dürfen nicht leer sein");

            // Reservierte hinzufügen
            _wordSetChanged |= _reservedWords.Add(word);
            for (int a = additionalWords.Length - 1; a >= 0; --a)
            {
                _wordSetChanged |= _reservedWords.Add(additionalWords[a]);
            }
        }

        /// <summary>
        /// Liefert die Regex oder erzeugt eine neue, falls der Wortschatz geändert wurde
        /// </summary>
        private Regex GetRegexFromWords()
        {
            Contract.Ensures(Contract.Result<Regex>() != null);

            // Gecachte Regex herausgeben
            if (!_wordSetChanged && _cachedRegex != null) return _cachedRegex;

            // Regex erzeugen
            _wordSetChanged = false;
            return _cachedRegex = GenerateRegularExpression(_keywords, _reservedWords);
            
        }

        /// <summary>
        /// Erzeugt die Regular Expression
        /// </summary>
        /// <param name="keywords">Die Schlüsselworte</param>
        /// <param name="reservedWords">Die reservierten Worte</param>
        /// <returns>Die Regular Expression</returns>
        private static Regex GenerateRegularExpression(ICollection<string> keywords, ICollection<string> reservedWords)
        {
            Contract.Requires(keywords != null, "Keywords dürfen nicht null sein");
            Contract.Requires(Contract.ForAll(keywords, w => !String.IsNullOrWhiteSpace(w)), "Keywords dürfen nicht leer sein");
            Contract.Requires(reservedWords != null, "Reservierte Worte dürfen nicht null sein");
            Contract.Requires(Contract.ForAll(reservedWords, w => !String.IsNullOrWhiteSpace(w)), "Reservierte Worte dürfen nicht leer sein");

            // Sanity check
            Contract.Assume(keywords.Any());

            // Regex vorbereiten
            string regularExpression;
            var escapedKeywords = keywords.Select(Regex.Escape);

            // Testen, ob reservierte Worte vorliegen
            if (reservedWords.Any())
            {
                // Expression mit reservierten Worten
                var escapedReservedWords = reservedWords.Select(Regex.Escape);
                regularExpression = "^(!?(" + String.Join("|", escapedReservedWords) + "))(?<token>" + String.Join("|", escapedKeywords) + ")";
            }
            else
            {
                // Expression ohne reservierte Worte
                regularExpression = "^(?<token>" + String.Join("|", escapedKeywords) + ")";
            }

            // Regex erzeugen
            return new Regex(regularExpression, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
        }
        
        #region Code Contracts

        /// <summary>
        /// Vertragsinvariante
        /// </summary>
        [ContractInvariantMethod]
        // ReSharper disable UnusedMember.Local
        private void ContractInvariant()
        // ReSharper restore UnusedMember.Local
        {
            Contract.Invariant(_keywords != null);
            Contract.Invariant(_reservedWords != null);
        }

        #endregion
    }
}
