namespace R_173.Views.TrainingSteps
{
    public interface ITrainingStep
    {
        string Caption { get; }
        object DataContext { get; }
        StepsTypes Type { get; }
    }
}
