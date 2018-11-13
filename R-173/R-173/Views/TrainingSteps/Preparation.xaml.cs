using System.Windows.Controls;

namespace R_173.Views.TrainingSteps
{
    /// <summary>
    /// Interaction logic for Preparation.xaml
    /// </summary>
    public partial class Preparation : UserControl, ITrainingStep
    {
        public static string StepCaption = "Подготовка к работе";

        public Preparation()
        {
            InitializeComponent();
        }


        public string Caption => StepCaption;
    }
}
