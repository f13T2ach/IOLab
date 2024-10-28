// 文件出处 https://www.bilibili.com/read/cv39493164 以上版权声明为自动生成 此文件中不具备效力
using System.Windows;

namespace IOLab.Resources.Guide
{
    public static class EzGuide
    {
        public static readonly DependencyProperty StepIndexProperty =
        DependencyProperty.RegisterAttached("StepIndex", typeof(int), typeof(EzGuide), new PropertyMetadata(0));

        public static void SetIndex(UIElement element, int value)
        {
            element.SetValue(StepIndexProperty, value);
        }

        public static int GetIndex(UIElement element)
        {
            return (int)element.GetValue(StepIndexProperty);
        }

        public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.RegisterAttached("Description", typeof(string), typeof(EzGuide), new PropertyMetadata(string.Empty));

        public static void SetDescription(UIElement element, string value)
        {
            element.SetValue(DescriptionProperty, value);
        }

        public static string GetDescription(UIElement element)
        {
            return (string)element.GetValue(DescriptionProperty);
        }

        public static readonly DependencyProperty TitleProperty =
        DependencyProperty.RegisterAttached("Title", typeof(string), typeof(EzGuide), new PropertyMetadata(string.Empty));

        public static void SetTitle(UIElement element, string value)
        {
            element.SetValue(TitleProperty, value);
        }

        public static string GetTitle(UIElement element)
        {
            return (string)element.GetValue(TitleProperty);
        }
    }
}