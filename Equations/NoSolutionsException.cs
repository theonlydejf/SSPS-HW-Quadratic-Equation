using System;
using System.Runtime.Serialization;

namespace Equations
{
    [Serializable]
    internal class NoSolutionException : Exception
    {
        private const string DefaultMessage = "The equation has no solution!";

        public NoSolutionException() : base(DefaultMessage)
        {
        }

        public NoSolutionException(string message) : base(message)
        {
        }

        public NoSolutionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoSolutionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}