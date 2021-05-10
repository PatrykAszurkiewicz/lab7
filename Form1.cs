using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        Bitmap bitmap3;
        Bitmap bitmap2;
        Bitmap bitmapGauss;
        Bitmap bitmapCopy2;

        double suma;
        double[,] gauss;

        int width;
        int height;
        int widthG;
        int heightG;

        int[] histr = new int[256];
        int[] histg = new int[256];
        int[] histb = new int[256];
        int[] hist = new int[256];
        int[] histw = new int[256];
        int[] histwr = new int[256];
        int[] histwg = new int[256];
        int[] histwb = new int[256];
        int[] histkum = new int[256];
        int[] histkumr = new int[256];
        int[] histkumg = new int[256];
        int[] histkumb = new int[256];
        bool readed = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                bitmap = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = bitmap;
                width = pictureBox1.Image.Width;
                height = pictureBox1.Image.Height;
                Array.Clear(histr, 0, histr.Length);
                Array.Clear(histg, 0, histg.Length);
                Array.Clear(histb, 0, histb.Length);
                Array.Clear(hist, 0, hist.Length);
                Array.Clear(histw, 0, histw.Length);
                Array.Clear(histwr, 0, histwr.Length);
                Array.Clear(histwg, 0, histwg.Length);
                Array.Clear(histwb, 0, histwb.Length);
                Array.Clear(histkum, 0, histkum.Length);
                Array.Clear(histkumr, 0, histkumr.Length);
                Array.Clear(histkumg, 0, histkumg.Length);
                Array.Clear(histkumb, 0, histkumb.Length);
                histogram();
                histogram2();
                readed = true;
                panel1.Invalidate();
                panel2.Invalidate();
                panel3.Invalidate();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                bitmap2 = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName);
                widthG = bitmap2.Width;
                heightG = bitmap2.Height;
                pictureBox3.Image = bitmap2;
                pictureBox5.Image = bitmap2;
                filtr();
            }
        }
        private void histogram()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color p = bitmap.GetPixel(x, y);

                    int r = p.R;
                    int g = p.G;
                    int b = p.B;
                    histr[r]++;
                    histg[g]++;
                    histb[b]++;

                }
            }
            for (int i = 0; i < 256; i++)
            {
                hist[i] = histr[i] + histg[i] + histb[i];
            }
            histogramKum();
        }

        private void histogramKum()
        {
            histkum[0] = hist[0];
            for (int i = 1; i < hist.Length; i++)
            {
                histkum[i] = hist[i] + histkum[i - 1];
            }
            histkumr[0] = histr[0];
            for (int i = 1; i < histr.Length; i++)
            {
                histkumr[i] = histr[i] + histkumr[i - 1];
            }
            histkumg[0] = histg[0];
            for (int i = 1; i < histg.Length; i++)
            {
                histkumg[i] = histg[i] + histkumg[i - 1];
            }
            histkumb[0] = histb[0];
            for (int i = 1; i < histb.Length; i++)
            {
                histkumb[i] = histb[i] + histkumb[i - 1];
            }
            histogramWyr();
        }
        private void histogramWyr()
        {
            double temp = 255 / (double)(width * height);
            try
            {
                bitmap3 = (Bitmap)bitmap.Clone();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {

                        Color p = bitmap3.GetPixel(x, y);

                        int r = p.R;
                        int g = p.G;
                        int b = p.B;

                        double r1 = histkumr[r] * 255;
                        double g1 = histkumg[g] * 255;
                        double b1 = histkumb[b] * 255;
                        double r2 = r1 / (bitmap3.Width * bitmap3.Height);
                        double g2 = g1 / (bitmap3.Width * bitmap3.Height);
                        double b2 = b1 / (bitmap3.Width * bitmap3.Height);

                        if (r2 > 255)
                        {
                            r2 = 255;
                        }
                        if (g2 > 255)
                        {
                            g2 = 255;
                        }
                        if (b2 > 255)
                        {
                            b2 = 255;
                        }

                        bitmap3.SetPixel(x, y, Color.FromArgb((int)r2, (int)g2, (int)b2));
                    }
                }

                pictureBox2.Image = bitmap3;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Image not loaded!");
            }

        }

        public void histogram2()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color p = bitmap3.GetPixel(x, y);

                    int r = p.R;
                    int g = p.G;
                    int b = p.B;
                    histwr[r]++;
                    histwg[g]++;
                    histwb[b]++;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                histw[i] = histwr[i] + histwg[i] + histwb[i];
            }
            panel3.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (readed == true)
            {
                int x = 0;
                Graphics g = e.Graphics;
                for (int i = 0; i < 256; i++)
                {
                    double b = hist[i];
                    b = b / (bitmap.Width * bitmap.Height);
                    b = b * 2200;
                    g.DrawLine(new Pen(Color.Black), x, panel1.Height, x, panel1.Height - (int)b);
                    x++;
                }

            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            if (readed == true)
            {
                int x = 0;
                Graphics g = e.Graphics;
                for (int i = 0; i < 256; i++)
                {
                    double b = histkum[i];
                    b = b / (bitmap.Width * bitmap.Height);
                    b = b * 87;
                    g.DrawLine(new Pen(Color.Black), x, panel2.Height, x, panel2.Height - (int)b);
                    x++;
                }

            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            if (readed == true)
            {
                int x = 0;
                Graphics g = e.Graphics;
                for (int i = 0; i < 256; i++)
                {
                    double b = histw[i];
                    b = b / (bitmap3.Width * bitmap3.Height);
                    b = b * 2200;
                    g.DrawLine(new Pen(Color.Black), x, panel3.Height, x, panel3.Height - (int)b);
                    x++;
                }

            }
        }
        private void filtr()
        {
            double[,] M = new double[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            double pomoc_r = 0;
            double pomoc_g = 0;
            double pomoc_b = 0;
            Bitmap bitmap4;
            Bitmap bitmapCopy;
            bitmap4 = (Bitmap)bitmap2.Clone();
            bitmapCopy = (Bitmap)bitmap2.Clone();

            for (int i = 0; i < bitmap4.Height; i++)
            {
                for (int j = 0; j < bitmap4.Width; j++)
                {
                    pomoc_r = 0;
                    pomoc_g = 0;
                    pomoc_b = 0;
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            int x1 = i + k;
                            int y1 = j + l;
                            if (x1 < 0)
                            {
                                x1 = 0;
                            }
                            if (x1 >= bitmap2.Height)
                            {
                                x1 = bitmap2.Height - 1;
                            }
                            if (y1 < 0)
                            {
                                y1 = 0;
                            }
                            if (y1 >= bitmap2.Width)
                            {
                                y1 = bitmap2.Width - 1;
                            }
                            Color p = bitmapCopy.GetPixel(y1, x1);
                            /*}*/
                            double r = (double)p.R;
                            double g = (double)p.G;
                            double b = (double)p.B;
                            pomoc_r += r * M[k + 1, l + 1];
                            pomoc_g += g * M[k + 1, l + 1];
                            pomoc_b += b * M[k + 1, l + 1];
                        }
                    }

                    double r1 = pomoc_r / 9;
                    double g1 = pomoc_g / 9;
                    double b1 = pomoc_b / 9;

                    if (r1 > 255)
                    {
                        r1 = 255;
                    }
                    if (g1 > 255)
                    {
                        g1 = 255;
                    }
                    if (b1 > 255)
                    {
                        b1 = 255;
                    }

                    bitmap4.SetPixel(j, i, Color.FromArgb((int)r1, (int)g1, (int)b1));
                }
            }
            pictureBox4.Image = bitmap4;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int n = (trackBar2.Value * 2) + 1;
            int k1 = trackBar2.Value;
            int sig = trackBar1.Value;
            
            gauss = new double[(2 * k1) + 1, (2 * k1) + 1];
            suma = 0;
            for (int i = 0; i < ((2 * k1) + 1); i++)
            {
                for (int j = 0; j < ((2 * k1) + 1); j++)
                {
                    gauss[i, j] = (1 / (2 * Math.PI * Math.Pow(sig, 2))) * Math.Pow(Math.E, -(Math.Pow(i - k1, 2) + Math.Pow(j - k1, 2)) / (2 * Math.Pow(sig, 2)));
                    suma = suma + gauss[i, j];
                }
            }

            double pomoc_r = 0;
            double pomoc_g = 0;
            double pomoc_b = 0;
            bitmapGauss = (Bitmap)bitmap2.Clone();
            int x1, y1;
            Color p;
            double r, g, b;
            for (int j = 0; j < widthG; j++)
            {
                for (int i = 0; i < heightG; i++)
                {
                    pomoc_r = 0;
                    pomoc_g = 0;
                    pomoc_b = 0;
                    for (int k = -k1; k <= k1; k++)
                    {
                        for (int l = -k1; l <= k1; l++)
                        {
                            x1 = i + k;
                            y1 = j + l;
                            if (x1 < 0)
                            {
                                x1 = 0;
                            }
                            if (x1 >= heightG)
                            {
                                x1 = heightG - 1;
                            }
                            if (y1 < 0)
                            {
                                y1 = 0;
                            }
                            if (y1 >= widthG)
                            {
                                y1 = widthG - 1;
                            }
                            p = bitmap2.GetPixel(y1, x1);

                            r = (double)p.R;
                            g = (double)p.G;
                            b = (double)p.B;
                            pomoc_r += r * gauss[k + k1, l + k1];
                            pomoc_g += g * gauss[k + k1, l + k1];
                            pomoc_b += b * gauss[k + k1, l + k1];
                        }
                    }

                    pomoc_r = pomoc_r / suma;
                    pomoc_g = pomoc_g / suma;
                    pomoc_b = pomoc_b / suma;

                    if (pomoc_r > 255)
                    {
                        pomoc_r = 255;
                    }
                    if (pomoc_g > 255)
                    {
                        pomoc_g = 255;
                    }
                    if (pomoc_b > 255)
                    {
                        pomoc_b = 255;
                    }

                    bitmapGauss.SetPixel(j, i, Color.FromArgb((int)pomoc_r, (int)pomoc_g, (int)pomoc_b));
                }
            }
            pictureBox5.Image = bitmapGauss;
        }
    }
}
