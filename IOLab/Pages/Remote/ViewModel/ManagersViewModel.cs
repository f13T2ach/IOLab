using IOLab.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace IOLab.Pages.Remote.ViewModel
{
    class ManagersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand DisconnectCommand { get; set; }

        public ManagersViewModel()
        {
            DisconnectCommand = new RelayCommand(OnUIDisconnectDevice);
        }

        public void OnUIDisconnectDevice(object sender)
        {

        }

        private void OnPropertyChanged(string name = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


    }
}
