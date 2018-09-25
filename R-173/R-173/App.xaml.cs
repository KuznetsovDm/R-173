using System.Windows;
using P2PMulticastNetwork.Interfaces;
using P2PMulticastNetwork.Model;
using R_173.BL;
using R_173.Extensions;
using R_173.Interfaces;
using Unity;
using Unity.Lifetime;

namespace R_173
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UnityContainer ServiceCollection;

        public static ISamplePlayer Player;

        public static IDataProvider Server;

        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigureIOC();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Player.Dispose();
            Server.Dispose();
            base.OnExit(e);
        }

        private void ConfigureIOC()
        {
            IUnityContainer container = new UnityContainer();
            container.AddCommunicationServices()
                     .AddAudioServices();

            container.RegisterType<IRadioManager, RadioManager>(new SingletonLifetimeManager());
            container.RegisterType<IAudioReaderAndSender<SendableRadioModel>, AudioReaderAndSender>(new SingletonLifetimeManager());
            container.RegisterType<IAudioReceiverAndPlayer<ReceivableRadioModel>, AudioReceiverAndPlayer>(new SingletonLifetimeManager());
            container.RegisterType<IMicrophone, Microphone>(new SingletonLifetimeManager());

            var manager = container.Resolve<IRadioManager>();
            var model = new Models.RadioModel();
            manager.SetModel(model);
            model.Power = ModelS
            ServiceCollection = container;
        }
    }
}
