using iNKORE.UI.WPF.Modern.Controls;
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
using System.Windows.Shapes;
using IOLab.Interfaces.ViewModel;

namespace IOLab.Windows.Dialog.Universal
{
    /// <summary>
    /// NameEdit.xaml 的交互逻辑
    /// </summary>
    public partial class NameEdit : ContentDialog
    {
        public NameEdit(INameEditViewModel model)
        {
            InitializeComponent();
            this.DataContext = model;
        }
    }
}
