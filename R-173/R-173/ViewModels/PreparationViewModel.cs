namespace R_173.ViewModels
{
    class TrainingStepViewModel : ViewModelBase
    {
        private int _currentStep;

        public TrainingStepViewModel()
        {
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
    }
}
