using System.Collections.Generic;
using System.Linq;
using System.Text;

using GZipTest.Exceptions;
using GZipTest.Model.Interfaces;
using GZipTest.Model.Modes.Сonverting;

namespace GZipTest.Utilities
{
    internal static partial class _extentions
    {
        public static IList<IWorkItem> Next(this IList<IWorkItem> workItems, IWorkItem next)
        {
            if (workItems.Count > 0)
            {
                IWorkItem last = workItems[workItems.Count - 1];

                if (next is WorkItemBaseStart)
                {
                    throw new BrokenWorkItemsSequenceException(Properties.Resources.ErrSeveralStartWorkItems);
                }
                if (last is WorkItemBaseEnd)
                {
                    throw new BrokenWorkItemsSequenceException(Properties.Resources.ErrAttemptingInsertAnElementAfterEnding);
                }

                ((IBufferIn) next).In = ((IBufferOut)last).Out;
            }
            else if (!(next is WorkItemBaseStart))
            {
                throw new BrokenWorkItemsSequenceException(Properties.Resources.ErrlnvaidTypeOfFirstWorkItem);
            }

            workItems.Add(next);

            return workItems;
        }

        public static IList<IWorkItem> Verify(this IList<IWorkItem> workItems)
        {
            if(workItems == null || workItems.Count == 0)
                throw new WorkItemsCollectionIsNullOrEmptyException();

            var sb = new StringBuilder();
            if (!(workItems[0] is WorkItemBaseStart))
                sb.AppendLine(Properties.Resources.ErrlnvaidTypeOfFirstWorkItem);
            if (!(workItems[workItems.Count - 1] is WorkItemBaseEnd))
                sb.AppendLine(Properties.Resources.ErrlnvaidTypeOfLastWorkItem);
            if (workItems.OfType<WorkItemBaseStart>().Count() == 0)
                sb.AppendLine(Properties.Resources.ErrNoAnyWorkItemBaseStart);
            if (workItems.OfType<WorkItemBaseStart>().Count() > 1)
                sb.AppendLine(Properties.Resources.ErrSeveralStartWorkItems);
            if (workItems.OfType<WorkItemBaseEnd>().Count() == 0)
                sb.AppendLine(Properties.Resources.ErrNoAnyWorkItemBaseEnd);
            if (workItems.OfType<WorkItemBaseEnd>().Count() > 1)
                sb.AppendLine(Properties.Resources.ErrSeveralEndWorkItems);

            if(sb.Length > 0)
                throw new BrokenWorkItemsSequenceException(sb.ToString());

            return workItems;
        }
    }
}
