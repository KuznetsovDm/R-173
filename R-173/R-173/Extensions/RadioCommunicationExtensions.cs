using NAudio.Wave;
using P2PMulticastNetwork;
using P2PMulticastNetwork.Model;
using P2PMulticastNetwork.Interfaces;
using Unity;
using Unity.Lifetime;
using NAudio.Wave.SampleProviders;
using P2PMulticastNetwork.Network;
using R_173.Interfaces;
using R_173.BL;
using Unity.Injection;
using System.Net;
using static P2PMulticastNetwork.Network.RedistLocalConnectionTable;
using R_173.BE;

namespace R_173.Extensions
{
	public static class RadioCommunicationExtensions
	{
		public static IUnityContainer AddCommunicationServices(this IUnityContainer services)
		{
			var listenOptions = MulticastConnectionOptions.Create(ipAddress: "225.0.0.0",
				exclusiveAddressUse: false);

			var senderOptions = MulticastConnectionOptions.Create(ipAddress: "225.0.0.0",
				exclusiveAddressUse: false,
				useBind: false);

			var listenConnection = new UdpMulticastConnection(listenOptions);
			var senderConnection = new UdpMulticastConnection(senderOptions);

			services.AddLocalConnectionTable();
			services.RegisterInstance<IDataReceiver>(listenConnection);
			services.RegisterInstance<IDataTransmitter>(senderConnection);
			IDataProvider miner = new DataEngineMiner(services.Resolve<IDataReceiver>());
			services.RegisterInstance(miner, new SingletonLifetimeManager());
			services.RegisterInstance<IDataAsByteConverter<DataModel>>(new DataModelConverter());
			services.RegisterType<IDataProcessingBuilder, DataModelProcessingBuilder>();
			return services;
		}

		public static IUnityContainer AddLocalConnectionTable(this IUnityContainer services)
		{
			//use default
			var redistributableOption = new RedistributableTableOption();
			var searchTableOption = new UdpConnectionOption { Address = IPAddress.Any, Port = 33100 };
			var table = new RedistLocalConnectionTable(searchTableOption, redistributableOption);
			var settings = services.Resolve<RadioSettings>();

			var localEp = new IPEndPoint(settings.LocalIp, searchTableOption.Port);
			var notificationData = new NotificationData { Id = settings.NetworkToken, Endpoint = localEp };
			table.Register(notificationData);
			services.RegisterInstance<IRedistributableLocalConnectionTable>(table, new SingletonLifetimeManager());
			return services;
		}

		public static IUnityContainer AddAudioServices(this IUnityContainer services)
		{
			services.RegisterInstance(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
			services.RegisterWavePlayer();
			services.RegisterWaveIn();
			var format = services.Resolve<WaveFormat>();
			var mixer = new MixingSampleProvider(format) { ReadFully = true };
			services.RegisterInstance(mixer, new SingletonLifetimeManager());

			var player = services.Resolve<SamplePlayer>();
			player.Add(mixer);
			var noise = new NoiseProvider(format);
			player.Add(noise);
			services.RegisterInstance<IGlobalNoiseController>(noise);
			services.RegisterInstance<ISamplePlayer>(player, new SingletonLifetimeManager());
			services.RegisterSingleton<ToneProvider>();
			return services;
		}

		public static IUnityContainer RegisterWavePlayer(this IUnityContainer services)
		{
            //регистрируем пустышки для машин на которых нет устройства с выводом.
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
