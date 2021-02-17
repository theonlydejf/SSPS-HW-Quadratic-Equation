using System;
using System.Runtime.Serialization;

namespace Equations
{
    [Serializable]
    internal class InfinitlyManySolutionsException : Exception
    {
        private const string DefaultMessage = "The equation has infinitly many solutions!";

        public InfinitlyManySolutionsException() : base(DefaultMessage)
        {
        }

        public InfinitlyManySolutionsException(string message) : base(message)
        {
        }

        public InfinitlyManySolutionsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InfinitlyManySolutionsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}