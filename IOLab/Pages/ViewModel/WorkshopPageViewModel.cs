using IOLab.Commands;
using IOLab.Core.Generic;
using IOLab.Core.Network.Data;
using IOLab.Network;
using IOLab.Windows.Dialog;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IOLab.Pages.ViewModel
{
    internal class WorkshopPageViewModel : INotifyPropertyChanged
    {
        public ICommand RefreshCommand {  get; set; }

        private ContentCollection workshopContents;
        public ContentCollection WorkshopContents
        {
            get => workshopContents;
            set
            {
                workshopContents = value;
                OnPropertyChanged(nameof(WorkshopContents));
            }
        }

        public WorkshopPageViewModel()
        {
            workshopContents = new ContentCollection();

            RefreshContents(true);

            ActionBundle.LoginStatusChange += RefreshCommandAction;

            RefreshCommand = new RelayCommand(RefreshCommandDo);
        }

        public void RefreshCommandAction()
        {
            RefreshContents(true);
        }
        public void RefreshCommandDo(object parameter)
        {
            RefreshContents();
        }

        public async void RefreshContents(bool isAuto=false)
        {
            WaitSerial wait = new WaitSerial(title: "正在获取内容");
            

            if (InstanceBundle.UsingAuth == null)
            {
                if (!isAuto)
                {
                    new MainWindow().AlertLogin("请登录以获取内容");
                }
                return;
            }

            // 非自动状态下 不显示
            if (!isAuto)
            {
                _ = wait.ShowAsync();
            }


            Workshop workshop = new Workshop(InstanceBundle.UsingAuth);

            var result = await workshop.BrowseWorkshopItemsAsync();

            wait.Hide();
            if (!result.Success)
            {
                new MainWindow().CreateDialog("获取内容失败", result.Message);
                return;
            }

            WorkshopContents = new ContentCollection(result.Data);
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
