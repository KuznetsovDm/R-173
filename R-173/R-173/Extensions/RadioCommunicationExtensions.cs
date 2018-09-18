using NAudio.Wave;
using P2PMulticastNetwork;
using P2PMulticastNetwork.Model;
using P2PMulticastNetwork.Interfaces;
using Unity;
using Unity.Lifetime;

namespace R_173.Extensions
{
    public static class RadioCommunicationExtensions
    {
        public static IUnityContainer AddCommunicationServices(this IUnityContainer services)
        {
            services.RegisterType<IDataMiner, DataEngineMiner>();
            IDataMiner miner = new DataEngineMiner();
            services.RegisterInstance<IDataMiner>(miner, new SingletonLifetimeManager());
            services.RegisterInstance<IDataProvider>(miner, new SingletonLifetimeManager());
            services.RegisterInstance<IDataAsByteConverter<DataModel>>(new DataModelConverter());
            services.RegisterType<IDataProcessingBuilder, DataModelProcessingBuilder>();
            return services;
        }

        public static IUnityContainer AddAudioServices(this IUnityContainer services)
        {
            services.RegisterInstance(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
            return services;
        }
    }
}
