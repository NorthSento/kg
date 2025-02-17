using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab0
{
        public partial class Form1 : Form
        {
            private List<Point> points = new List<Point>();
            private Bitmap bmp;
            private Random random = new Random();

            public Form1()
            {
                InitializeComponent();
                this.Width = 800;
                this.Height = 600;
                bmp = new Bitmap(this.Width, this.Height);
                this.Paint += new PaintEventHandler(this.OnPaint);
                this.MouseDown += new MouseEventHandler(this.OnMouseDown);
            }
            private void DrawPixel(int x, int y, Color color)
            {
                if (x >= 0 && x < bmp.Width && y >= 0 && y < bmp.Height)
                {
                    bmp.SetPixel(x, y, color);

                }
            }
            private void OnMouseDown(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    points.Add(new Point(e.X, e.Y));
                    DrawPixel(e.X, e.Y, Color.Black);
                    this.Invalidate();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    foreach (var point in points)
                    {
                        Color randomcolor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                        DrawPixel(point.X,point.Y, randomcolor);
                        
                    }
                    this.Invalidate();
                }
            }
            private void OnPaint(object sender, PaintEventArgs e)
            {
                e.Graphics.DrawImage(bmp, 0, 0);
            }
        }
       
}
