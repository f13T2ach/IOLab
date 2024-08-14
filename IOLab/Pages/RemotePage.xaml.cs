using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

using IOLab.Pages.Remote;
using iNKORE.UI.WPF.Modern.Controls;

namespace IOLab.Pages
{
    /// <summary>
    /// Interaction logic for RemotePage.xaml
    /// </summary>
    public partial class RemotePage : Page
    {
        NoRemoteTarget PageRemote_NoRemoteTarget = new NoRemoteTarget();
        Managers PageRemote_Managers = new Managers();
        public RemotePage()
        {
            InitializeComponent();
            Preparations();
        }

        private void Preparations()
        {
            ViewField.Navigate(PageRemote_NoRemoteTarget);
        }
    }
}
