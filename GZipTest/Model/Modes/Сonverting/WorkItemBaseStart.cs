using System.Collections.Concurrent;

using GZipTest.Model.Interfaces;

namespace GZipTest.Model.Modes.Сonverting
{
    internal class WorkItemBaseStart : WorkItemBase, IBufferOut
    {
        public BlockingCollection<BufferInfo> Out { get; } = new BlockingCollection<BufferInfo>(Properties.Settings.Default.MaxBufferLength);
    }
}
