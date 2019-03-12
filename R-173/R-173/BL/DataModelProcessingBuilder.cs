using System;
using System.Threading.Tasks;
using P2PMulticastNetwork;
using P2PMulticastNetwork.Model;
using R_173.Interfaces;
using Unity;

namespace R_173.BL
{
    public class DataModelProcessingBuilder : IDataProcessingBuilder
    {
        private readonly PipelineBuilder<DataModel> _builder;
        private readonly IUnityContainer _container;

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