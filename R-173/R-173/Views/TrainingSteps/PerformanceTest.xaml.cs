using System.Windows.Controls;

namespace R_173.Views.TrainingSteps
{
    /// <summary>
    /// Interaction logic for PerformanceTest.xaml
    /// </summary>
    public partial class PerformanceTest : UserControl, ITrainingStep
    {
        public PerformanceTest()
        {
            InitializeComponent();
        }


        public string Caption => "Проверка работоспособности";
    }
}
