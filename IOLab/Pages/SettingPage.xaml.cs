using iNKORE.UI.WPF.Modern;
using IOLab.Models.Related;
using IOLab.Static;
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


namespace IOLab.Pages
{

    
    /// <summary>
    /// Interaction logic for SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page
    {

        private int TotalClick = 0;
        public SettingPage()
        {
            InitializeComponent();
            LoadSettings();

            InitializeColorPicker();
            TotalClick = 0;
        }

        private void LoadSettings()
        {

        }

        private void SaveAllSettings()
        {

        }

        // 颜色选取器载入
        // 试用功能 未封装控件
        private void InitializeColorPicker()
        {

            var colors = new List<ColorItem>();
            // 遍历加入颜色
            foreach (string str in Theme.coloursHex)
            {
                colors.Add(new ColorItem { Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(str)) });
            }

            ColorPickerComboBox.ItemsSource = colors;

            Color? setColor = new ThemeResources().AccentColor;
            // 遍历找到设置颜色
            for (int index=0;index<colors.Count;index++)
            {
                ColorItem item = colors[index];
                if (item.Color.Color == setColor)
                {
                    ColorPickerComboBox.SelectedIndex = index;
                    break;
                }
            }

        }
        private void ColorPickerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ColorPickerComboBox.SelectedItem is ColorItem selectedColor)
            {
                new ThemeResources().AccentColor = selectedColor.Color.Color;
            }
        }

        // 连接社区
        private async void ToggleSwitch_UseOnlineCommunity_Toggled(object sender, RoutedEventArgs e)
        {
            if(ToggleSwitch_UseOnlineCommunity.IsOn == false)
            {
                return; // 下面的语句会重新触发此方法
            }
            ToggleSwitch_UseOnlineCommunity.IsOn = false;
            Windows.Dialog.ConnectCommunity cc = new Windows.Dialog.ConnectCommunity();
            var result = await cc.ShowAsync();
        }

        // 主题改变
        private void ComboBox_ThemeMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ComboBox_ThemeMode.SelectedIndex;

            if(index == -1)
            {
                return;
            }else if(index == 0)
            {
                if (Util.ThemeMode.GetSystemDarkMode())
                {
                    Static.Theme.themeMode = iNKORE.UI.WPF.Modern.ApplicationTheme.Dark;
                }
                else
                {
                    Static.Theme.themeMode = iNKORE.UI.WPF.Modern.ApplicationTheme.Light;
                }
            }else if(index == 1)
            {
                Static.Theme.themeMode = iNKORE.UI.WPF.Modern.ApplicationTheme.Light;
            }
            else
            {
                Static.Theme.themeMode = iNKORE.UI.WPF.Modern.ApplicationTheme.Dark;
            }
        }

        private void EntryOfGame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TotalClick++;
            if(TotalClick == 6)
            {
                TotalClick = 0;
            }
        }

        
    }
}
