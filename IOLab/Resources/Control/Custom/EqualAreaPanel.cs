using System;
using System.Windows;
using System.Windows.Controls;

namespace IOLab.Resources.Control.Custom
{
    public class EqualAreaPanel : Panel
    {
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            // 当子控件集合发生变化时，强制重新布局
            this.InvalidateMeasure();
            this.InvalidateArrange();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            int count = InternalChildren.Count;
            if (count == 0) return new Size();

            // 计算行数和列数
            int columns = (int)Math.Ceiling(Math.Sqrt(count));
            int rows = (int)Math.Ceiling((double)count / columns);

            // 计算子元素宽度和高度
            double childWidth = availableSize.Width / columns;
            double childHeight = availableSize.Height / rows;

            // 测量每个子元素
            foreach (UIElement child in InternalChildren)
            {
                child.Measure(new Size(childWidth, childHeight)); // 使用计算后的子元素尺寸
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int count = InternalChildren.Count;
            if (count == 0) return finalSize;

            // 计算行数和列数
            int columns = (int)Math.Ceiling(Math.Sqrt(count));
            int rows = (int)Math.Ceiling((double)count / columns);

            // 计算最终的子元素宽度和高度
            double childWidth = finalSize.Width / columns;
            double childHeight = finalSize.Height / rows;

            // 布置子元素
            for (int i = 0; i < count; i++)
            {
                int row = i / columns;
                int column = i % columns;

                double x = column * childWidth;
                double y = row * childHeight;

                Rect rect = new Rect(new Point(x, y), new Size(childWidth, childHeight));

                // 设置每个子控件的布局位置和大小
                InternalChildren[i].Arrange(rect);

                // 强制子元素填充整个区域
                if (InternalChildren[i] is FrameworkElement element)
                {
                    element.HorizontalAlignment = HorizontalAlignment.Stretch;
                    element.VerticalAlignment = VerticalAlignment.Stretch;
                }
            }

            return finalSize;
        }
    }
}
