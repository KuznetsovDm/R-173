using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace R_173.Views.TrainingSteps.Vertical
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

        public string Caption => Horizontal.FrequencyCheck.StepCaption;

        public StepsTypes Type => StepsTypes.FrequencyCheck;
    }
}
