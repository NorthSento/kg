using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form1 : Form
    {
        private List<Point> points = new List<Point>();
        private Bitmap bmp;
        private bool polygonDraw = false; 
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
            if(x>=0 && x<bmp.Width && y>=0 && y < bmp.Height)
            {
                bmp.SetPixel(x, y, color);
            }
        }
        private void DrawLine(Point p1, Point p2, Color color)
        {
            int x0 = p1.X;
            int y0 = p1.Y;
            int x1 = p2.X;
            int y1 = p2.Y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;
            while(true)
            {
                DrawPixel(x0, y0, color);
                if (x0 == x1 && y0 == y1) { break; }
                int e2 = err * 2;
                if(e2>-dy) { err -= dy; x0 += sx; }
                if (e2 < dx) { err += dx;y0 += sy; }
            }
        }
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                
                points.Add(new Point(e.X, e.Y));
                DrawPixel(e.X, e.Y, Color.Black);
                polygonDraw = false;
                this.Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (polygonDraw)
                {
                    
                    bmp = new Bitmap(this.Width, this.Height);
                    foreach (var point in points)
                    {
                        DrawPixel(point.X, point.Y, Color.Black);
                    }
                    polygonDraw = false;
                }
                else if (points.Count > 1)
                {
                    
                    for (int i = 0; i < points.Count - 1; i++)
                    {
                        DrawLine(points[i], points[i + 1], Color.Red);
                    }
                    DrawLine(points[points.Count - 1], points[0], Color.Red);
                    polygonDraw = true; 
                }
                this.Invalidate();
            }
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0,0);
        }
    }
}
