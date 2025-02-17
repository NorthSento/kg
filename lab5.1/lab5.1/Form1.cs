using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab5._1
{
    public partial class Form1 : Form
    {
        private Cube cube;

        public Form1()
        {
            InitializeComponent();
            this.Width = 800;
            this.Height = 600;
            this.Text = "3D Cube Transformations";
            this.DoubleBuffered = true;

            cube = new Cube();

            this.Paint += OnPaint;
            this.KeyDown += OnKeyDown;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            cube.Draw(g, this.ClientSize.Width / 2, this.ClientSize.Height / 2);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.X: cube.RotateX(Math.PI / 18); break;
                case Keys.Y: cube.RotateY(Math.PI / 18); break;
                case Keys.Z: cube.RotateZ(Math.PI / 18); break;
                case Keys.Q: cube.Translate(0.1, 0, 0); break;
                case Keys.W: cube.Translate(0, 0.1, 0); break;
                case Keys.E: cube.Translate(0, 0, 0.1); break;
                case Keys.A: cube.Scale(1.1, 1, 1); break;
                case Keys.S: cube.Scale(1, 1.1, 1); break;
                case Keys.D: cube.Scale(1, 1, 1.1); break;
            }
            this.Invalidate();
        }

        public class Cube
        {
            private Point3D[] vertices;
            private int[,] edges;

            public Cube()
            {
                vertices = new Point3D[]
                {
                    new Point3D(-0.5, -0.5, -0.5),
                    new Point3D(0.5, -0.5, -0.5),
                    new Point3D(0.5, 0.5, -0.5),
                    new Point3D(-0.5, 0.5, -0.5),
                    new Point3D(-0.5, -0.5, 0.5),
                    new Point3D(0.5, -0.5, 0.5),
                    new Point3D(0.5, 0.5, 0.5),
                    new Point3D(-0.5, 0.5, 0.5),
                };

                edges = new int[,]
                {
                    {0, 1}, {1, 2}, {2, 3}, {3, 0},
                    {4, 5}, {5, 6}, {6, 7}, {7, 4},
                    {0, 4}, {1, 5}, {2, 6}, {3, 7}
                };
            }

            public void RotateX(double angle)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    double y = vertices[i].Y * Math.Cos(angle) - vertices[i].Z * Math.Sin(angle);
                    double z = vertices[i].Y * Math.Sin(angle) + vertices[i].Z * Math.Cos(angle);
                    vertices[i] = new Point3D(vertices[i].X, y, z);
                }
            }

            public void RotateY(double angle)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    double x = vertices[i].X * Math.Cos(angle) + vertices[i].Z * Math.Sin(angle);
                    double z = -vertices[i].X * Math.Sin(angle) + vertices[i].Z * Math.Cos(angle);
                    vertices[i] = new Point3D(x, vertices[i].Y, z);
                }
            }

            public void RotateZ(double angle)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    double x = vertices[i].X * Math.Cos(angle) - vertices[i].Y * Math.Sin(angle);
                    double y = vertices[i].X * Math.Sin(angle) + vertices[i].Y * Math.Cos(angle);
                    vertices[i] = new Point3D(x, y, vertices[i].Z);
                }
            }

            public void Translate(double dx, double dy, double dz)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = new Point3D(vertices[i].X + dx, vertices[i].Y + dy, vertices[i].Z + dz);
                }
            }

            public void Scale(double sx, double sy, double sz)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = new Point3D(vertices[i].X * sx, vertices[i].Y * sy, vertices[i].Z * sz);
                }
            }

            public void Draw(Graphics g, int centerX, int centerY)
            {
                for (int i = 0; i < edges.GetLength(0); i++)
                {
                    Point p1 = Project(vertices[edges[i, 0]], centerX, centerY);
                    Point p2 = Project(vertices[edges[i, 1]], centerX, centerY);
                    g.DrawLine(Pens.Black, p1, p2);
                }
            }

            private Point Project(Point3D point, int centerX, int centerY)
            {
                double scale = 200;
                return new Point(
                    (int)(centerX + point.X * scale),
                    (int)(centerY - point.Y * scale)
                );
            }
        }

        public struct Point3D
        {
            public double X, Y, Z;

            public Point3D(double x, double y, double z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }
    }
}
