using System.ComponentModel;

namespace R_173.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(string prop = "")
        {
            var c = PropertyChanged;
            c?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
