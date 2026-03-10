using System;

namespace Communication.Domain.Exceptions
{
    public class InvalidCommunicationException : Exception
    {
        public InvalidCommunicationException(string message) : base(message) { }
    }
}
