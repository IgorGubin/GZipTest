using System.Collections.Concurrent;

using GZipTest.Model.Interfaces;

namespace GZipTest.Model.Modes.Сonverting
{
    internal class WorkItemBaseEnd : WorkItemBase, IBufferIn
    {
        public BlockingCollection<BufferInfo> In { get; set; }
    }
}
