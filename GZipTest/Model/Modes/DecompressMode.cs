using System;
using System.IO;

using GZipTest.Model.Modes.Сonverting;
using GZipTest.Utilities;

namespace GZipTest.Model.Modes
{
    internal class DecompressMode : BaseMode
    {
        public DecompressMode(string filePathIn, string filePathOut = null)
        {
            if (filePathIn.IsNullOrEmptyOrwhitespace())
                throw new ArgumentException(nameof(filePathIn));


            if (filePathOut.IsNullOrEmptyOrwhitespace())
            {
                filePathOut = Path.GetFileNameWithoutExtension(filePathIn);
            }

            _pipeline = new PipelineDecompress(filePathIn, filePathOut);
        }
    }
}
