// 文件出处 https://www.bilibili.com/read/cv39493164 以上版权声明为自动生成 此文件中不具备效力
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace IOLab.Resources.Guide
{
    public class EzGuideManager
    {
        private readonly Window _window;
        private int _currentStep = 0;// 管理器步骤
        private Border? _infoBorder;
        private Path? _overlay;
        private Canvas? _overlayInfo;
        private Grid? _baseGrid;
        private EzGuideStepViewModel ezGuideStepViewModel;
        public EzGuideManager(Window window)
        {
            _window = window;
            //如果 Content 不是 Grid，或者是 Grid 但是 Name 不是 BaseGrid ，那么新建一个名为BaseGrid的Grid
            if ((_window.Content as Grid)?.Name != "EzBaseGrid")
            {
                _baseGrid = new Grid();
                _baseGrid.Name = "EzBaseGrid";
                UIElement oricontent = _window.Content as UIElement;
                _window.Content = null;
                _baseGrid.Children.Add(oricontent);
                _window.Content = _baseGrid;
            }
            else
            {
                _baseGrid = _window.Content as Grid;
            }

            // 如果 baseGrid 中没有 Overlay，那么新建一个
            _overlay = _baseGrid?.Children.OfType<Path>().FirstOrDefault(x => x.Name == "EzOverlay");
            if (_overlay == null)
            {
                _overlay = new Path
                {
                    Name = "EzOverlay",
                    Fill = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0)),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                };
                _baseGrid?.Children.Add(_overlay);
            }

            _overlayInfo = _baseGrid?.Children.OfType<Canvas>().FirstOrDefault(x => x.Name == "EzOverlayInfo");
            if (_overlayInfo == null)
            {
                _overlayInfo = new Canvas
                {
                    Name = "EzOverlayInfo",
                    Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    IsHitTestVisible = true
                };

                // 使用 ContentControl 来加载 DataTemplate
                ContentControl contentControl = new ContentControl();

                // 设置 DataTemplate，查找资源字典中的模板
                contentControl.ContentTemplate = _window.FindResource("GuideLayoutTemplate") as DataTemplate;

                // 创建 ViewModel 用于绑定命令（下一步和跳过）
                ezGuideStepViewModel = new(NextStep, () => JumpToStep(0));

                // 将 ViewModel 绑定到 ContentControl 的 Content 上
                contentControl.Content = ezGuideStepViewModel;

                // 将 ContentControl 放入 Border
                _infoBorder = new Border();
                //_infoBorder.SetResourceReference(Border.StyleProperty, "BorderRegion");
                _infoBorder.Child = contentControl;

                // 将 Border 添加到遮罩层中
                _overlayInfo.Children.Add(_infoBorder);
                _baseGrid?.Children.Add(_overlayInfo);
            }
            _window.UpdateLayout();// 需要即刻更新
        }

        public void NextStep()
        {
            _currentStep++;
            ShowStep(_currentStep);
        }
        public void JumpToStep(int index = 1)
        {
            _currentStep = index;
            ShowStep(_currentStep);
        }
        private bool ShowStep(int stepIndex)
        {
            if (stepIndex == 0)
            {
                _overlay.Data = null;
                _overlayInfo.IsHitTestVisible = false;
                _overlayInfo.Visibility = Visibility.Collapsed;
                return false;
            }
            // 遍历窗口中的所有控件，寻找具有 StepIndex 的控件
            var elements = GetAllElements(_baseGrid);
            foreach (UIElement element in elements)
            {
                int index = EzGuide.GetIndex(element);
                if (index == stepIndex)
                {
                    _overlayInfo.IsHitTestVisible = true;
                    _overlayInfo.Visibility = Visibility.Visible;
                    Rect baseBounds = new Rect(0, 0, _baseGrid.RenderSize.Width, _baseGrid.RenderSize.Height);
                    Rect elementBounds = element.TransformToAncestor(_baseGrid).TransformBounds(new Rect(element.RenderSize));
                    elementBounds.X -= 4;
                    elementBounds.Y -= 4;
                    elementBounds.Width += 8;
                    elementBounds.Height += 8;
                    CombinedGeometry combinedGeometry = new CombinedGeometry(
                    GeometryCombineMode.Exclude,
                    new RectangleGeometry(baseBounds),   // 覆盖整个窗口的矩形
                                new RectangleGeometry(elementBounds) { RadiusX = 3, RadiusY = 3 }); // 挖空的高亮区域
                    _overlay.Data = combinedGeometry;
                    ezGuideStepViewModel.TitleString = EzGuide.GetTitle(element);
                    ezGuideStepViewModel.DescriptString = EzGuide.GetDescription(element);
                    _infoBorder?.UpdateLayout();
                    _infoBorder?.SetValue(Canvas.LeftProperty, elementBounds.X - _infoBorder.RenderSize.Width);
                    _infoBorder?.SetValue(Canvas.TopProperty, elementBounds.Y);
                    return true;
                }
            }
            _overlayInfo.IsHitTestVisible = false;
            _overlayInfo.Visibility = Visibility.Collapsed;
            _overlay.Data = null;
            return false; // 没有更多步骤时返回 false
        }
        private static IEnumerable<UIElement> GetAllElements(DependencyObject parent)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is UIElement element)
                {
                    yield return element;
                }

                foreach (var grandChild in GetAllElements(child))
                {
                    yield return grandChild;
                }
            }
        }
    }
}
