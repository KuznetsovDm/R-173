using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using P2PMulticastNetwork;
using P2PMulticastNetwork.Model;
using P2PMulticastNetwork.Interfaces;
using Unity;
using Unity.Lifetime;

namespace R_173
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IUnityContainer ServiceCollection;

        public MainWindow()
        {
            InitializeComponent();
            ConfigureIOC();
            BuildDataPipeline();
        }

        private void BuildDataPipeline()
        {
            var builder = ServiceCollection.Resolve<DataProcessingBuilder<DataModel>>();
            
        }

        private void ConfigureIOC()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IDataMiner, DataEngineMiner>();

            IDataMiner miner = new DataEngineMiner();
            container.RegisterInstance<IDataMiner>(miner, new SingletonLifetimeManager());
            container.RegisterInstance<IDataProvider>(miner, new SingletonLifetimeManager());
            container.RegisterInstance<IDataAsByteConverter<DataModel>>(new DataModelConverter());
            container.RegisterType<DataProcessingBuilder<DataModel>>();
            ServiceCollection = container;
        }
    }

    public class TabSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TabControl tabControl = values[0] as TabControl;
            double width = tabControl.ActualWidth / (tabControl.Items.Count);
            return (width <= 1) ? 0 : (width - 1);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
