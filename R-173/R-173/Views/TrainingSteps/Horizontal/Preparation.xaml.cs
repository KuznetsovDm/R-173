namespace R_173.Views.TrainingSteps.Horizontal
{
    /// <summary>
    /// Interaction logic for Preparation.xaml
    /// </summary>
    public partial class Preparation : ITrainingStep
    {
        public static string StepCaption = "Подготовка к работе";

        public Preparation()
        {
            InitializeComponent();
        }


        public string Caption => StepCaption;

        public StepsTypes Type => StepsTypes.Preparation;
    }
}
