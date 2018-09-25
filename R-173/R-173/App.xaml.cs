using System.Windows;
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
        public static IUnityContainer ServiceCollection;

        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigureIOC();
            BuildDataPipeline();
            base.OnStartup(e);
        }

        private void BuildDataPipeline()
        {

            var builder = ServiceCollection.Resolve<IDataProcessingBuilder>();
            var pipinDelegate = builder.Use(async (data, next) =>
            {
                await next.Invoke(data);
            })
            .Use(async (data, next) =>
            {
                //data...
            })
            .Build();

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
