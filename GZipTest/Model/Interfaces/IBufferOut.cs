using System.Collections.Concurrent;

namespace GZipTest.Model.Interfaces
{
    internal interface IBufferOut
    {
        BlockingCollection<BufferInfo> Out { get; }
    }
}
