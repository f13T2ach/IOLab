using Emoji.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace IOLab.Resources.Control
{
    public partial class AnimatedTextSwitcher : UserControl
    {
        public static readonly DependencyProperty TextArrayProperty =
            DependencyProperty.Register("TextArray", typeof(string[]), typeof(AnimatedTextSwitcher),
                new PropertyMetadata(new string[] { "Default Text" }, OnTextArrayChanged));

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(TimeSpan), typeof(AnimatedTextSwitcher),
                new PropertyMetadata(TimeSpan.FromSeconds(2), OnIntervalChanged));

        private readonly DispatcherTimer _timer;
        private int _currentTextIndex = 0;

        public AnimatedTextSwitcher()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;

            // Set the initial interval
            _timer.Interval = Interval;
        }

        public string[] TextArray
        {
            get { return (string[])GetValue(TextArrayProperty); }
            set { SetValue(TextArrayProperty, value); }
        }

        public TimeSpan Interval
        {
            get { return (TimeSpan)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        private static void OnTextArrayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedTextSwitcher animatedTextBlock && animatedTextBlock.TextArray.Length > 0)
            {
                animatedTextBlock._currentTextIndex = 0;
                animatedTextBlock.textBlock.Text = animatedTextBlock.TextArray[0];

                // 手动设置控件宽度为第一个文本的宽度
                animatedTextBlock.textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                animatedTextBlock.animatedUserControl.Width = animatedTextBlock.textBlock.DesiredSize.Width;

                // 启动计时器以开始显示后续的文本
                animatedTextBlock._timer.Start();
            }
        }

        private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedTextSwitcher animatedTextBlock && e.NewValue is TimeSpan newInterval)
            {
                animatedTextBlock._timer.Interval = newInterval;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 开始渐变消失动画
            var fadeOutStoryboard = (Storyboard)FindResource("FadeOutStoryboard");

            fadeOutStoryboard.Completed -= FadeOutCompleted; // 移除之前的事件绑定，防止重复触发
            fadeOutStoryboard.Completed += FadeOutCompleted;

            fadeOutStoryboard.Begin();
        }

        private void FadeOutCompleted(object sender, EventArgs e)
        {
            // 更新文本
            _currentTextIndex = (_currentTextIndex + 1) % TextArray.Length;
            textBlock.Text = TextArray[_currentTextIndex];

            // 计算 TextBlock 的实际宽度并设置到 UserControl
            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            double desiredWidth = textBlock.DesiredSize.Width;
            animatedUserControl.Width = desiredWidth;

            // 开始渐变出现动画
            var fadeInStoryboard = (Storyboard)FindResource("FadeInStoryboard");
            fadeInStoryboard.Begin();
        }
    }
}