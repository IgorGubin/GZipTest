using GZipTest.Utilities;

namespace GZipTest.Model.Modes.Сonverting
{
    internal class PipelineDecompress : PipelineBase
    {
        public PipelineDecompress(string filePathIn, string filePathOut)
        {
            _workItems
                .Next(new WorkItemStartGzFileReader(filePathIn))
                .Next(new WorkItemMiddleGzDecompress())
                .Next(new WorkItemEndFileWriter(filePathOut))
                .Verify();
        }
    }
}
