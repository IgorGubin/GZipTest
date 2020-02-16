using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using GZipTest.Model.Interfaces;
using GZipTest.Utilities;

namespace GZipTest.Model.Modes.Сonverting
{
    internal abstract class WorkItemBaseMiddle : WorkItemBase, IBufferIn, IBufferOut
    {
        private Func<byte[], byte[]> _convertor;

        protected WorkItemBaseMiddle(Func<byte[], byte[]> convertor)
        {
            _convertor = convertor;
        }

        public BlockingCollection<BufferInfo> In { get; set; }

        public BlockingCollection<BufferInfo> Out { get; } = new BlockingCollection<BufferInfo>(Properties.Settings.Default.MaxBufferLength);

        #region IWorkItem implementation of base class
        protected override void Work()
        {
            var count = Context.FileInLength < Properties.Settings.Default.ReadBlockMaxSize
                ? (ushort) 1
                : (ushort) Math.Min(Environment.ProcessorCount, Math.Floor(Context.FileInLength / Properties.Settings.Default.ReadBlockMaxSize));

            var localContext = new ConcurrentDictionary<string, dynamic>();

            var mh = new MultiHandler(_convertor, count, localContext) { In = In };
            var tv = new Transceiver(() => !mh.IsCompleted, localContext) { In = mh.Out, Out = Out };

            var threads = new List<Thread>(mh.GetWorks()) { tv.GetWork() };

            foreach (var th in threads)
                th.Start();
            foreach (var th in threads)
                th.Join();
        }

        private sealed class MultiHandler : IMultiWorkItem
        {
            private ushort _maxHandlersCount;
            private Func<byte[], byte[]> _convert;
            private ConcurrentDictionary<string, dynamic> _localContext;
            private static object _objInLock = new object();

            public MultiHandler(Func<byte[], byte[]> convert, ushort maxHandlersCount, ConcurrentDictionary<string, dynamic> localContext)
            {
                _convert = convert;
                _maxHandlersCount = maxHandlersCount;
                _localContext = localContext;
            }

            public bool IsCompleted { get; private set; } = false;

            public BlockingCollection<BufferInfo> In { get; set; }

            public ConcurrentDictionary<decimal, BufferInfo> Out { get; } = new ConcurrentDictionary<decimal, BufferInfo>();

            #region IMultiWorkItem implementation

            public IEnumerable<Thread> GetWorks()
            {
                uint isBusy = 0;
                var res = new Thread[_maxHandlersCount];
                for (int i = 0; i < _maxHandlersCount; i++)
                {
                    var index = i;
                    res[i] = new Thread(
                        _utility.ExitIfError(() => {
                            while (!IsCompleted)
                            {
                                if (GetInNext(out var bufferInfo))
                                {
                                    isBusy |= (1U << index);
                                    try
                                    {
                                        var gzBuffer = _convert(bufferInfo.Buffer);
                                        bufferInfo.Buffer = gzBuffer;

                                        Out.TryAdd(bufferInfo.OrigOffset, bufferInfo);
                                    }
                                    finally
                                    {
                                        isBusy ^= (1U << index);
                                    }
                                }

                                if (In.IsCompleted)
                                    IsCompleted = isBusy == 0;
                            }
                        }));
                }
                return res;
            }

            private bool GetInNext(out BufferInfo bufferInfo)
            {
                bool res = false;
                bufferInfo = null;

                if (In.Count > 0)
                {
                    lock (_objInLock)
                    {
                        if (In.Count > 0)
                        {
                            var start = DateTime.Now;
                            var tmpBufferInfo = In.Take();
                            if (tmpBufferInfo != null)
                            {
                                bufferInfo = tmpBufferInfo;
                                _localContext[$"{bufferInfo.OrigOffset}"] = start;

                                Debug.WriteLine($"h(+): {bufferInfo.OrigOffset} [{bufferInfo.OrigLength}]");

                                res = true;
                            }
                        }
                    }
                }

                return res;
            }

            #endregion IMultiWorkItem implementation
        }

        private sealed class Transceiver : IWorkItem
        {
            private decimal _lastOffset = 0;
            private Func<bool> _doing;
            private ConcurrentDictionary<string, dynamic> _localContext;

            public Transceiver(Func<bool> doing, ConcurrentDictionary<string, dynamic> localContext)
            {
                _doing = doing;
                _localContext = localContext;
            }

            public ConcurrentDictionary<decimal, BufferInfo> In { get; set; }

            public BlockingCollection<BufferInfo> Out { get; set; }

            #region IWorkItem implementation
            public Thread GetWork()
            {
                var res = new Thread(
                    _utility.ExitIfError(() =>
                    {
                        while (_doing() || In.Count > 0)
                        {
                            if (GetInNext(out var bufferInfo))
                            {
                                if (_localContext.TryRemove(bufferInfo.OrigOffset.ToString(), out dynamic start))
                                {
                                    Context.SetMax("Q2.MaxDelta", (int)(DateTime.Now - (DateTime)start).TotalMilliseconds);
                                }

                                Out.Add(bufferInfo);

                                Context.Params["Q2.CurCount"] = Out.Count;
                                Context.SetMax("Q2.MaxCount", Out.Count);
                            }
                        } 
                        Out.CompleteAdding();
                    }));
                return res;
            }

            private bool GetInNext(out BufferInfo bufferInfo)
            {
                var res = false;
                bufferInfo = null;

                if (In.TryRemove(_lastOffset, out var tmpBufferInfo))
                {
                    bufferInfo = tmpBufferInfo;

                    _lastOffset += bufferInfo.OrigLength;
                    res = true;
                }

                return res;
            }
            #endregion IWorkItem implementation
        }

        #endregion IWorkItem implementation of base class
    }
}
