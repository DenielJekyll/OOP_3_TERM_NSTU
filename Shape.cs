using System;
using System.Drawing;

namespace Shapes
{

    public class Shape
    {
        private double _scale;
        private Point _translate;
        public double R { get; set; }
        public Color color { set; get; }
        public Point center { set; get; }
        public double angle { set; get; }
        public bool active { set; get; }
        public int move_speed { set; get; }
        public int rotating_speed { set; get; }
        public uint index { set; get; }
        public Point[] static_points { set; get; }
        public Point translate { get { return _translate; } }
        public double scale { set { _scale += value; } get { return _scale; } }

        public void toRotate(double angle)
        {
            this.angle += angle * Math.PI / 10 * rotating_speed;
        }

        public void setTranslate(int x, int y)
        {
            _translate.X += x;
            _translate.Y += y;
        }
    }

    public class Parallelogram : Shape
    {
        private Point _translate;
        public int type { get; }

        public Parallelogram(Point[] new_points, Point c, double R, uint index)
        {
            static_points = new Point[4];
            static_points[0].X = new_points[0].X;
            static_points[0].Y = new_points[0].Y;

            static_points[1].X = new_points[1].X;
            static_points[1].Y = new_points[1].Y;

            static_points[2].X = new_points[2].X;
            static_points[2].Y = new_points[2].Y;

            static_points[3].X = new_points[3].X;
            static_points[3].Y = new_points[3].Y;



            angle = 0;
            type = 14;
            scale = 1;
            center = c;
            this.R = R;
            active = false;
            move_speed = 2;
            this.index = index;
            rotating_speed = 1;
            _translate = new Point(0, 0);
            Random rand = new Random();
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }
    }

    public class Pentagon : Shape
    {
        private Point _translate;
        public int type { get; }

        public Pentagon(Point[] new_points, Point center, double R, uint index)
        {
            static_points = new Point[5];
            static_points[0].X = new_points[0].X;
            static_points[0].Y = new_points[0].Y;

            static_points[1].X = new_points[1].X;
            static_points[1].Y = new_points[1].Y;

            static_points[2].X = new_points[2].X;
            static_points[2].Y = new_points[2].Y;

            static_points[3].X = new_points[3].X;
            static_points[3].Y = new_points[3].Y;

            static_points[4].X = new_points[4].X;
            static_points[4].Y = new_points[4].Y;


            type = 8;
            angle = 0;
            scale = 1;
            this.R = R;
            active = false;
            move_speed = 2;
            this.index = index;
            rotating_speed = 1;
            _translate = new Point(0, 0);
            this.center = center;
            Random rand = new Random();
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }
    }

    public class Rhombus : Shape
    {
        private Point _translate;
        public int type { get; }

        public Rhombus(Point[] new_points, Point c, double R, uint index)
        {
            static_points = new Point[4];
            static_points[0].X = new_points[0].X;
            static_points[0].Y = new_points[0].Y;

            static_points[1].X = new_points[1].X;
            static_points[1].Y = new_points[1].Y;

            static_points[2].X = new_points[2].X;
            static_points[2].Y = new_points[2].Y;

            static_points[3].X = new_points[3].X;
            static_points[3].Y = new_points[3].Y;


            type = 4;
            angle = 0;
            scale = 1;
            center = c;
            this.R = R;
            active = false;
            move_speed = 2;
            this.index = index;
            rotating_speed = 1;
            _translate = new Point(0, 0);
            Random rand = new Random();
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }
    }

    public class Ellipse : Shape
    {
        public int type { get; }
        public Ellipse()
        {


        }

        public Ellipse(Point[] new_points, Point c)
        {

        }
    }

}
