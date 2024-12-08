using IOLab.Core.Commands;
using IOLab.Core.Generic;
using IOLab.Core.Network.Data;
using IOLab.Network;
using IOLab.Windows.Dialog;
using IOLab.Windows.Dialog.Online;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace IOLab.Pages.ViewModel
{
    internal class WorkshopPageViewModel : INotifyPropertyChanged
    {
        public ICommand RefreshCommand { get; set; }

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
            ActionBundle.WorkshopContentShowInformation += ShowWorkshopContent;

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

        public async void ShowWorkshopContent(WorkshopContent workshopContent)
        {
            if (workshopContent == null || workshopContent.IsEmpty())
                return;
            WorkshopContentUI dialog = new WorkshopContentUI(workshopContent);
            await dialog.ShowAsync();
        }
        /**
         * System.InvalidOperationException
  HResult=0x80131509
  Message=Only a single ContentDialog can be open at any time.
  Source=iNKORE.UI.WPF.Modern.Controls
  StackTrace:
   在 iNKORE.UI.WPF.Modern.Controls.ContentDialog.ThrowAlreadyOpenException()
   在 iNKORE.UI.WPF.Modern.Controls.ContentDialog.ThrowIfHasOpenDialog(Window owner)
   在 iNKORE.UI.WPF.Modern.Controls.ContentDialog.<ShowAsync>d__135.MoveNext()
   在 iNKORE.UI.WPF.Modern.Controls.ContentDialog.<ShowAsync>d__134.MoveNext()
   在 IOLab.Pages.ViewModel.WorkshopPageViewModel.<ShowWorkshopContent>d__11.MoveNext() 在 D:\IOLab\IOLab\IOLab\Pages\ViewModel\WorkshopPageViewModel.cs 中: 第 60 行
**/

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
