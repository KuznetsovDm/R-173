using NAudio.Wave;
using P2PMulticastNetwork;
using P2PMulticastNetwork.Model;
using P2PMulticastNetwork.Interfaces;
using Unity;
using Unity.Lifetime;
using NAudio.Wave.SampleProviders;
using R_173.Interfaces;
using R_173.BL;
using Unity.Injection;

namespace R_173.Extensions
{
    public static class RadioCommunicationExtensions
    {
        public static IUnityContainer AddCommunicationServices(this IUnityContainer services)
        {
            var listenOptions = MulticastConnectionOptions.Create(ipAddress: "225.0.0.0",
                exclusiveAddressUse: false,
                multicastLoopback: false,
                useBind: true);

            var senderOptions = MulticastConnectionOptions.Create(ipAddress: "225.0.0.0",
                exclusiveAddressUse: false,
                useBind: false);

            var listenConnection = new UdpMulticastConnection(listenOptions);
            var senderConnection = new UdpMulticastConnection(senderOptions);

            services.RegisterInstance<IDataReceiver>(listenConnection);
            services.RegisterInstance<IDataTransmitter>(senderConnection);
            IDataProvider miner = new DataEngineMiner(services.Resolve<IDataReceiver>());
            services.RegisterInstance<IDataProvider>(miner, new SingletonLifetimeManager());
            services.RegisterInstance<IDataAsByteConverter<DataModel>>(new DataModelConverter());
            services.RegisterType<IDataProcessingBuilder, DataModelProcessingBuilder>();
            return services;
        }

        public static IUnityContainer AddAudioServices(this IUnityContainer services)
        {
            services.RegisterInstance(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
            services.RegisterWavePlayer();
            services.RegisterWaveIn();
            var format = services.Resolve<WaveFormat>();
            var mixer = new MixingSampleProvider(format);
            mixer.ReadFully = true;
            services.RegisterInstance<MixingSampleProvider>(mixer, new SingletonLifetimeManager());

            var player = services.Resolve<SamplePlayer>();
            player.Add(mixer);
            var noise = new NoiseProvider();
            player.Add(noise);
            services.RegisterInstance<IGlobalNoiseController>(noise);
            services.RegisterInstance<ISamplePlayer>(player, new SingletonLifetimeManager());
            services.RegisterSingleton<ToneProvider>();
            return services;
        }

        public static IUnityContainer RegisterWavePlayer(this IUnityContainer services)
        {
            if (WaveOut.DeviceCount > 0)
                services.RegisterType<IWavePlayer, WaveOut>(new InjectionConstructor());
            else
                services.RegisterType<IWavePlayer, EmptyWrapperWavePlayer>();
            return services;
        }

        public static IUnityContainer RegisterWaveIn(this IUnityContainer services)
        {
            if (WaveInEvent.DeviceCount > 0)
                services.RegisterType<IWaveIn, WaveInEvent>();
            else
                services.RegisterType<IWaveIn, EmptyWrapperWaveIn>();
            return services;
        }
    }
}
