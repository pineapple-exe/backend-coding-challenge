using System;

namespace UsaPopulation.Domain.Interactors
{
    [Serializable]
    public class InvalidNumberException : Exception
    {
        public InvalidNumberException() { }
        public InvalidNumberException(string message) : base(message) { }
        public InvalidNumberException(string message, Exception inner) : base(message, inner) { }
        protected InvalidNumberException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
