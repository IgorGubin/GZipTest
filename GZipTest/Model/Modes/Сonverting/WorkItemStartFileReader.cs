using System;
using System.Diagnostics;
using System.IO;

namespace GZipTest.Model.Modes.Сonverting
{
    internal class WorkItemStartFileReader: WorkItemBaseStart
    {
        private string _filePathIn;
        private int _maxBufferSise;

        public WorkItemStartFileReader(string filePathIn, int maxBufferSise)
        {
            _filePathIn = filePathIn;
            _maxBufferSise = maxBufferSise;
        }

        #region IWorkItem implementation of base class
        protected override void Work()
        {
            var sw = new Stopwatch();

            Context.FileInLength = new FileInfo(_filePathIn).Length;

            decimal offset = 0;
            var buffer = new byte[_maxBufferSise];
            using (var fs = File.Open(_filePathIn, FileMode.Open, FileAccess.Read))
            {
                long readed;
                while ((readed = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    sw.Start();

                    byte[] block = new byte[readed];
                    Array.Copy(buffer, 0, block, 0, readed);

                    var bufferInfo = new BufferInfo(offset, block);
                    Out.Add(bufferInfo);

                    Debug.WriteLine($"r: {bufferInfo.OrigOffset} [{bufferInfo.OrigLength}]");

                    offset += readed;

                    sw.Stop();

                    if (Context.ConsoleOutputMode == OutputStateEnum.Debug)
                    {
                        Context.Params["Q1.CurCount"] = Out.Count;
                        Context.SetMax("Q1.MaxCount", Out.Count);
                        Context.SetMax("Q1.MaxDelta", sw.ElapsedMilliseconds);
                    }
                }
            }
            Out.CompleteAdding();
        }
        #endregion IWorkItem implementation of base class
    }
}
