using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace IOLab.Resources.Control.Custom
{
    public partial class SearchComboBox : ComboBox
    {
        static SearchComboBox()
        {
            // 为控件提供默认的样式
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchComboBox), new FrameworkPropertyMetadata(typeof(SearchComboBox)));
        }

        // 数据源的依赖属性
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<Item>), typeof(SearchComboBox));

        public ObservableCollection<Item> ItemsSource
        {
            get { return (ObservableCollection<Item>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public SearchComboBox()
        {
            // 初始化
            this.IsEditable = true;
            this.IsTextSearchEnabled = true;
            this.GotFocus += SearchComboBox_GotFocus;
            this.LostFocus += SearchComboBox_LostFocus;
        }

        public void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // 清空文本框
            var comboBox = sender as SearchComboBox;
            comboBox.ClearText();
        }

        // 在模板加载后获取 TextBox 控件并处理 TextChanged 事件
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // 获取 TextBox 控件
            var editableTextBox = GetTemplateChild("PART_EditableTextBox") as TextBox;
            if (editableTextBox != null)
            {
                editableTextBox.TextChanged += SearchComboBox_TextChanged;
            }
        }

        // 文本变化时过滤数据
        private void SearchComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = this.Text.ToLower();
            var filteredItems = ItemsSource.Where(item => item.Name.ToLower().Contains(filterText)).ToList();
            this.ItemsSource = new ObservableCollection<Item>(filteredItems);
        }

        // 获取焦点时展开下拉框
        private void SearchComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.IsDropDownOpen = true;
        }

        // 失去焦点时关闭下拉框
        private void SearchComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.IsDropDownOpen = false;
        }

        // 清空文本框内容
        public void ClearText()
        {
            this.Text = string.Empty;
        }
    }

    // 数据模型
    public class Item
    {
        public string Name { get; set; }
    }
}
