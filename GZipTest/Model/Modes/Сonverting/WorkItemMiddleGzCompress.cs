using GZipTest.Utilities;

namespace GZipTest.Model.Modes.Сonverting
{
    internal class WorkItemMiddleGzCompress : WorkItemBaseMiddle
    {
        public WorkItemMiddleGzCompress() : base(b => b.GZipCompress())
        {
        }
    }
}
