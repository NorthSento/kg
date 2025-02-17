using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab6
{
    public partial class Form1 : Form
    {
        private int depth = 10; 
        private const double CRe = -0.1194;
        private const double CIm = 0.6289; 

        public Form1()
        {
            InitializeComponent();
            this.ClientSize = new Size(800, 800);
            this.Text = "Julia Set Fractal";
            this.BackColor = Color.White;
            this.MouseClick += Form1_MouseClick;
            this.Paint += Form1_Paint;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                depth+=10;
                this.Invalidate(); 
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawJuliaSet(e.Graphics);
        }

        private void DrawJuliaSet(Graphics g)
        {
            int width = this.ClientSize.Width;
            int height = this.ClientSize.Height;

            using (Brush brush = new SolidBrush(Color.Black))
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        double zx = 2.0 * (x - width / 4.0) / (width)-0.2;
                        double zy = 0.5 * (-y + height / 1.0) / (height)-0.2; //надо поменять параметры(1.0;2.0)на какие то другие чтобы уменьшить маштаб
                        int iteration = 0;
                        //const int maxIteration = 10;

                        while (zx * zx + zy * zy < 4 && iteration < depth )
                        {
                            double temp = zx * zx - zy * zy + CRe;
                            zy = 2*zx * zy + CIm;                 // или здесь
                            zx = temp;
                            iteration++;
                        }

                        if (iteration >= depth )
                        {
                            g.FillRectangle(brush, x, y, 1, 1); 
                        }
                    }
                }
            }
        }
    }
}
