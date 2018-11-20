using System.Windows.Controls;

namespace R_173.Views.TrainingSteps.Horizontal
{
    /// <summary>
    /// Interaction logic for PerformanceTest.xaml
    /// </summary>
    public partial class PerformanceTest : UserControl, ITrainingStep
    {
        public static string StepCaption = "Проверка работоспособности";

        public PerformanceTest()
        {
            InitializeComponent();
        }


        public string Caption => StepCaption;

        public StepsTypes Type => StepsTypes.PerformanceTest;
    }
}
