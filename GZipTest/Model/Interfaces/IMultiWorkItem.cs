using System.Collections.Generic;
using System.Threading;

namespace GZipTest.Model.Interfaces
{
    internal interface IMultiWorkItem
    {
        IEnumerable<Thread> GetWorks();
    }
}
