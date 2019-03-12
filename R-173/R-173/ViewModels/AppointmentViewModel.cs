namespace R_173.ViewModels
{
    class AppointmentViewModel : ViewModelBase
    {
        private readonly RadioViewModel _radioViewModel;

        public AppointmentViewModel()
        {
            _radioViewModel = new RadioViewModel(blackoutIdEnabled: true);
        }


        public RadioViewModel RadioViewModel => _radioViewModel;
    }
}
