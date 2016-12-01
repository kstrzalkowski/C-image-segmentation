using System.Drawing;
using System.Drawing.Imaging;


namespace Progowanie
{
    class ImageSegmentation
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
        public ImageSegmentation(Bitmap image)
        {
            this.image = image;  
        }
        unsafe public void Proguj(byte progR, byte progG, byte progB)
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

                    //byte pixelR = row[rIndex];
                    //byte pixelG = row[gIndex];
                    //byte pixelB = row[bIndex];

                    if (row[rIndex] > progR) row[rIndex] = 0xff;
                    else row[rIndex] = 0;

                    if (row[gIndex] > progG) row[gIndex] = 0xff;
                    else row[gIndex] = 0;

                    if (row[bIndex] > progB) row[bIndex] = 0xff;
                    else row[bIndex] = 0;
                }
            }

            image.UnlockBits(imageData);
        }
    }
}
