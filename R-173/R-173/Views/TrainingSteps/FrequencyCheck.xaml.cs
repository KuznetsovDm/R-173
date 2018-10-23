using System.Windows.Controls;

namespace R_173.Views.TrainingSteps
{
    /// <summary>
    /// Interaction logic for FrequencyCheck.xaml
    /// </summary>
    public partial class FrequencyCheck : UserControl, ITrainingStep
    {
        public FrequencyCheck()
        {
            InitializeComponent();
        }


        public string Caption => "Подготовка рабочих частот";
    }
}
