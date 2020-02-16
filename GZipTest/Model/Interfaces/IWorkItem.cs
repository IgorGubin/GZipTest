using System.Threading;

namespace GZipTest.Model.Interfaces
{
    internal interface IWorkItem
    {
        Thread GetWork();
    }
}
