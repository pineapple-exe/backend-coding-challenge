using System;

namespace UsaPopulation.Domain.Interactors
{
    [Serializable]
    public class InvalidYearException : Exception
    {
        public InvalidYearException() { }
        public InvalidYearException(string message) : base(message) { }
        public InvalidYearException(string message, Exception inner) : base(message, inner) { }
        protected InvalidYearException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
