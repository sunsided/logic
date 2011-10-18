using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Logic.LanguageParser;
using System.Linq;
using Logic.LanguageParser.Descriptions;

namespace Logic.TokenInterpreter
{
    /// <summary>
    /// Interpretiert eine Sequenz von Token und erzeugt einen Ausdrucksbaum
    /// </summary>
    public sealed class Interpreter
    {
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

        public bool SplitSequenceOn(IList<Token> tokenSequence, Func<Token, bool> predicate, out IList<Token> left, out IList<Token> right)
        {
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
