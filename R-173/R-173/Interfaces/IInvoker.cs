using System.Threading.Tasks;
using P2PMulticastNetwork.Model;
using RadioPipeline;

namespace R_173.Interfaces
{
    public interface IInvoker
    {
        Task Invoke(DataModel model, PipelineDelegate<DataModel> next);
    }
}