using System.Threading.Tasks;
using P2PMulticastNetwork.Model;
using RadioPipeline;

namespace P2PMulticastNetwork
{
    public interface IPiplineProcessingInput
    {
        Task Invoke(DataModel model);
    }

    public class PiplineProcessingInput : IPiplineProcessingInput
    {
        private PipelineDelegate<DataModel> _pipline;

        public PiplineProcessingInput(PipelineDelegate<DataModel> pipeline)
        {
            _pipline = pipeline;
        }

        public async Task Invoke(DataModel model)
        {
            await _pipline.Invoke(model);
        }
    }
}