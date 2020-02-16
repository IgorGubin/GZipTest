using GZipTest.Model.Interfaces;
using GZipTest.Model.Modes.Сonverting;

namespace GZipTest.Model.Modes
{
    internal abstract class BaseMode : IMode
    {
        protected PipelineBase _pipeline;

        public void Do()
        {
            _pipeline.Handle();
        }
    }
}
