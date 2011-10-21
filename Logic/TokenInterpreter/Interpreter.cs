using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Logic.LanguageParser;
using System.Linq;
using Logic.LanguageParser.Descriptions;
using Logic.TokenInterpreter.TokenTypes;

namespace Logic.TokenInterpreter
{
    /// <summary>
    /// Interpretiert eine Sequenz von Token und erzeugt einen Ausdrucksbaum
    /// </summary>
    public sealed class Interpreter
    {
        /// <summary>
        /// Die Token-Map
        /// </summary>
        private readonly TokenTypeMap TokenMap = new TokenTypeMap();

        private void InitializeTokenMap()
        {
            TokenMap.Add(new TokenTypeMapEntry(typeof(AndOperator), "*", "&", "and"));
            TokenMap.Add(new TokenTypeMapEntry(typeof(OrOperator), "+", "|", "^", "or"));
            TokenMap.Add(new TokenTypeMapEntry(typeof(NotOperator), "~", "!", "'"));
            
            // TODO: Schmarrn - wann Term, wann Gruppe?
            // TODO: Aufbau der Parser-Regex aus diesen Defitionen?
        }

        /// <summary>
        /// Interprets the specified token sequence.
        /// </summary>
        /// <param name="tokenSequence">The token sequence.</param>
        public void Interpret(IList<Token> tokenSequence)
        {
            Contract.Requires(tokenSequence != null, "Token sequence must not be null");

            // Klammern finden
            IList<Token> left, right;
            SplitSequenceOn(tokenSequence, token => token.OriginDescription is GroupOpenToken, out left, out right);
        }

        /// <summary>
        /// Trennt die Sequenz bei in zwei Teilsequenzen auf
        /// </summary>
        /// <param name="tokenSequence">Die Ursprungssequenz</param>
        /// <param name="predicate">Die Bedingung. Das Element, auf welches diese Bedingung zutrifft, wird aus beiden Teilsequenzen entfernt</param>
        /// <param name="left">Die Elemente links des Pivotelementes</param>
        /// <param name="right">Die Elemente rechts des Pivotelementes.</param>
        /// <returns><c>true</c> im Erfolgsfall, ansonsten <c>false</c></returns>
        private static bool SplitSequenceOn(IList<Token> tokenSequence, Func<Token, bool> predicate, out IList<Token> left, out IList<Token> right)
        {
            Contract.Requires(tokenSequence != null, "Token sequence must not be null");
            Contract.Requires(predicate != null, "Predicate function must not be null");
            Contract.Ensures(Contract.ValueAtReturn(out left) != null, "Left sequence must not be null");
            Contract.Ensures(Contract.ValueAtReturn(out right) != null, "Right sequence must not be null");

            // Ausgaben vorbereiten
            left = new List<Token>(0);
            right = new List<Token>(0);

            // Splitten
            for (int t = 0; t < tokenSequence.Count; ++t)
            {
                if (predicate(tokenSequence[t]))
                {
                    // Linke liste Erzeugen
                    left = new List<Token>();
                    for(int tl = 0; tl < t; ++tl)
                    {
                        left.Add(tokenSequence[tl]);
                    }

                    // Rechte Liste erzeugen
                    right = new List<Token>();
                    for (int tr = t+1; tr < tokenSequence.Count; ++tr)
                    {
                        right.Add(tokenSequence[tr]);
                    }

                    // Ja lecker!
                    return true;
                }
            }

            // Nope.
            return false;
        }
    }
}
