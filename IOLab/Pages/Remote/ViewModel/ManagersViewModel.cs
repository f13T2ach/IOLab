using IOLab.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

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
