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
            foreach (UIElement child in InternalChildren)
            {
                child.Measure(availableSize);
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int count = InternalChildren.Count;
            if (count == 0) return finalSize;

            // 计算行数和列数，使得行列数量尽可能接近
            int rows = (int)Math.Sqrt(count);
            int columns = (int)Math.Ceiling((double)count / rows);

            // 计算最终的子元素大小
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
