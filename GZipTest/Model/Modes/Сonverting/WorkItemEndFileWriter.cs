using System.Diagnostics;
using System.IO;

using GZipTest.Utilities;

namespace GZipTest.Model.Modes.Сonverting
{
    internal class WorkItemEndFileWriter : WorkItemBaseEnd
    {
        private string _filePathOut;
        private object _objInLock = new object();

        public WorkItemEndFileWriter(string filePathOut)
        {
            _filePathOut = filePathOut;
        }

        #region IWorkItem implementation of base class
        protected override void Work()
        {
            if (Properties.Settings.Default.RewriteExistsFile)
            {
                using (var _ = new FileStream(_filePathOut, FileMode.Truncate, FileAccess.Write)) {/* truncate exists file*/}
            }
            else
            {
                _filePathOut = _filePathOut.GetFreeFilePath();
            }

            double fileInLength = Context.FileInLength;

            var sw = new Stopwatch();
            double offset = 0;

            using (var fs = File.Open(_filePathOut, FileMode.OpenOrCreate, FileAccess.Write))
            {
                do
                {
                    if (GetInNext(out BufferInfo bufferInfo))
                    {
                        sw.Start();
                        fs.Write(bufferInfo.Buffer, 0, bufferInfo.Buffer.Length);
                        sw.Stop();

                        Debug.WriteLine($"w: {bufferInfo.OrigOffset} [{bufferInfo.OrigLength}]");

                        offset += bufferInfo.OrigLength;
                    }

                    var progress = offset / fileInLength;
                    if (Context.ConsoleOutputMode == OutputStateEnum.Debug)
                    {
                        var q1CurCount = Context.Params.TryGetValue("Q1.CurCount", out var tmpQ1CurCount) ? tmpQ1CurCount : 0;
                        var q1MaxCount = Context.Params.TryGetValue("Q1.MaxCount", out var tmpQ1MaxCount) ? tmpQ1MaxCount : 0;
                        var q1MaxDelta = Context.Params.TryGetValue("Q1.MaxDelta", out var tmpQ1MaxDelta) ? tmpQ1MaxDelta : 0;
                        var q2CurCount = Context.Params.TryGetValue("Q2.CurCount", out var tmpQ2CurCount) ? tmpQ2CurCount : 0;
                        var q2MaxCount = Context.Params.TryGetValue("Q2.MaxCount", out var tmpQ2MaxCount) ? tmpQ2MaxCount : 0;
                        var q2MaxDelta = Context.Params.TryGetValue("Q2.MaxDelta", out var tmpQ2MaxDelta) ? tmpQ2MaxDelta : 0;

                        _utility.OutProgress(100 * progress, $"Position: {fs.Position}; Q1[{q1CurCount}:{q1MaxCount}]:{q1MaxDelta}; Q2[{q2CurCount}:{q2MaxCount}]:{q2MaxDelta}; write:{sw.ElapsedMilliseconds}");
                    }
                    else
                    {
                        var expectedTotalTime = Context.ExpectedTime(progress);
                        _utility.OutProgress(100 * progress, $" Consumed time: {Context.ConsumedTime.ToFormatString()}; Expected left time: {(expectedTotalTime - Context.ConsumedTime).ToFormatString()}; Expected total time: {expectedTotalTime.ToFormatString()}");
                    }
                    

                } while (!In.IsCompleted);
            }
        }

        private bool GetInNext(out BufferInfo bufferInfo)
        {
            var res = false;
            bufferInfo = null;

            if (In.Count > 0)
            {
                lock (_objInLock)
                {
                    if (In.Count > 0)
                    {
                        var tmpBufferInfo = In.Take();
                        if (tmpBufferInfo != null)
                        {
                            bufferInfo = tmpBufferInfo;
                            res = true;
                        }
                    }
                }
            }
            return res;
        }
        #endregion IWorkItem implementation of base class
    }
}
