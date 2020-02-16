using System;
using System.Runtime.Serialization;

namespace GZipTest.Exceptions
{
    internal class BrokenWorkItemsSequenceException : Exception
    {
        public BrokenWorkItemsSequenceException() : base() { }
        public BrokenWorkItemsSequenceException(string message) : base(message) {}
        public BrokenWorkItemsSequenceException(string message, Exception innerException) : base(message, innerException) { }
        public BrokenWorkItemsSequenceException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
