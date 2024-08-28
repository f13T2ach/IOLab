
using System.Windows.Input;
using System.Windows;
using Windows.Security.Cryptography.Certificates;
using System.Windows.Controls;

namespace IOLab.Windows
{
    public sealed partial class ExperimentPractise : UserControl
    {

        private bool isDragging = false;
        private Point clickPosition;

        public ExperimentPractise()
        {
            this.InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDragging = true;
                clickPosition = e.GetPosition(this);
                this.CaptureMouse();
            }
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var currentPosition = e.GetPosition(this.Parent as UIElement);
                double offsetX = currentPosition.X - clickPosition.X;
                double offsetY = currentPosition.Y - clickPosition.Y;

                // Update position of the parent element
                var parentCanvas = this.Parent as Canvas;
                if (parentCanvas != null)
                {
                    double newLeft = Canvas.GetLeft(this) + offsetX;
                    double newTop = Canvas.GetTop(this) + offsetY;
                    Canvas.SetLeft(this, newLeft);
                    Canvas.SetTop(this, newTop);
                }

                clickPosition = currentPosition;
            }
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                isDragging = false;
                this.ReleaseMouseCapture();
            }
        }
    }
}
