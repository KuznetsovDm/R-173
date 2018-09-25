using System.Threading.Tasks;
using P2PMulticastNetwork.Model;
using RadioPipeline;

namespace R_173.Handlers
{
    public interface IInvoker
    {
        Task Invoke(DataModel model, PipelineDelegate<DataModel> next);
    }
}