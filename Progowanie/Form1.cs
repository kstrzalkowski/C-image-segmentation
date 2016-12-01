using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Diagnostics;

using AForge.Math;


namespace Progowanie
{
    public partial class Form1 : Form
    {
        private Bitmap imageOriginal = null;
        private Bitmap imageSegmentation = null;
        private Bitmap imageCrossfade = null;

        bool loadedPicture = false;

        private int countExec = 0;

        private string path;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog1.Filter = "JPG files (*.jpg)|*.jpg|BMP files (*.bmp)|*.bmp|PNG files (*.png)|*.png|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imageOriginal = new Bitmap(openFileDialog1.FileName);
                path = openFileDialog1.FileName;
                pictureBox1.Image = imageOriginal;
                pictureBox2.Image = imageOriginal;
                loadedPicture = true;
                enableControls();
                toolStripStatusLabel2.Text = String.Format("Rozmiar w pikselach: {0} x {1}", imageOriginal.Width, imageOriginal.Height);
                statusStrip1.Refresh();
               
            }
        }



        


 
        private void button6_Click(object sender, EventArgs e)
        {
            imageOriginal = new Bitmap(path);
            pictureBox1.Image = imageOriginal;
            pictureBox2.Image = imageOriginal;

            imageSegmentation = null;
            imageCrossfade = null;

            checkBox3.Checked = false;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = "Red " + trackBar1.Value.ToString();

            if (checkBox2.Checked)
            {
                trackBar3.Value = trackBar2.Value = trackBar1.Value;
            }

            if (ActiveControl == trackBar1)
            {
                prog();
            }

        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {

            label2.Text = "Green " + trackBar2.Value.ToString();

            if (checkBox2.Checked)
            {
                trackBar3.Value = trackBar1.Value = trackBar2.Value;
            }

            if (ActiveControl == trackBar2)
            {
                prog();
            }
        }


        private void prog()
        {
            byte progR = Convert.ToByte(trackBar1.Value);
            byte progG = Convert.ToByte(trackBar2.Value);
            byte progB = Convert.ToByte(trackBar3.Value);

            ImageSegmentation imageSeg = new ImageSegmentation(new Bitmap(imageOriginal));
            imageSeg.Proguj(progR, progG, progB);

            imageSegmentation = imageSeg.Image;
            pictureBox2.Image = imageSegmentation;
        }
        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            label3.Text = "Blue " + trackBar3.Value.ToString();

            if (checkBox2.Checked)
            {
                trackBar2.Value = trackBar1.Value = trackBar3.Value;
            }

            if (ActiveControl == trackBar3)
            {
                prog();
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            imageOriginal = new Bitmap(path);
            pictureBox1.Image = imageOriginal;
            pictureBox2.Image = imageOriginal;

            if (checkBox3.Checked)
            {
                GrayScale grayScale = new GrayScale(imageOriginal);
                grayScale.SetGrayScale(true);
            }

        }

      

        private void button2_Click_1(object sender, EventArgs e)
        {

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "JPG files (*.jpg)|*.jpg|BMP files (*.bmp)|*.bmp|PNG files (*.png)|*.png";
            sfd.FileName = "Picture1";

            ImageFormat format = ImageFormat.Png;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(sfd.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                   case ".png":
                        format = ImageFormat.Png;
                        break;
                }
                pictureBox2.Image.Save(sfd.FileName, format);
            }


        }

        private void enableControls()
        {
            if (!loadedPicture)
            {
                button2.Enabled = false;
                button4.Enabled = false;
                button6.Enabled = false;

                trackBar1.Enabled = false;
                trackBar2.Enabled = false;
                trackBar3.Enabled = false;
                trackBar4.Enabled = false;

                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
                button4.Enabled = true;
                button6.Enabled = true;

                trackBar1.Enabled = true;
                trackBar2.Enabled = true;
                trackBar3.Enabled = true;
                trackBar4.Enabled = true;

                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            enableControls();
        }


        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {

            if (imageSegmentation == null) return;

            

            byte value = (byte)trackBar4.Value;
            //float percentage = (float)value / 255.0f * 100.0f;
            label4.Text = "Miksowanie: " + value.ToString() + "%";

            Crossfade crossfade = new Crossfade();
            crossfade.miksuj(new Bitmap(imageOriginal), new Bitmap(imageSegmentation), value);

            imageCrossfade = crossfade.Image;
            pictureBox2.Image = imageCrossfade;

            countExec++;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                pictureBox2.SizeMode = PictureBoxSizeMode.Normal;
            }
                
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Histogram hist = new Histogram((Bitmap)pictureBox2.Image);
            

            FormHistogram formHist = new FormHistogram();
            hist.getIlluminateStatics(formHist.pictureBox1);
            hist.getRedStatics(formHist.pictureBox2);
            hist.getGreenStatics(formHist.pictureBox3);
            hist.getBlueStatics(formHist.pictureBox4);

            formHist.Show(this);
        }

        private void trackBar4_MouseUp(object sender, MouseEventArgs e)
        {
            
            toolStripStatusLabel3.Text = String.Format("Ilość wywołań: {0}", countExec);
            statusStrip1.Refresh();
            countExec = 0;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }


    }
}
