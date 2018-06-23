using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
namespace KreyGasm
{
    public static class ImageHelper
    {
        public static Image ResizeImg(this Image b, int nWidth, int nHeight)
        {
            Image result = new Bitmap(nWidth, nHeight);
            using (Graphics g = Graphics.FromImage((Image)result))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(b, 0, 0, nWidth, nHeight);
                g.Dispose();
            }
            return result;
        }
        public static int[,] ToByte(this Image myImage)
        {
            var bmp = new Bitmap(myImage);
            int[,] mass = new int[28, 28];
            for (int x = 0; x < 28; x++)
            {
                for (int y = 0; y < 28; y++)
                {
                    var isWhite = (bmp.GetPixel(x, y).R >= 230 && bmp.GetPixel(x, y).B >= 230 && bmp.GetPixel(x, y).G >= 230);
                    mass[x, y] = isWhite ? 0 : 1;
                }
            }
            return mass;
        }
    }
}
