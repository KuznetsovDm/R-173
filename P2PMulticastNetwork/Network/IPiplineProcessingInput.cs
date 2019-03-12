using System.Threading.Tasks;
using P2PMulticastNetwork.Model;

namespace P2PMulticastNetwork.Network
{
    public interface IPiplineProcessingInput
    {
        Task Invoke(DataModel model);
    }

    public class PiplineProcessingInput : IPiplineProcessingInput
    {
        private readonly PipelineDelegate<DataModel> _pipline;

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