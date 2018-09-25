using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using P2PMulticastNetwork;
using P2PMulticastNetwork.Interfaces;
using P2PMulticastNetwork.Model;
using R_173.BL;
using R_173.Extensions;
using R_173.Handlers;
using R_173.Interfaces;
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

            var builderInput = ServiceCollection.Resolve<IDataProcessingBuilder>();
            var inputPipeline = builderInput.Use(async (data, next) =>
            {
                try
                {
                    await next.Invoke(data);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                }
            })
            .UseMiddleware<AudioMixerHandler>()
            .Build();

            var builderOutput = ServiceCollection.Resolve<IDataProcessingBuilder>();
            //var outputPipeline = builderOutput.

            Server = ServiceCollection.Resolve<IDataMiner>();
            var converter = ServiceCollection.Resolve<IDataAsByteConverter<DataModel>>();
            Server.OnDataAwaliable(async (bytes) =>
            {
                var model = converter.ConvertFrom(bytes);
                await inputPipeline.Invoke(model);
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
