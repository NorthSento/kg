using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace lab3
{
    public partial class Form1 : Form
    {
        private List<Point> points = new List<Point>(); 
        private Bitmap bmp;
        private bool isDrawingRectangle;
        private Point rectStartPoint;
        private Point rectEndPoint;
        private Rectangle clipRect;
        private bool hasRectangle;

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

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                AddPolylinePoint(e.Location);
            }
            else if (e.Button == MouseButtons.Right)
            {
                rectStartPoint = e.Location;
                isDrawingRectangle = true;

                if (hasRectangle && points.Count > 1)
                {
                    ClearRectangleAndClippedLines();
                }
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                
                    rectEndPoint = e.Location;
                    isDrawingRectangle = false;

                    hasRectangle = true;
                    clipRect = GetRectangle(rectStartPoint, rectEndPoint);

                    ClipPolylineWithRectangle();
                    this.Invalidate();
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawingRectangle)
            {
                rectEndPoint = e.Location;
                this.Invalidate();
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);

            if (isDrawingRectangle)
            {
                Pen pen = new Pen(Color.Black, 1);
                e.Graphics.DrawRectangle(pen, GetRectangle(rectStartPoint, rectEndPoint));
            }
            else if (hasRectangle)
            {
                Pen pen = new Pen(Color.Black, 1);
                e.Graphics.DrawRectangle(pen, clipRect);
            }
        }

        private void AddPolylinePoint(Point point)
        {
            if (points.Count > 0)
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    Pen redPen = new Pen(Color.Red, 1);
                    g.DrawLine(redPen, points[points.Count - 1], point);
                }
            }
            points.Add(point);
            this.Invalidate();
        }

        private void ClearRectangleAndClippedLines()
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                Pen redPen = new Pen(Color.Red, 1);

                if (points.Count > 1)
                {
                    for (int i = 1; i < points.Count; i++)
                    {
                        g.DrawLine(redPen, points[i - 1], points[i]);
                    }
                }
            }
            hasRectangle = false;
        }

        private void ClipPolylineWithRectangle()
        {
            if (points.Count < 2)
            {
                Console.WriteLine("Ошибка: Недостаточно точек для обрезки.");
                return;
            }

            
          
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    
                    g.Clear(Color.White);

                    Pen bluePen = new Pen(Color.Blue, 1);

                    for (int i = 1; i < points.Count; i++)
                    {
                        ClipLine(points[i - 1], points[i], bluePen, g);
                    }
                }

                this.Invalidate();
            
            
        }

        private void ClipLine(Point p1, Point p2, Pen pen, Graphics g)
        {
            
                int xMin = clipRect.Left;
                int xMax = clipRect.Right;
                int yMin = clipRect.Top;
                int yMax = clipRect.Bottom;

                Console.WriteLine($"Обрезаем линию {p1} -> {p2} в прямоугольнике {clipRect}");

                int code1 = ComputeOutCode(p1, xMin, xMax, yMin, yMax);
                int code2 = ComputeOutCode(p2, xMin, xMax, yMin, yMax);
                bool accept = false;

                while (true)
                {
                    if ((code1 | code2) == 0)
                    {
                        accept = true;
                        break;
                    }
                    else if ((code1 & code2) != 0)
                    {
                        break;
                    }
                    else
                    {
                        int codeOut = code1 != 0 ? code1 : code2;
                        int x = 0, y = 0;

                        if ((codeOut & 8) != 0)
                        {
                            if (p2.Y == p1.Y)
                            {
                                Console.WriteLine("Ошибка: деление на ноль!");
                                return;
                            }
                            x = p1.X + (p2.X - p1.X) * (yMax - p1.Y) / (p2.Y - p1.Y);
                            y = yMax;
                        }
                        else if ((codeOut & 4) != 0)
                        {
                            if (p2.Y == p1.Y)
                            {
                                Console.WriteLine("Ошибка: деление на ноль!");
                                return;
                            }
                            x = p1.X + (p2.X - p1.X) * (yMin - p1.Y) / (p2.Y - p1.Y);
                            y = yMin;
                        }
                        else if ((codeOut & 2) != 0)
                        {
                            if (p2.X == p1.X)
                            {
                                Console.WriteLine("Ошибка: деление на ноль!");
                                return;
                            }
                            y = p1.Y + (p2.Y - p1.Y) * (xMax - p1.X) / (p2.X - p1.X);
                            x = xMax;
                        }
                        else if ((codeOut & 1) != 0)
                        {
                            if (p2.X == p1.X)
                            {
                                Console.WriteLine("Ошибка: деление на ноль!");
                                return;
                            }
                            y = p1.Y + (p2.Y - p1.Y) * (xMin - p1.X) / (p2.X - p1.X);
                            x = xMin;
                        }

                        if (codeOut == code1)
                        {
                            p1 = new Point(x, y);
                            code1 = ComputeOutCode(p1, xMin, xMax, yMin, yMax);
                        }
                        else
                        {
                            p2 = new Point(x, y);
                            code2 = ComputeOutCode(p2, xMin, xMax, yMin, yMax);
                        }
                    }
                }

                if (accept)
                {
                    g.DrawLine(pen, p1, p2);
                }
        }

        private int ComputeOutCode(Point p, int xMin, int xMax, int yMin, int yMax)
        {
            int code = 0;
            if (p.X < xMin) code |= 1;
            else if (p.X > xMax) code |= 2;
            if (p.Y < yMin) code |= 4;
            else if (p.Y > yMax) code |= 8;
            return code;
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
