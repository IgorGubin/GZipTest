using System.Collections.Concurrent;

namespace GZipTest.Model.Interfaces
{
    internal interface IBufferIn
    {
        BlockingCollection<BufferInfo> In { get; set; }
    }
}
