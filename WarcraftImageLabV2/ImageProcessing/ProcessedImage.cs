using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarcraftImageLabV2.ImageProcessing
{
    internal class ProcessedImage
    {
        public Bitmap Image { get; }
        public string FileName { get; }

        public ProcessedImage(Bitmap image, string fileName)
        {
            Image = image;
            FileName = fileName;
        }
    }
}
