using GZipTest.Utilities;

namespace GZipTest.Model.Modes.Сonverting
{
    internal class PipelineCompress : PipelineBase
    {
        public PipelineCompress(string filePathIn, string filePathOut, int maxBufferSise) 
        {
            _workItems
                .Next(new WorkItemStartFileReader(filePathIn, maxBufferSise))
                .Next(new WorkItemMiddleGzCompress())
                .Next(new WorkItemEndFileWriter(filePathOut))
                .Verify();
        }
    }
}
