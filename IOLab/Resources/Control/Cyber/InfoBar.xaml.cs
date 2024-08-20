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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IOLab.Resources.Control.Cyber
{
    /// <summary>
    /// Attention.xaml 的交互逻辑
    /// </summary>
    public partial class InfoBar : UserControl
    {
        public InfoBar()
        {
            InitializeComponent();

        }
        private DispatcherTimer warningBarTimer;
        /// <summary>
        /// 动画等待计时器
        /// </summary>
        /// <param name="method">下一个动画委托</param>
        /// <param name="time">等待时间</param>
        private void InitializeTimer(Action method, double time)
        {
            warningBarTimer = new DispatcherTimer();
            warningBarTimer.Interval = TimeSpan.FromSeconds(time);
            warningBarTimer.Tick += (sender, e) =>
            {
                warningBarTimer.Stop();
                method.Invoke();
            };
        }


        /// <summary>
        /// 显示警告条
        /// </summary>
        public void ShowWaringBar(string text)
        {

            this.Dispatcher.Invoke(new Action(() => {
                WarningBarText.Text = text;
                this.Visibility = Visibility.Visible;
            }));
            InitializeTimer(ScrollWarningBar, 0.1);
            //闪烁动画
            DoubleAnimation IconAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.1),
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior(2)
            };
            //等待一会
            IconAnimation.Completed += (sender, e) =>
            {
                warningBarTimer.Start();
            };
            WarningBarContainer.BeginAnimation(UIElement.OpacityProperty, IconAnimation);

        }

        /// <summary>
        /// 放警告条
        /// </summary>
        private void ScrollWarningBar()
        {
            InitializeTimer(ReverseWarningBar, 5);
            //展开动画
            DoubleAnimation WarningBarAnimation = new DoubleAnimation();
            WarningBarAnimation.From = WarningBarContainer.ActualWidth;
            WarningBarAnimation.To = 445;
            WarningBarAnimation.Duration = TimeSpan.FromSeconds(0.2);
            WarningBarAnimation.Completed += (sender, e) =>
            {
                warningBarTimer.Start();
            };
            WarningBarContainer.BeginAnimation(Button.WidthProperty, WarningBarAnimation);


        }

        /// <summary>
        /// 收警告条
        /// </summary>
        private void ReverseWarningBar()
        {
            //收起时变换布局
            this.Dispatcher.Invoke(new Action(() =>
            {
                WarningBarImage.HorizontalAlignment = HorizontalAlignment.Right;
            }));
            DoubleAnimation WarningBarAnimation = new DoubleAnimation();
            WarningBarAnimation.From = WarningBarContainer.ActualWidth;
            WarningBarAnimation.To = 12;
            WarningBarAnimation.Duration = TimeSpan.FromSeconds(0.2);
            WarningBarContainer.BeginAnimation(Button.WidthProperty, WarningBarAnimation);
            //回滚动画
            InitializeTimer(
                new Action(() =>
                {
                    //闪动两下
                    DoubleAnimation BlinkAnimation = new DoubleAnimation
                    {
                        From = 1.0,
                        To = 0.0,
                        Duration = TimeSpan.FromSeconds(0.1),
                        AutoReverse = true,
                        RepeatBehavior = new RepeatBehavior(2)
                    };
                    //完成后隐藏
                    BlinkAnimation.Completed += (sender, e) =>
                    {
                        this.Dispatcher.Invoke(new Action(() => {
                           
                            this.Visibility = Visibility.Hidden;
                            ForceStop();
                        }));
                    };
                    //应用
                    WarningBarContainer.BeginAnimation(UIElement.OpacityProperty, BlinkAnimation);
                }),
                0.5
            );
            warningBarTimer.Start();
        }

        /// <summary>
        /// 强行停止所有动画并回到初始状态
        /// </summary>
        public void ForceStop()
        {
            this.Dispatcher.Invoke(new Action(() => {
                this.Visibility = Visibility.Hidden;
                if(warningBarTimer != null)
                {
                    warningBarTimer.Stop();

                }
                WarningBarContainer.BeginAnimation(Button.WidthProperty, null); // 终止动画

                WarningBarContainer.Width = 40;
                
                WarningBarImage.HorizontalAlignment = HorizontalAlignment.Left;
                WarningBarContainer.HorizontalAlignment = HorizontalAlignment.Center;

            }));
        }
    }
}
