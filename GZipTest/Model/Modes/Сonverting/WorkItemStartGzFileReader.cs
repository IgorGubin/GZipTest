using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GZipTest.Model.Modes.Сonverting
{
    internal class WorkItemStartGzFileReader : WorkItemBaseStart
    {
        private string _filePathIn;

        public WorkItemStartGzFileReader(string filePathIn)
        {
            _filePathIn = filePathIn;
        }

        #region IWorkItem implementation of base class
        protected override void Work()
        {
            Context.FileInLength = new FileInfo(_filePathIn).Length;

            using (var gzReader = new GzReader(_filePathIn).Open())
            {
                var sw = new Stopwatch();
                foreach (var bufferInfo in gzReader.Next())
                {
                    sw.Start();
                    Out.Add(bufferInfo);
                    sw.Stop();

                    Debug.WriteLine($"r: {bufferInfo.OrigOffset} [{bufferInfo.OrigLength}]");

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

        
        // https://www.ietf.org/rfc/rfc1952.txt
        // https://www.ietf.org/rfc/rfc1951.txt
        private class GzReader : IDisposable
        {
            /// <summary>
            /// GZip file header.
            /// </summary>
            public static byte[] Header = new byte[] { 31, 139, 8, 0, 0, 0, 0, 0, 4, 0 };  //  +---+---+---+---+---+---+---+---+---+---+
                                                                                           //  |ID1|ID2|CM |FLG|     MTIME     |XFL| OS|
                                                                                           //  +---+---+---+---+---+---+---+---+---+---+
                                                                                           //  | 31|139| 8 | 0 | 0 | 0 | 0 | 0 | 4 | 0 | 
                                                                                           //   CM = 8  - gzip;
                                                                                           //   FLG = 0 - FTEXT
                                                                                           //   MTIME = 0
                                                                                           //   XFL = 4 - compressor used fastest algorithm
                                                                                           //   OS = 0 - FAT filesystem (MS-DOS, OS/2, NT/Win32)
            private string _filePath;
            private FileStream _fileStream;

            public GzReader(string filePath)
            {
                _filePath = filePath;
            }

            public void Dispose()
            {
                _fileStream.Dispose();
            }

            public GzReader Open()
            {
                _fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return this;
            }

            public IEnumerable<BufferInfo> Next()
            {
                List<byte> curList = null;

                int headerPosition = 0;
                decimal filePosition = 0;
                decimal fileOffsetsOfGzBuffer;
                uint readed;

                ulong bufferSize = (ulong) Math.Min(_fileStream.Length, Properties.Settings.Default.ReadBlockMaxSize);

                byte[] buffer = new byte[bufferSize];

                if (bufferSize == 0)
                    yield return new BufferInfo(0, buffer);

                int readCount;
                while ((readCount = _fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < readCount; i++, filePosition++)
                    {
                        byte curByte = buffer[i];

                        if (curList != null)
                        {
                            curList.Add(curByte);
                        }

                        if (curByte == Header[headerPosition])
                        {
                            headerPosition++;
                            if (headerPosition == Header.Length)
                            {
                                if (curList != null)
                                {
                                    fileOffsetsOfGzBuffer = filePosition + 1 - curList.Count;
                                    readed = (uint)(curList.Count - Header.Length);
                                    var buf = new byte[readed];
                                    for (int j = 0; j < readed; j++)
                                    {
                                        buf[j] = curList[j];
                                    }

                                    yield return new BufferInfo(fileOffsetsOfGzBuffer, buf);
                                }

                                curList = new List<byte>(Header);
                                headerPosition = 0;
                            }
                        }
                        else if (headerPosition > 0)
                        {
                            headerPosition = 0;
                        }
                    }
                }

                if (curList != null)
                {
                    fileOffsetsOfGzBuffer = filePosition - curList.Count;
                    var buf = curList.ToArray();
                    yield return new BufferInfo(fileOffsetsOfGzBuffer, buf);
                }
                else
                    yield break;
            }
        }

        #endregion IWorkItem implementation of base class
    }
}
