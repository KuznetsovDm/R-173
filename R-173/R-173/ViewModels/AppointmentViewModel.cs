using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R_173.ViewModels
{
    class AppointmentViewModel : ViewModelBase
    {
        private RadioViewModel _radioViewModel;

        public AppointmentViewModel()
        {
            _radioViewModel = new RadioViewModel(blackoutIdEnabled: true);
        }


        public RadioViewModel RadioViewModel => _radioViewModel;
    }
}
