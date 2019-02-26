namespace R_173.Views.TrainingSteps.Vertical
{
    /// <summary>
    /// Interaction logic for FrequencyCheck.xaml
    /// </summary>
    public partial class FrequencyCheck : ITrainingStep
    {
        public FrequencyCheck()
        {
            InitializeComponent();
        }

        public string Caption => Horizontal.FrequencyCheck.StepCaption;

        public StepsTypes Type => StepsTypes.FrequencyCheck;
    }
}
