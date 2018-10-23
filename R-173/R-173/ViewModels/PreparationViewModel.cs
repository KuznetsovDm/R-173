namespace R_173.ViewModels
{
    class PreparationViewModel : ViewModelBase
    {
        private int _currentStep;

        public PreparationViewModel()
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
