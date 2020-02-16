using System;

using GZipTest.Model.Modes.Сonverting;
using GZipTest.Utilities;

namespace GZipTest.Model.Modes
{
    internal class CompressMode : BaseMode
    {
        public CompressMode(string filePathIn, string filePathOut = null) 
        {
            if (filePathIn.IsNullOrEmptyOrwhitespace())
                throw new ArgumentException(nameof(filePathIn));

            filePathOut = filePathOut.IsNullOrEmptyOrwhitespace() ? filePathIn + ".gz" : filePathOut;

            _pipeline = new PipelineCompress(filePathIn, filePathOut, Properties.Settings.Default.ReadBlockMaxSize);
        }
    }
}
