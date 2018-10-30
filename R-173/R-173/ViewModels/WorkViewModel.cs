namespace R_173.ViewModels
{
    class WorkViewModel
    {
        private readonly RadioViewModel _radioViewModel;

        public WorkViewModel()
        {
            _radioViewModel = new RadioViewModel();
        }

        public RadioViewModel RadioViewModel => _radioViewModel;
    }
}
