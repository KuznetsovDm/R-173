using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using P2PMulticastNetwork;
using P2PMulticastNetwork.Interfaces;
using P2PMulticastNetwork.Model;
using R_173.Extensions;
using R_173.Handlers;
using RadioPipeline;
using Unity;
using Unity.Lifetime;

namespace R_173
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IUnityContainer ServiceCollection;

        public static ISamplePlayer Player;

        public static IDataMiner Server;

        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigureIOC();
            BuildDataPipeline();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Player.Dispose();
            Server.Dispose();
            base.OnExit(e);
        }

        private void BuildDataPipeline()
        {

            var builder = ServiceCollection.Resolve<IDataProcessingBuilder>();
            var pipelineDelegate = builder.Use(async (data, next) =>
            {
                await next.Invoke(data);
            })
            .UseMiddleware<AudioMixerHandler>()
            .Use(async (data, next) =>
            {
                Console.WriteLine("InsideMethod");
                //data...
            })
            .Build();

            Server = ServiceCollection.Resolve<IDataMiner>();
            var converter = ServiceCollection.Resolve<IDataAsByteConverter<DataModel>>();
            Server.OnDataAwaliable(async (bytes) =>
            {
                var model = converter.ConvertFrom(bytes);
                await pipelineDelegate.Invoke(model);
            });

            Server.Start();
            Player = ServiceCollection.Resolve<ISamplePlayer>();
            Player.Play();
        }



        private void ConfigureIOC()
        {
            IUnityContainer container = new UnityContainer();
            container.AddCommunicationServices()
                     .AddAudioServices();

            container.RegisterType<IRadioManager, RadioManager>(new SingletonLifetimeManager());

            ServiceCollection = container;
        }
    }
}
