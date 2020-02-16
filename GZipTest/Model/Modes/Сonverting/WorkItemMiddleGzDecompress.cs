using GZipTest.Utilities;

namespace GZipTest.Model.Modes.Сonverting
{
    internal class WorkItemMiddleGzDecompress : WorkItemBaseMiddle
    {
        public WorkItemMiddleGzDecompress() : base(b => b.GZipDecompress())
        {
        }
    }
}
