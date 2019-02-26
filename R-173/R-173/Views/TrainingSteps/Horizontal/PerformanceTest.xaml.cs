namespace R_173.Views.TrainingSteps.Horizontal
{
    /// <summary>
    /// Interaction logic for PerformanceTest.xaml
    /// </summary>
    public partial class PerformanceTest : ITrainingStep
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
