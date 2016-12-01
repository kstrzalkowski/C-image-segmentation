using System;
using System.Linq;
using System.Drawing;
using AforgeImg = AForge.Imaging;
using System.Windows.Forms;

namespace Progowanie
{
    class Histogram
    {
        private Bitmap bitmap;
        private AforgeImg.ImageStatistics statistics;
        private AforgeImg.ImageStatisticsHSL hslStatistics;

        public Histogram(Bitmap b)
        {
            this.bitmap = b;
            statistics = new AforgeImg.ImageStatistics(this.bitmap);
            hslStatistics = new AforgeImg.ImageStatisticsHSL(this.bitmap);
            
        }
        private int[] SmoothHistogram(int[] originalValues)
        {
            int[] smoothedValues = new int[originalValues.Length];

            double[] mask = new double[] { 0.25, 0.5, 0.25 };

            for (int bin = 1; bin < originalValues.Length - 1; bin++)
            {
                double smoothedValue = 0;
                for (int i = 0; i < mask.Length; i++)
                {
                    smoothedValue += originalValues[bin - 1 + i] * mask[i];
                }
                smoothedValues[bin] = (int)smoothedValue;
            }

            return smoothedValues;
        }

        public void getRedStatics(PictureBox b)
        {
            int[] values =  SmoothHistogram(statistics.Red.Values);
            Bitmap bitmap = new Bitmap(b.Width, b.Height);

            int maxi = 0;
            for (int i = 0; i < values.Count(); i++)
            {
                if (values[i] > maxi)
                    maxi = values[i];
            }
            double wsp = Convert.ToDouble(b.Height) / maxi;


            Graphics g = Graphics.FromImage(bitmap);

            for (int i = 0; i < values.Count(); i++)
            {
                double y = values[i] * wsp;
                g.DrawLine(new Pen(Color.Red), new Point(i, bitmap.Height), new Point(i, bitmap.Height - Convert.ToInt32(y)));
            }


            b.Image = bitmap;
        }

        public void getGreenStatics(PictureBox b)
        {
            int[] values = SmoothHistogram(statistics.Green.Values);
            Bitmap bitmap = new Bitmap(b.Width, b.Height);

            int maxi = 0;
            for (int i = 0; i < values.Count(); i++)
            {
                if (values[i] > maxi)
                    maxi = values[i];
            }
            double wsp = Convert.ToDouble(b.Height) / maxi;


            Graphics g = Graphics.FromImage(bitmap);

            for (int i = 0; i < values.Count(); i++)
            {
                double y = values[i] * wsp;
                g.DrawLine(new Pen(Color.Green), new Point(i, bitmap.Height), new Point(i, bitmap.Height - Convert.ToInt32(y)));
            }


            b.Image = bitmap;
        }


        public void getBlueStatics(PictureBox b)
        {
            int[] values = SmoothHistogram(statistics.Blue.Values);
            Bitmap bitmap = new Bitmap(b.Width, b.Height);

            int maxi = 0;
            for (int i = 0; i < values.Count(); i++)
            {
                if (values[i] > maxi)
                    maxi = values[i];
            }
            double wsp = Convert.ToDouble(b.Height) / maxi;


            Graphics g = Graphics.FromImage(bitmap);

            for (int i = 0; i < values.Count(); i++)
            {
                double y = values[i] * wsp;
                g.DrawLine(new Pen(Color.Blue), new Point(i, bitmap.Height), new Point(i, bitmap.Height - Convert.ToInt32(y)));
            }


            b.Image = bitmap;
        }

        public void getIlluminateStatics(PictureBox b)
        {
            int[] values = SmoothHistogram(hslStatistics.Luminance.Values);
            Bitmap bitmap = new Bitmap(b.Width, b.Height);

            int maxi = 0;
            for (int i = 0; i < values.Count(); i++)
            {
                if (values[i] > maxi)
                    maxi = values[i];
            }
            double wsp = Convert.ToDouble(b.Height) / maxi;


            Graphics g = Graphics.FromImage(bitmap);

            for (int i = 0; i < values.Count(); i++)
            {
                double y = values[i] * wsp;
                g.DrawLine(new Pen(Color.Black), new Point(i, bitmap.Height), new Point(i, bitmap.Height - Convert.ToInt32(y)));
            }


            b.Image = bitmap;
        }
    }
}
