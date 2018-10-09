using NAudio.Wave;
using P2PMulticastNetwork;
using P2PMulticastNetwork.Model;
using P2PMulticastNetwork.Interfaces;
using Unity;
using Unity.Lifetime;
using R_173.Handlers;
using NAudio.Wave.SampleProviders;
using R_173.Interfaces;
using R_173.BL;

namespace R_173.Extensions
{
    public static class RadioCommunicationExtensions
    {
        public static IUnityContainer AddCommunicationServices(this IUnityContainer services)
        {
            services.RegisterInstance<IDataReceiver>(
                new UdpMulticastConnection(
                MulticastConnectionOptions.Create(exclusiveAddressUse: false, multicastLoopback: true, useBind: true)));
            IDataProvider miner = new DataEngineMiner(services.Resolve<IDataReceiver>());
            services.RegisterInstance<IDataProvider>(miner, new SingletonLifetimeManager());
            services.RegisterInstance<IDataAsByteConverter<DataModel>>(new DataModelConverter());
            services.RegisterType<IDataProcessingBuilder, DataModelProcessingBuilder>();
            services.RegisterInstance<IDataTransmitter>(new UdpMulticastConnection(
                MulticastConnectionOptions.Create(exclusiveAddressUse: false, useBind: false)));
            return services;
        }

        public static IUnityContainer AddAudioServices(this IUnityContainer services)
        {
            services.RegisterInstance(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
            var format = services.Resolve<WaveFormat>();
            var mixer = new MixingSampleProvider(format);
            //it's meant that mixer will receive samples continuous.
            mixer.ReadFully = true;
            services.RegisterInstance<MixingSampleProvider>(mixer, new SingletonLifetimeManager());
            var player = new SamplePlayer(format);
            player.Add(mixer);
            var noise = new NoiseProvider();
            player.Add(noise);
            services.RegisterInstance<IGlobalNoiseController>(noise);
            services.RegisterInstance<ISamplePlayer>(player, new SingletonLifetimeManager());
            services.RegisterSingleton<ToneProvider>();
            return services;
        }
    }
}
