﻿using System;
using System.Collections.Generic;
using System.Drawing;
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
using WarcraftImageLab.ImageProcessing;
using WarcraftImageLab.ImageProcessing.Read;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace WarcraftImageLab.Preview
{
    public partial class PreviewControl : UserControl
    {
        private string imageFilePath;

        public PreviewControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates the file path and loads the image.
        /// </summary>
        public void UpdateImage(string? fullPath)
        {
            imageFilePath = fullPath;
            ReloadImage();
        }

        /// <summary>
        /// Reloads the last selected image.
        /// </summary>
        public void ReloadImage()
        {
            textBlockError.Visibility = Visibility.Hidden;
            if (string.IsNullOrEmpty(imageFilePath))
                return;

            try
            {
                Bitmap bitmap = Reader.ReadImageFile(imageFilePath);
                Processor processor = new Processor(string.Empty, true);
                var list = processor.ApplyFilters(bitmap);
                var processedImage = list[0].Image;

                BitmapSource bitmapSource = BitmapConverter.ToBitmapSource(processedImage);
                image.Source = bitmapSource;

                int width = (int)Math.Round(bitmapSource.Width);
                int height = (int)Math.Round(bitmapSource.Height);
                textBlockResolution.Text = $"Resolution: {width}x{height}";
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
            }
        }

        public void SetErrorMessage(string errorMsg)
        {
            textBlockError.Visibility = Visibility.Visible;
            textBlockError.Text = "Error: " + errorMsg;
            image.Source = null;
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
            textColor.Text = $"R:{pixel.R} G:{pixel.G} B:{pixel.B} A:{pixel.A}   {color}";
        }

        private void image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var size = e.NewSize;
            rectBackground.Width = size.Width;
            rectBackground.Height = size.Height;
        }

        private void checkBoxTransparency_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)checkBoxTransparency.IsChecked)
                rectBackground.Fill = rectFillTransparency;
            else
                rectBackground.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }
    }
}
