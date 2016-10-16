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
            Draw(new Figure());

        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void field_KeyDown(object sender, KeyEventArgs e)
        {

            // Z и X отвечают за масштабирование 
            if (e.KeyCode == Keys.Z)
            {
                // вызов функции, в которой мы реализуем масштабирование - передаем коэффициент масштабирования и выбранную ось в окне программы 
                
            }
            if (e.KeyCode == Keys.X)
            {
                // вызов функции, в которой мы реализуем масштабирование - передаем коэффициент масштабирования и выбранную ось в окне программы 
               
            }

            // W и S отвечают за перенос 
            if (e.KeyCode == Keys.W)
            {
                // вызов функции, в которой мы реализуем перенос - передаем значение перемещения и выбранную ось в окне программы 
                
            }
            if (e.KeyCode == Keys.S)
            {
                // вызов функции, в которой мы реализуем перенос - передаем значение перемещения и выбранную ось в окне программы 
                

            } // A и D отвечают за поворот 
            if (e.KeyCode == Keys.A)
            {
                // вызов функции, в которой мы реализуем поворот - передаем значение для поворота и выбранную ось 
                
            }
            if (e.KeyCode == Keys.D)
            {
                
            }
        }

        private void field_Load(object sender, EventArgs e)
        {

        }

        private void myMouse(object sender, MouseEventArgs e)
        {
            if (count_clic < 3)
            {
                label_x_cord.Text = e.X + "";
                label_y_cord.Text = field.Height - e.Y + "";
                temp_points[count_clic].X = e.X;
                temp_points[count_clic].Y = field.Height - e.Y;
                count_clic++;
            }
            else
            {
                count_clic = 0;
                if ((temp_points[2].X - temp_points[0].X) * (temp_points[1].Y - temp_points[0].Y) - (temp_points[2].Y - temp_points[0].Y) * (temp_points[1].X - temp_points[0].X) > 0)
                {
                    Point temp;
                    temp = temp_points[2];
                    temp_points[2] = temp_points[1];
                    temp_points[1] = temp;
                }
                double t = (double)(temp_points[0].Y - temp_points[2].Y) / (temp_points[0].Y - temp_points[1].Y - temp_points[2].Y + temp_points[1].Y);
                temp_points[3].X = temp_points[0].X + Convert.ToInt32((temp_points[2].X - temp_points[1].X) * t);
                temp_points[3].Y = temp_points[0].Y + Convert.ToInt32((temp_points[2].Y - temp_points[1].Y) * t);
                Draw(new Figure(temp_points));
            }


        }
    }

    public class Figure
    {
        private Point[] static_points;
        private Point center;
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
            //объявление точки вращения вокруг собственной оси center = new Point(x_c, y_c);
        }

        public void setCenter(Point center)
        {
            this.center = center;
        }

        public void setPoints(Point[] new_points)
        {

        }

        public Point[] getPoints()
        {
            return static_points;
        }
        // маштаб
        public void CreateZoom()
        {

        }

        // перенос 
        public void CreateTranslate()
        {

        }

        // поворот
        public void CreateRotate()
        {

        }
    }
}
