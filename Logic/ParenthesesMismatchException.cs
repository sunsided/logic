using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Logic
{
    /// <summary>
    /// Exception, wenn die Anzahl der Klammern in der Eingabe ungültig ist.
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class ParenthesesMismatchException : Exception
    {
        /// <summary>
        /// Fehlertyp
        /// </summary>
        public enum ErrorType
        {
            /// <summary>
            /// Schließende Klammer fehlt
            /// </summary>
            TooFewClosing,

            /// <summary>
            /// Öffnende Klammer fehlt
            /// </summary>
            TooManyClosing
        }

        /// <summary>
        /// Fehlertyp
        /// </summary>
        public ErrorType Error { [Pure] get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <remarks></remarks>
        public ParenthesesMismatchException(ErrorType error)
        {
            Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParenthesesMismatchException"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public ParenthesesMismatchException(ErrorType error, string message) : base(message)
        {
            Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParenthesesMismatchException"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <remarks></remarks>
        public ParenthesesMismatchException(ErrorType error, string message, Exception inner) : base(message, inner)
        {
            Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        ///   
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        /// <remarks></remarks>
        protected ParenthesesMismatchException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
