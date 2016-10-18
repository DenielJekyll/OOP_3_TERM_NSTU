using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// для работы с библиотекой OpenGL 
using Tao.OpenGl;
// для работы с библиотекой FreeGLUT 
using Tao.FreeGlut;
// для работы с элементом управления SimpleOpenGLControl 
using Tao.Platform.Windows;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        Figure example;
        private int count_clic = 0;
        private Point[] temp_points = new Point[4];
        public Form1()
        {
            InitializeComponent();
            field.InitializeContexts();
        }

        private void Draw(Figure figure)
        {
            // очистка буфера цвета и буфера глубины 
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glClearColor(0, 0, 0, 1);
            // очищение текущей матрицы 
            Gl.glLoadIdentity();

            // установка черного цвета 
            Gl.glColor3f(0, 0, 0);

            // помещаем состояние матрицы в стек матриц 
            Gl.glPushMatrix();
            Gl.glTranslated(example.getCenterX(), example.getCenterY(), 0.0f);
            Gl.glRotated(example.getAngle(), 0, 0, 1);
            Gl.glScalef((float)example.getScaleX(), (float)example.getScaleY(), 1);
            Gl.glTranslated(example.getTranslateX(), example.getTranslateY(), 0.0f);
            Gl.glTranslated(-example.getCenterX(), -example.getCenterY(), 0.0f);
            // помещаем состояние матрицы в стек матриц 
            Gl.glPushMatrix();

            Point[] points = figure.getPoints();
            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glColor3d(255, 0, 0);
            Gl.glVertex2f(points[0].X, points[0].Y);
            Gl.glVertex2f(points[1].X, points[1].Y);
            Gl.glVertex2f(points[2].X, points[2].Y);
            Gl.glVertex2f(points[3].X, points[3].Y);
            Gl.glEnd();
            

            // возвращаем состояние матрицы 
            Gl.glPopMatrix();

            // отрисовываем геометрию 
            Gl.glFlush();

            // обновляем состояние элемента 
            field.Invalidate();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            // инициализация Glut 
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_SINGLE);
            
            // очистка окна 
            Gl.glClearColor(0, 0, 0, 1);

            // установка порта вывода в соответствии с размерами элемента anT 
            Gl.glViewport(0, 0, field.Width, field.Height);


            // настройка проекции 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            // теперь необходимо корректно настроить 2D ортогональную проекцию 
            Glu.gluOrtho2D(0.0,  (float)field.Width, 0.0, (float)field.Height);
            
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
           
            Gl.glLoadIdentity();

            
        }

        public void btn_create_primitive(object sender, EventArgs e)
        {
            example = new Figure();
            RenderTime.Enabled = true;
            Draw(example);
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            Draw(example);
        }

        private void field_KeyDown(object sender, KeyEventArgs e)
        {

            // Z и X отвечают за масштабирование 
            if (e.KeyCode == Keys.Z)
            {
                // вызов функции, в которой мы реализуем масштабирование - передаем коэффициент масштабирования и выбранную ось в окне программы 
                example.setScale(-0.05f, 0.00f);
            }
            if (e.KeyCode == Keys.X)
            {
                // вызов функции, в которой мы реализуем масштабирование - передаем коэффициент масштабирования и выбранную ось в окне программы 
                example.setScale(0.05f, 0.00f);
            }

            if (e.KeyCode == Keys.C)
            {
                // вызов функции, в которой мы реализуем масштабирование - передаем коэффициент масштабирования и выбранную ось в окне программы 
                example.setScale(0.0f, -0.03f);
            }

            if (e.KeyCode == Keys.V)
            {
                // вызов функции, в которой мы реализуем масштабирование - передаем коэффициент масштабирования и выбранную ось в окне программы 
                example.setScale(0.0f, 0.03f);
            }

            // W и S отвечают за перенос 
            if (e.KeyCode == Keys.W)
            {
                example.setTranslate(0, 1);
                // вызов функции, в которой мы реализуем перенос - передаем значение перемещения и выбранную ось в окне программы 
                
            }
            if (e.KeyCode == Keys.S)
            {
                example.setTranslate(0, -1);
                // вызов функции, в которой мы реализуем перенос - передаем значение перемещения и выбранную ось в окне программы 


            } // A и D отвечают за перемещение по Х 
            if (e.KeyCode == Keys.A)
            {
                example.setTranslate(-1, 0);
                // вызов функции, в которой мы реализуем поворот - передаем значение для поворота и выбранную ось 

            }
            if (e.KeyCode == Keys.D)
            {
                example.setTranslate(1, 0);
            }
            // Е и Q отвечают за поворот
            if(e.KeyCode == Keys.E)
            {
                example.CreateRotate(1.0f);
                label3.Text = "" + example.getAngle();
            }

            if (e.KeyCode == Keys.Q)
            {
                example.CreateRotate(-1.0f);
                label3.Text = "" + example.getAngle();
                label2.Text = "" + example.getSpeed();
                example.speed = trackBar1.Value;
            }
        }

        private void field_Load(object sender, EventArgs e)
        {

        }

        private void myMouse(object sender, MouseEventArgs e)
        {
            //if (count_clic < 3)
            //{
            //    label_x_cord.Text = e.X + "";
            //    label_y_cord.Text = field.Height - e.Y + "";
            //    temp_points[count_clic].X = e.X;
            //    temp_points[count_clic].Y = field.Height - e.Y;
            //    count_clic++;
            //}
            //else
            //{
            //    count_clic = 0;
            //    if ((temp_points[2].X - temp_points[0].X) * (temp_points[1].Y - temp_points[0].Y) - (temp_points[2].Y - temp_points[0].Y) * (temp_points[1].X - temp_points[0].X) > 0)
            //    {
            //        Point temp;
            //        temp = temp_points[2];
            //        temp_points[2] = temp_points[1];
            //        temp_points[1] = temp;
            //    }
            //    double t = (double)(temp_points[0].Y - temp_points[2].Y) / (temp_points[0].Y - temp_points[1].Y - temp_points[2].Y + temp_points[1].Y);
            //    temp_points[3].X = temp_points[0].X + Convert.ToInt32((temp_points[2].X - temp_points[1].X) * t);
            //    temp_points[3].Y = temp_points[0].Y + Convert.ToInt32((temp_points[2].Y - temp_points[1].Y) * t);
            //    Draw(new Figure(temp_points));
            //}
            if (e.Button == MouseButtons.Left)
            {
                example = new Figure();
                RenderTime.Enabled = true;
                Draw(example);
            }
            else {
                if(e.Button == MouseButtons.Right)
                    example.setCenter(new Point(e.X, e.Y));
            }

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            double x = (double)trackBar1.Value;
            label1.Text = "" + x;
            
        }
    }

    public class Figure
    {
        private Point[] static_points;
        private Point center;
        private Point translate = new Point(0, 0);
        public double speed;
        private double angle;
        private double[] scale = new double[2];
        public Figure()
        {
            static_points = new Point[4];
            static_points[0].X = 200;
            static_points[0].Y = 200;

            static_points[1].X = 250;
            static_points[1].Y = 300;

            static_points[2].X = 450;
            static_points[2].Y = 300;

            static_points[3].X = 400;
            static_points[3].Y = 200;

            speed = 1.0f;

            angle = 0;

            scale[0] = 1;
            scale[1] = 1;
            

            center = new Point(225,250);
        }

        public Figure(Point[] new_points)
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

            //объявление точки вращения вокруг собственной оси center = new Point(x_c, y_c);
        }

        public void setTranslate(int x, int y)
        {
            translate.X += x;
            translate.Y += y;
        }

        public double getTranslateX()
        {
            return translate.X;
        }

        public double getTranslateY()
        {
            return translate.Y;
        }

        public double getCenterX()
        {
            return this.center.X;
        }
        public double getCenterY()
        {
            return this.center.Y;
        }
        public void setCenter(Point center)
        {
            this.center = center;
        }

        public void setSpeed(double x)
        {
            this.speed = x;
        }

        public double getSpeed()
        {
            return speed;
        }

        public void setPoints(Point[] new_points)
        {

        }

        public double getAngle()
        {
            return angle;
        }
        public Point[] getPoints()
        {
            return static_points;
        }
        // маштаб
        public void setScale(double x, double y)
        {
            scale[0] += x;
            scale[1] += y;
        }

        public double getScaleX()
        {
            return scale[0];
        }

        public double getScaleY()
        {
            return scale[1];
        }

        // перенос 
        public void CreateTranslate()
        {

        }

        // поворот
        public void CreateRotate(double angle)
        {
            this.angle += angle * Math.PI / 120.0 * speed;
        }
    }
}
