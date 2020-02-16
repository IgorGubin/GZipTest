using System;
using System.Collections.Generic;
using System.Linq;

using GZipTest.Model.Interfaces;

namespace GZipTest.Model.Modes.Сonverting
{
    internal abstract class PipelineBase
    {
        protected IList<IWorkItem> _workItems = new List<IWorkItem>();

        public void Handle()
        {
            WorkItemBase.Context.Params.Clear();
            WorkItemBase.Context.ConsoleOutputMode = Properties.Settings.Default.OutputState;
            WorkItemBase.Context.PipelineStart = DateTime.Now;

            var threads = _workItems.Select(wi => wi.GetWork()).ToArray();

            foreach (var thread in threads)
                thread.Start();

            foreach (var thread in threads)
                thread.Join();
        }
    }
}
