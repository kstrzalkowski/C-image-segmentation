using System.Drawing;
using System.Drawing.Imaging;

namespace Progowanie
{
    class Crossfade
    {
        private Bitmap image;

        public Bitmap Image
        {
            get
            {
                return image;
            }
        }

        unsafe public void miksuj(Bitmap image1, Bitmap image2, byte alpha)
        {
            Rectangle imageRect = new Rectangle(0, 0, image1.Width, image1.Height);
            BitmapData imageData1 = image1.LockBits(imageRect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData imageData2 = image2.LockBits(imageRect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            int bytesPerPixel = 3;

            byte* scan1 = (byte*)imageData1.Scan0.ToPointer();
            byte* scan2 = (byte*)imageData2.Scan0.ToPointer();

            int stride = imageData1.Stride;

            float alphaDst = (float)alpha / 100.0F;
            float alphaSrc = 1.0F - alphaDst;

            for (int y = 0; y < imageRect.Height; y++)
            {
                byte* row1 = scan1 + (y * stride);
                byte* row2 = scan2 + (y * stride);

                for (int x = 0; x < imageRect.Width; x++)
                {
                    int bIndex = x * bytesPerPixel;
                    int gIndex = bIndex + 1;
                    int rIndex = bIndex + 2;

                    row1[bIndex] = (byte)(alphaSrc * (float)row1[bIndex] + alphaDst * (float)row2[bIndex]);
                    row1[gIndex] = (byte)(alphaSrc * (float)row1[gIndex] + alphaDst * (float)row2[gIndex]);
                    row1[rIndex] = (byte)(alphaSrc * (float)row1[rIndex] + alphaDst * (float)row2[rIndex]);

                }
            }

            image1.UnlockBits(imageData1);
            image = image1;
        }

    }
}
