using iNKORE.UI.WPF.Modern.Controls;
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
