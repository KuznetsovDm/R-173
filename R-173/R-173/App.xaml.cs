using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using P2PMulticastNetwork.Interfaces;
using P2PMulticastNetwork.Model;
using R_173.BE;
using R_173.BL;
using R_173.BL.Learning;
using R_173.Extensions;
using R_173.Handlers;
using R_173.Interfaces;
using R_173.ViewModels;
using R_173.Views;
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
        private MainWindow _mainWindow;
        private readonly object _mainWindowLock = new object();
        private Preloader _preloader;

        protected override void OnStartup(StartupEventArgs e)
        {
            lock (_mainWindowLock)
            {
                var preloaderThread = new Thread(() =>
                {
                    _preloader = new Preloader();
                    _preloader.ContentRendered += Preloader_ContentRendered;
                    _preloader.Closed += delegate
                    {
                        _preloader.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
                    };
                    _preloader.ShowDialog();
                    Dispatcher.Run();
                });

                preloaderThread.SetApartmentState(ApartmentState.STA);
                preloaderThread.IsBackground = true;
                preloaderThread.Start();

                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                ConfigureIOC();
                base.OnStartup(e);
                _mainWindow = ServiceCollection.Resolve<MainWindow>();
            }
        }

        private void Preloader_ContentRendered(object sender, EventArgs ev)
        {
            lock (_mainWindowLock)
            {
                _mainWindow.Dispatcher.BeginInvoke((Action)(() =>
                {
                    _mainWindow.ContentRendered += delegate
                    {
                        _mainWindow.Activate();
                        _preloader.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            _preloader.Close();
                        }));
                    };
                    _mainWindow.Show();
                }));
            }
        }


        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            SimpleLogger.Log((Exception)e.ExceptionObject);
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

            var binder = new JsonBinder<ActionDescriptionOption>();
            var option = binder.BindFromFile(R_173.Properties.Resources.JsonRadioStationTextPath);

            container.RegisterInstance<ActionDescriptionOption>(option, new SingletonLifetimeManager());
            container.RegisterType<IRadioManager, RadioManager>(new SingletonLifetimeManager());
            container.RegisterType<IAudioReaderAndSender<SendableRadioModel>, AudioReaderAndSender>(new SingletonLifetimeManager());
            container.RegisterType<IAudioReceiverAndPlayer<ReceivableRadioModel>, AudioReceiverAndPlayer>(new SingletonLifetimeManager());
            container.RegisterType<IMicrophone, Microphone>(new SingletonLifetimeManager());
            container.RegisterType<IMessageBox, MessageBoxViewModel>(new SingletonLifetimeManager());
            container.RegisterType<KeyboardHandler>(new SingletonLifetimeManager());
            container.RegisterType<MainWindow>(new SingletonLifetimeManager());
            ServiceCollection = container;

        }
    }
}
