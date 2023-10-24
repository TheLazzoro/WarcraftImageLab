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
using WarcraftImageLabV2.ImageProcessing;

namespace WarcraftImageLabV2.Preview
{
    public partial class PreviewControl : UserControl
    {
        public PreviewControl()
        {
            InitializeComponent();
        }

        private Point GetImageCoordsAt(MouseButtonEventArgs e)
        {
            if (image != null && image.IsMouseOver)
            {
                var controlSpacePosition = e.GetPosition(image);
                var imageControl = image;
                if (imageControl != null && imageControl.Source != null)
                {
                    // Convert from control space to image space
                    var x = Math.Floor(controlSpacePosition.X * (imageControl.Source.Width / imageControl.ActualWidth));
                    var y = Math.Floor(controlSpacePosition.Y * (imageControl.Source.Height / imageControl.ActualHeight));

                    return new Point(x, y);
                }
            }
            return new Point(-1, -1);
        }

        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = GetImageCoordsAt(e);
            var bitmap = BitmapConverter.BitmapSourceToBitmap((BitmapSource)image.Source);

            int X = (int)point.X;
            int Y = (int)point.Y;
            var pixel = bitmap.GetPixel(X, Y);
            Color color = new Color
            {
                R = pixel.R,
                G = pixel.G,
                B = pixel.B,
                A = pixel.A,
            };
            rectColor.Fill = new SolidColorBrush(color);
        }
    }
}
