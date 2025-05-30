using IOLab.Core.Commands;
using IOLab.Core.Generic;
using IOLab.Core.Network.Data;
using IOLab.Network;
using IOLab.Resources.Control.Custom;
using IOLab.Windows.Dialog;
using IOLab.Windows.Dialog.Online;
using OfficeOpenXml;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace IOLab.Pages.ViewModel
{
    internal class WorkshopPageViewModel : INotifyPropertyChanged
    {
        public ICommand RefreshCommand { get; set; }
        public ICommand UploadCommand { get; set; }
        public ObservableCollection<Item> SearchItems { get; set; }

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

            SearchItems = new ObservableCollection<Item>
            {
                new Item { Name = "Apple" },
                new Item { Name = "Banana" },
                new Item { Name = "Cherry" },
                new Item { Name = "Date" },
                new Item { Name = "Grape" },
                new Item { Name = "Melon" },
                new Item { Name = "Pineapple" },
                new Item { Name = "Watermelon" }
            };

            RefreshContents(true);

            ActionBundle.LoginStatusChange += RefreshCommandAction;
            ActionBundle.WorkshopContentShowInformation += ShowWorkshopContent;

            RefreshCommand = new RelayCommand(RefreshCommandDo);
            UploadCommand = new RelayCommand(UploadCommandDo);
        }

        public void RefreshCommandAction()
        {
            RefreshContents(true);
        }
        public void RefreshCommandDo(object parameter)
        {
            RefreshContents();
        }

        public async void UploadCommandDo(object parameter)
        {
            UploadExperiment uploadExperiment = new UploadExperiment();
            await uploadExperiment.ShowAsync();

        }

        

        public async void ShowWorkshopContent(WorkshopContent workshopContent)
        {
            if (workshopContent == null || workshopContent.IsEmpty())
                return;
            WorkshopContentUI dialog = new WorkshopContentUI(workshopContent);
            await dialog.ShowAsync();
        }


        public async void RefreshContents(bool isAuto = false)
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
