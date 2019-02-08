using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace filtrV2
{
    public partial class Form1 : Form
    {
        Bitmap MyImg;
        Bitmap OrImg;

        public Form1()
        {
            InitializeComponent();
        }

        private void открытьToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files | *.png; *.jpg; *.bmp | All Files (*.*) | *.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                MyImg = new Bitmap(dialog.FileName);
                OrImg = new Bitmap(dialog.FileName);
                pictureBox1.Image = MyImg;
                pictureBox1.Refresh();
            }
            else
            {
                return;
            }
        }

        private void сохранитьToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void negativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MyImg.Width; i++)
                for (int j = 0; j < MyImg.Height; j++)
                {
                    Color ReversedColor = MyImg.GetPixel(i, j);
                    ReversedColor = Color.FromArgb(255 - ReversedColor.R, 255 - ReversedColor.G, 255 - ReversedColor.B);
                    MyImg.SetPixel(i, j, ReversedColor);
                }
            pictureBox1.Refresh();
        }

        private void originalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyImg = OrImg;
            pictureBox1.Image = MyImg;
            pictureBox1.Refresh();
        }

        private void smurfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MyImg.Width; i++)
                for (int j = 0; j < MyImg.Height; j++)
                {
                    Color ReversedColor = MyImg.GetPixel(i, j);
                    ReversedColor = Color.FromArgb(ReversedColor.B, ReversedColor.G, ReversedColor.R);
                    MyImg.SetPixel(i, j, ReversedColor);
                }
            pictureBox1.Refresh();
        }

        private void posterizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MyImg.Width; i++)
                for (int j = 0; j < MyImg.Height; j++)
                {
                    Color ReversedColor = MyImg.GetPixel(i, j);
                    const int val = 6;
                    ReversedColor = Color.FromArgb(((ReversedColor.R >> val) << val) + 32, ((ReversedColor.G >> val) << val) + 32, ((ReversedColor.B >> val) << val) + 32);
                    MyImg.SetPixel(i, j, ReversedColor);
                }
            pictureBox1.Refresh();
        }

        private void blackwhiteOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MyImg.Width; i++)
                for (int j = 0; j < MyImg.Height; j++)
                {
                    Color ReversedColor = MyImg.GetPixel(i, j);
                    if (ReversedColor.B + ReversedColor.G + ReversedColor.R > 325)
                        ReversedColor = Color.FromArgb(255, 255, 255);
                    else
                        ReversedColor = Color.FromArgb(0, 0, 0);
                    MyImg.SetPixel(i, j, ReversedColor);
                }
            pictureBox1.Refresh();
        }

        private void blackwhiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MyImg.Width; i++)
                for (int j = 0; j < MyImg.Height; j++)
                {
                    Color ReversedColor = MyImg.GetPixel(i, j);
                    int col = (ReversedColor.B + ReversedColor.G + ReversedColor.R) / 3;
                    ReversedColor = Color.FromArgb(col, col, col);
                    MyImg.SetPixel(i, j, ReversedColor);
                }
            pictureBox1.Refresh();
        }

        private void brightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyImg = OrImg;
            for (int i = 0; i < MyImg.Width; i++)
                for (int j = 0; j < MyImg.Height; j++)
                {
                    Color ReversedColor = MyImg.GetPixel(i, j);
                    int colR = ReversedColor.R * 2;
                    if (colR > 255)
                        colR = 255;
                    int colB = ReversedColor.B * 2;
                    if (colB > 255)
                        colB = 255;
                    int colG = ReversedColor.G * 2;
                    if (colG > 255)
                        colG = 255;
                    ReversedColor = Color.FromArgb(colR, colG, colB);
                    MyImg.SetPixel(i, j, ReversedColor);
                }
            pictureBox1.Refresh();
        }

        private void blurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MyImg.Width; i++)
                for (int j = 0; j < MyImg.Height; j++)
                {
                    BlurFilter MyBlur = new BlurFilter();
                    Color ReversedColor = MyBlur.CalculateNewClolor(MyImg, i, j);
                    MyImg.SetPixel(i, j, ReversedColor);
                }
            pictureBox1.Refresh();
        }

        private void sharpnesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MyImg.Width; i++)
                for (int j = 0; j < MyImg.Height; j++)
                {
                    SharpnessFilter MySharp = new SharpnessFilter();
                    Color ReversedColor = MySharp.CalculateNewClolor(MyImg, i, j);
                    MyImg.SetPixel(i, j, ReversedColor);
                }
            pictureBox1.Refresh();
        }

        class MatrixFilter
        {
            protected float[,] kernel = null;
            public Color CalculateNewClolor(Bitmap img, int x, int y)
            {
                int Radx = kernel.GetLength(0) / 2;
                int Rady = kernel.GetLength(1) / 2;
                float ResR = 0;
                float ResG = 0;
                float ResB = 0;
                for (int i = -Rady; i <= Rady; i++)
                    for (int j = -Radx; j <= Radx; j++)
                    {
                        int X = Math.Min(x + j, img.Width - 1);
                        X = Math.Max(X, 0);
                        int Y = Math.Min(y + i, img.Height - 1);
                        Y = Math.Max(Y, 0);
                        Color NeighborColor = img.GetPixel(X, Y);
                        ResR += NeighborColor.R * kernel[j + Radx, i + Rady];
                        ResG += NeighborColor.G * kernel[j + Radx, i + Rady];
                        ResB += NeighborColor.B * kernel[j + Radx, i + Rady];
                    }
                ResR = Math.Min(ResR, 255);
                ResR = Math.Max(ResR, 0);
                ResG = Math.Min(ResG, 255);
                ResG = Math.Max(ResG, 0);
                ResB = Math.Min(ResB, 255);
                ResB = Math.Max(ResB, 0);
                return Color.FromArgb((int)ResR, (int)ResG, (int)ResB);
            }
        }

        class BlurFilter : MatrixFilter
        {
            public BlurFilter()
            {
                int sizeX = 3;
                int sizeY = 3;
                kernel = new float[sizeX, sizeY];
                for (int i = 0; i < sizeX; i++)
                    for (int j = 0; j < sizeY; j++)
                        kernel[i, j] = 1.0f / (float)(sizeX * sizeY);
            }
        }

        class SharpnessFilter : MatrixFilter
        {
            public SharpnessFilter()
            {
                int sizeX = 3;
                int sizeY = 3;
                kernel = new float[sizeX, sizeY];
                for (int i = 0; i < sizeX; i++)
                    for (int j = 0; j < sizeY; j++)
                        kernel[i, j] = -1.0f / 2.0f;
                kernel[1, 1] = 1.0f / 2.0f * sizeX * sizeY + 1.0f / 2.0f;
            }
        }
    }
}
