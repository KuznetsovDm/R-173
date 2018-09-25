using System;
using System.Threading.Tasks;
using P2PMulticastNetwork.Model;
using RadioPipeline;
using Unity;
using static R_173.App;

namespace R_173.Handlers
{
    public interface IDataProcessingBuilder
    {
        PipelineDelegate<DataModel> Build();

        IDataProcessingBuilder Use(Func<DataModel, PipelineDelegate<DataModel>, Task> action);

        IDataProcessingBuilder UseMiddleware<T>() where T : IInvoker;
    }

    public class DataModelProcessingBuilder : IDataProcessingBuilder
    {
        private PipelineBuilder<DataModel> _builder;
        private IUnityContainer _container;

        public DataModelProcessingBuilder(IUnityContainer container, PipelineBuilder<DataModel> builder)
        {
            _builder = builder;
            _container = container;
        }

        public PipelineDelegate<DataModel> Build()
        {
            return _builder.Build();
        }

        public IDataProcessingBuilder Use(Func<DataModel, PipelineDelegate<DataModel>, Task> action)
        {
            _builder.Use(action);
            return this;
        }

        public IDataProcessingBuilder UseMiddleware<T>() where T : IInvoker
        {
            var invoker =  _container.Resolve<T>();
            Use(invoker.Invoke);
            return this;
        }
    }
}