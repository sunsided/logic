using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Logic.LanguageParser
{
    /// <summary>
    /// Exception beim Auswerten von Token
    /// </summary>
    [Serializable]
    public class ParserException : Exception
    {
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the substring.
        /// </summary>
        /// <value>The substring.</value>
        public string Substring { get; set; }

        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="substring">The substring.</param>
        public ParserException(int index, string substring)
        {
            Index = index;
            Substring = substring;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="index">The index.</param>
        /// <param name="substring">The substring.</param>
        public ParserException(string message, int index, string substring)
            : base(message)
        {
            Index = index;
            Substring = substring;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="index">The index.</param>
        /// <param name="substring">The substring.</param>
        /// <param name="inner">The inner.</param>
        public ParserException(string message, int index, string substring, Exception inner) : base(message, inner)
        {
            Index = index;
            Substring = substring;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected ParserException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
