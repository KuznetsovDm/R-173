using System.Windows.Controls;

namespace R_173.Views.TrainingSteps.Vertical
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

        public string Caption => Horizontal.PerformanceTest.StepCaption;

        public StepsTypes Type => StepsTypes.PerformanceTest;
    }
}
