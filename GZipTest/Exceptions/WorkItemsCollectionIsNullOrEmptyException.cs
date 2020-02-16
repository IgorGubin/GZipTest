using System;
using System.Runtime.Serialization;

namespace GZipTest.Exceptions
{
    internal class WorkItemsCollectionIsNullOrEmptyException : Exception
    {
        public WorkItemsCollectionIsNullOrEmptyException() : base() { }
        public WorkItemsCollectionIsNullOrEmptyException(string message) : base(message) {}
        public WorkItemsCollectionIsNullOrEmptyException(string message, Exception innerException) : base(message, innerException) { }
        public WorkItemsCollectionIsNullOrEmptyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
