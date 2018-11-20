using System.Windows.Controls;

namespace R_173.Views.TrainingSteps.Horizontal
{
    /// <summary>
    /// Interaction logic for FrequencyCheck.xaml
    /// </summary>
    public partial class FrequencyCheck : UserControl, ITrainingStep
    {
        public static string StepCaption = "Подготовка рабочих частот";

        public FrequencyCheck()
        {
            InitializeComponent();
        }


        public string Caption => StepCaption;

        public StepsTypes Type => StepsTypes.FrequencyCheck;
    }
}
