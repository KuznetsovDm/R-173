namespace R_173.ViewModels
{
    class TrainingStepViewModel : ViewModelBase
    {
        private int _currentStep;
        private int _stepCount;

        public TrainingStepViewModel(int stepCount)
        {
            _stepCount = stepCount;
            _currentStep = -1;
        }


        public int CurrentStep
        {
            get => _currentStep;
            set
            {
                if (value == _currentStep)
                    return;
                _currentStep = value;
                OnPropertyChanged(nameof(CurrentStep));
            }
        }

        public void SetMaxStep()
        {
            CurrentStep = _stepCount;
        }
    }
}
