using System;
using System.Drawing;
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
        private short count_click = 0;
        private Point[] temp_points = new Point[4];
        private bool flag_input = false;// триггер для произвольного ввода точек

        public Form1()
        {
            InitializeComponent();
            field.InitializeContexts();
            field.MouseWheel += myMouseWheel;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++)
                temp_points[i] = new Point(-1, -1);
            // инициализация Glut 
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_SINGLE);
            // очистка окна 
            Gl.glClearColor(0, 0, 0, 1);
            // установка порта вывода в соответствии с размерами элемента anT 
            Gl.glViewport(0, 0, field.Width, field.Height);
            Gl.glPointSize(5);
            Gl.glEnable(Gl.GL_POINT_SMOOTH);
            // настройка проекции 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            // теперь необходимо корректно настроить 2D ортогональную проекцию 
            Glu.gluOrtho2D(0.0, (float)field.Width, 0.0, (float)field.Height);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }

        private void Draw()
        {
            // очистка буфера цвета и буфера глубины 
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glClearColor(255, 255, 255, 1);
            // очищение текущей матрицы 
            Gl.glLoadIdentity();
            // помещаем состояние матрицы в стек матриц 
            Gl.glPushMatrix();
            if (example != null)
            {
                /*
                 Список известных ошибок:
                 - центр вращения "не пересчитывается" после сдвига фигуры по x || y
                 Тест: создать фигуру, задать центр, убрать руку от мыши, повращать, 
                 убедиться, что вращение проиходит вокруг выбранной точки, сдвинуть фигуру по x и повращать заного.
                 Важный момент, точка которая является центром будет смещена на ваш сдвиг, 
                 но вращение будет происходить относительно старой точки, где ваш курсов мыши.

                 - если повернуть фигуру на 180, то происходит эффект инверсии(движение верх это движение вниз)
                 Тест: создать фигуру, задать центр, сделать поворот фигуры на 180, попытаться сдвинуть по x || y.

                 - если при задании точек все точки будут лежать на одной прямой => вылет
                 Тест: не проверено

                - если задать все 3 точки как одну (т.е. жать в одну точку 3 раза), то значение t = NaN; 
                функция myMouse() -> left button
                Тест: вставлен костыль try - cath 
                 */
                Gl.glTranslated(example.getCenterX(), example.getCenterY(), 0.0f);
                Gl.glRotated(example.getAngle(), 0, 0, 1);
                Gl.glScalef((float)example.getScaleX(), (float)example.getScaleY(), 1);
                Gl.glTranslated(example.getTranslateX(), example.getTranslateY(), 0.0f);
                Gl.glTranslated(-example.getCenterX(), -example.getCenterY(), 0.0f);
                Point[] points = example.getPoints();
                Gl.glBegin(Gl.GL_POLYGON);
                Gl.glColor3d(255, 0, 0);
                Gl.glVertex2f(points[3].X, points[3].Y);
                Gl.glColor3d(0, 255, 0);
                Gl.glVertex2f(points[0].X, points[0].Y);
                Gl.glColor3d(0, 0, 255);
                Gl.glVertex2f(points[1].X, points[1].Y);
                Gl.glColor3d(255, 123, 0);
                Gl.glVertex2f(points[2].X, points[2].Y);
                Gl.glEnd();
                Gl.glBegin(Gl.GL_POINTS);
                Gl.glColor3d(255, 0, 0);
                Gl.glVertex2d(example.getCenterX(), example.getCenterY());
                Gl.glEnd();
            }
            if (flag_input)
            {
                Gl.glBegin(Gl.GL_POINTS);
                Gl.glColor3d(0, 0, 0);
                Gl.glVertex2d(temp_points[0].X, temp_points[0].Y);
                Gl.glVertex2d(temp_points[1].X, temp_points[1].Y);
                Gl.glVertex2d(temp_points[2].X, temp_points[2].Y);
                Gl.glEnd();
             }
                
            // возвращаем состояние матрицы 
            Gl.glPopMatrix();
            // отрисовываем геометрию 
            Gl.glFlush();
            // обновляем состояние элемента 
            field.Invalidate();

        }

        public void btn_create_primitive(object sender, EventArgs e)
        {
            example = new Figure();
            Draw();
            field.Focus();
        }

        private void field_Load(object sender, EventArgs e)
        {
           
        }



        //************* функции обработчики **************
        private void field_KeyDown(object sender, KeyEventArgs e)
        {
            if (example != null)
                switch (e.KeyCode)
                {
                    // W A S D отвечают за перемещение по x и y 
                    case Keys.W:
                        example.setTranslate(0, example.move_speed);
                        break;
                    case Keys.S:
                        example.setTranslate(0, -example.move_speed);
                        break;
                    case Keys.A:
                        example.setTranslate(-example.move_speed, 0);
                        break;
                    case Keys.D:
                        example.setTranslate(example.move_speed, 0);
                        break;
                    // Z и X маштабирование по x
                    case Keys.Z:
                        example.setScale(-0.05f, 0.00f);
                        break;
                    case Keys.X:
                        example.setScale(0.05f, 0.00f);
                        break;
                    //С и V маштабирование по y
                    case Keys.C:
                        example.setScale(0.0f, -0.03f);
                        break;
                    case Keys.V:
                        example.setScale(0.0f, 0.03f);
                        break;
                    // Е и Q отвечают за поворот
                    case Keys.E:
                        example.CreateRotate(1.0f);
                        break;
                    case Keys.Q:
                        example.CreateRotate(-1.0f);
                        break;
                }
        }

        private void myMouse(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (flag_input)
                {

                    if (count_click < 2)
                    {
                        temp_points[count_click].X = e.X;
                        temp_points[count_click].Y = field.Height - e.Y;
                        count_click++;
                    }
                    else
                    {
                        temp_points[count_click].X = e.X;
                        temp_points[count_click].Y = field.Height - e.Y;
                        count_click = 0;
                        flag_input = false;
                        if ((temp_points[2].X - temp_points[0].X) * (temp_points[1].Y - temp_points[0].Y) - (temp_points[2].Y - temp_points[0].Y) * (temp_points[1].X - temp_points[0].X) < 0)
                        {
                            Point p = temp_points[2];
                            temp_points[2] = temp_points[1];
                            temp_points[1] = p;
                        }
                        //warning t = NaN
                        try
                        {
                            double t = (double)(temp_points[0].Y - temp_points[2].Y) / (temp_points[0].Y - temp_points[1].Y - temp_points[2].Y + temp_points[1].Y);
                            temp_points[3].X = temp_points[0].X + Convert.ToInt32((temp_points[2].X - temp_points[1].X) * t);
                            temp_points[3].Y = temp_points[0].Y + Convert.ToInt32((temp_points[2].Y - temp_points[1].Y) * t);
                            //warning
                            example = new Figure(temp_points);
                            Draw();
                            for (int i = 0; i < 3; i++)
                                temp_points[i] = new Point(-1, -1);
                        }
                        catch (Exception)
                        {
                            exeption_label.Text = "Warning t = NaN Введите точки заного";
                            for (int i = 0; i < 3; i++)
                                temp_points[i] = new Point(-1, -1);
                        }

                    }
                }
            }
            if (e.Button == MouseButtons.Right)
                if (example != null)
                {
                    center_label.Text = "(" + e.X + ", " + (field.Height - e.Y) + ")";
                    example.setCenter(new Point(e.X, field.Height - e.Y));
                }
                   
        }

        private void myMouseWheel(object sender, MouseEventArgs e)
        {

            if (e.Delta > 0)
                if (example != null)
                    example.setScale(-0.05f, -0.05f);
            if (e.Delta < 0)
                if (example != null)
                    example.setScale(0.05f, 0.05f);
        }
        //*************                     **************



        //************* функции константы **************
        private void rotatingSpeed(object sender, EventArgs e)
        {
            if (example != null)
             example.rotating_speed = barRotatingSpeed.Value;
            field.Focus();
        }

        private void enableInput(object sender, EventArgs e)
        {
            exeption_label.Text = "";
            flag_input = true;
            field.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Draw();
        }

        private void moveSpeed(object sender, EventArgs e)
        {
            if (example != null)
                example.move_speed = barMoveSpeed.Value;
            field.Focus();
        }
        
        //*************               **************
    }

    public class Figure
    {
        private Point[] static_points;
        private Point center = new Point(0, 0);
        private Point translate = new Point(0, 0);
        public int move_speed = 2;
        public double rotating_speed = 1;
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
            
            angle = 0;

            scale[0] = 1;
            scale[1] = 1;
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
            scale[0] = 1;
            scale[1] = 1;
        }

        public void CreateRotate(double angle)
        {
            this.angle += angle * Math.PI * rotating_speed;
        }

        //************* set функции **************
        public void setTranslate(int x, int y)
        {
            translate.X += x;
            translate.Y += y;
        }

        public void setScale(double x, double y)
        {
            scale[0] += x;
            scale[1] += y;
        }

        public void setCenter(Point center)
        {
            this.center = center;
        }

        public void setPoints(Point[] new_points)
        {

        }

        //************* get функции **************
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

        public double getRotetingSpeed()
        {
            return rotating_speed;
        }

        public double getMoveSpeed()
        {
            return move_speed;
        }

        public double getAngle()
        {
            return angle;
        }

        public Point[] getPoints()
        {
            return static_points;
        }

        public double getScaleX()
        {
            return scale[0];
        }

        public double getScaleY()
        {
            return scale[1];
        }

    }
}
