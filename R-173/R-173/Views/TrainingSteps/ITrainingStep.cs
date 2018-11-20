namespace R_173.Views.TrainingSteps
{
    interface ITrainingStep
    {
        string Caption { get; }
        object DataContext { get; }
        StepsTypes Type { get; }
    }
}
