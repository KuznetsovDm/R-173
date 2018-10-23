using R_173.Handlers;
using R_173.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;
using Unity;


namespace R_173.Views.TrainingSteps
{
    /// <summary>
    /// Interaction logic for Preparation.xaml
    /// </summary>
    public partial class Preparation : UserControl, ITrainingStep
    {
        public Preparation()
        {
            InitializeComponent();

            var preparationViewModel = new PreparationViewModel();
            DataContext = preparationViewModel;

            //Loaded += (s, e) =>
            //{
            //    App.ServiceCollection.Resolve<KeyboardHandler>().OnKeyDown = (key) =>
            //    {
            //        if (key == Key.Left)
            //            preparationViewModel.CurrentStep--;
            //        else if (key == Key.Right)
            //            preparationViewModel.CurrentStep++;
            //    };
            //};
        }
    }
}
