using System.Threading;

using NLog;

using GZipTest.Model.Interfaces;
using GZipTest.Utilities;

namespace GZipTest.Model.Modes.Сonverting
{
    internal abstract class WorkItemBase : IWorkItem
    {
        protected static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static WorkItemContext Context = new WorkItemContext();

        #region IWorkItem implementation
        public Thread GetWork()
        {
            var t = new Thread(
                _utility.ExitIfError(
                    () => {
                        Work();
                    }));

            return t;
        }

        protected virtual void Work()
        {
        }
        #endregion IWorkItem implementation
    }
}
