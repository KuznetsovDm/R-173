using R_173.ViewModels;

namespace R_173.Views.TrainingSteps
{
    interface ITrainingStep
    {
        string Caption { get; }
        object DataContext { get; }
    }
}
