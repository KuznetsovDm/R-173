using System;
using System.Threading.Tasks;
using P2PMulticastNetwork.Model;
using RadioPipeline;

namespace R_173.Extensions
{
    internal interface IDataProcessingBuilder
    {
        PipelineDelegate<DataModel> Build();

        PipelineBuilder<DataModel> Use(Func<DataModel, PipelineDelegate<DataModel>, Task> action);
    }

    public class DataModelProcessingBuilder : PipelineBuilder<DataModel>, IDataProcessingBuilder { }
}