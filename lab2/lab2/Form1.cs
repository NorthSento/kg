using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab2
{
    public partial class Form1 : Form
    {
        private Point startPoint; 
        private Point endPoint;  
        private bool isDrawing;    
        private Bitmap bmp;        
        private Rectangle rect;    
        private bool shapeDrawn;  

        public Form1()
        {
            InitializeComponent();
            this.Width = 800;
            this.Height = 600;
            bmp = new Bitmap(this.Width, this.Height);
            this.Paint += new PaintEventHandler(this.OnPaint);
            this.MouseDown += new MouseEventHandler(this.OnMouseDown);
            this.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.MouseMove += new MouseEventHandler(this.OnMouseMove);
        }

        private void DrawPixel(int x, int y, Color color)
        {
            if (x >= 0 && x < bmp.Width && y >= 0 && y < bmp.Height)
            {
                bmp.SetPixel(x, y, color);
            }
        }

        private void DrawEllipse(int xc, int yc, int rx, int ry)
        {
            int x = 0;
            int y = ry;
            int rxSq = rx * rx;
            int rySq = ry * ry;
            int twoRxSq = 2 * rxSq;
            int twoRySq = 2 * rySq;
            int p;
            int px = 0;
            int py = twoRxSq * y;

            p = (4*rySq - (4*rxSq * ry) + ( rxSq));
            while (px < py)
            {
                DrawSymmetricPixels(xc, yc, x, y, Color.Black);
                x++;
                px += twoRySq;
                if (p < 0)
                {
                    p += 4*rySq + 4*px;
                }
                else
                {
                    y--;
                    py -= twoRxSq;
                    p += 4 * rySq + 4 * px - 4*py;
                }
            }
            
           

            p = (rySq * (2 * x + 1) * (2 * x + 1) + rxSq * (2*y - 2) * (2*y - 2) - 4*rxSq * rySq);
            while (y >= 0)
            {
                DrawSymmetricPixels(xc, yc, x, y, Color.Black);
                y--;
                py -= twoRxSq;
                if (p > 0)
                {
                    p += 4*rxSq - 4*py;
                }
                else
                {
                    x++;
                    px += twoRySq;
                    p += 4 * rxSq - 4 * py + 4*px;
                }
            }

            
            
        }
    
        

        private void DrawSymmetricPixels(int xc, int yc, int x, int y, Color color)
        {
            DrawPixel(xc + x, yc + y, color);
            DrawPixel(xc - x, yc + y, color);
            DrawPixel(xc + x, yc - y, color);
            DrawPixel(xc - x, yc - y, color);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
                isDrawing = true;
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                endPoint = e.Location;
                isDrawing = false;

               
                int xc = (startPoint.X + endPoint.X) / 2;
                int yc = (startPoint.Y + endPoint.Y) / 2;
                int rx = Math.Abs(endPoint.X - startPoint.X) / 2;
                int ry = Math.Abs(endPoint.Y - startPoint.Y) / 2;

                
                rect = GetRectangle(startPoint, endPoint);
                shapeDrawn = true;

              
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    Pen pen = new Pen(Color.Gray, 1);
                    g.DrawRectangle(pen, rect);  
                }
                DrawEllipse(xc, yc, rx, ry); 

                this.Invalidate(); 
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                endPoint = e.Location;
                this.Invalidate();
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);  

           
            if (isDrawing)
            {
                Pen pen = new Pen(Color.Gray, 1);
                e.Graphics.DrawRectangle(pen, GetRectangle(startPoint, endPoint));
            }
        }

        private Rectangle GetRectangle(Point p1, Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p1.X - p2.X),
                Math.Abs(p1.Y - p2.Y)
            );
        }
    }
}
