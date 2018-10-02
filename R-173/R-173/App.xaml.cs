using System.Windows;
using P2PMulticastNetwork.Interfaces;
using P2PMulticastNetwork.Model;
using R_173.BL;
using R_173.Extensions;
using R_173.Handlers;
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
        public static IUnityContainer ServiceCollection;

        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigureIOC();
            base.OnStartup(e);
            var obj = ServiceCollection.Resolve<MainWindow>();
            obj.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
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
            container.RegisterType<MainWindow>(new SingletonLifetimeManager());
            container.RegisterType<KeyboardHandler>(new SingletonLifetimeManager());
            ServiceCollection = container;

        }
    }
}
