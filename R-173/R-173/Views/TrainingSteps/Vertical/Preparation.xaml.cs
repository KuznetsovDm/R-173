using System.Windows.Controls;

namespace R_173.Views.TrainingSteps.Vertical
{
    /// <summary>
    /// Interaction logic for Preparation.xaml
    /// </summary>
    public partial class Preparation : UserControl, ITrainingStep
    {
        public Preparation()
        {
            InitializeComponent();
        }

        public string Caption => Horizontal.Preparation.StepCaption;

        public StepsTypes Type => StepsTypes.Preparation;
    }
}
