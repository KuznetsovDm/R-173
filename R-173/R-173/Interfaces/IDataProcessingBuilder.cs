using System;
using System.Threading.Tasks;
using P2PMulticastNetwork.Model;
using RadioPipeline;
using static R_173.App;

namespace R_173.Interfaces
{
    public interface IDataProcessingBuilder
    {
        PipelineDelegate<DataModel> Build();

        IDataProcessingBuilder Use(Func<DataModel, PipelineDelegate<DataModel>, Task> action);

        IDataProcessingBuilder UseMiddleware<T>() where T : IInvoker;
    }
}