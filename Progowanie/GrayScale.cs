using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Progowanie
{
    class GrayScale
    {
        private Bitmap image;
        public Bitmap Image
        {
            get
            {
                return image;
            }

            set
            {
                this.image = value;
            }
        }

        public GrayScale(Bitmap image)
        {
            this.image = image;
        }
        unsafe public void SetGrayScale(bool green)
        {
            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int bytesPerPixel = 3;

            byte* scan0 = (byte*)imageData.Scan0.ToPointer();

            int stride = imageData.Stride;

            for (int y = 0; y < imageData.Height; y++)
            {
                byte* row = scan0 + (y * stride);

                for (int x = 0; x < imageData.Width; x++)
                {

                    int bIndex = x * bytesPerPixel;
                    int gIndex = bIndex + 1;
                    int rIndex = bIndex + 2;

                    byte pixelR = row[rIndex];
                    byte pixelG = row[gIndex];
                    byte pixelB = row[bIndex];

                    if (green)
                    {
                        row[rIndex] = Convert.ToByte((0.299 * pixelR + 0.587 * pixelG + 0.114 * pixelB));
                        row[gIndex] = Convert.ToByte((0.299 * pixelR + 0.587 * pixelG + 0.114 * pixelB));
                        row[bIndex] = Convert.ToByte((0.299 * pixelR + 0.587 * pixelG + 0.114 * pixelB));
                    }
                    else
                    {
                        row[rIndex] = Convert.ToByte((pixelR + pixelG + pixelB) / 3);
                        row[gIndex] = Convert.ToByte((pixelR + pixelG + pixelB) / 3);
                        row[bIndex] = Convert.ToByte((pixelR + pixelG + pixelB) / 3);
                    }
                }
            }

            image.UnlockBits(imageData);
            //pictureBox1.Image = image;
            //pictureBox1.Refresh();

        }
        
    }
}
