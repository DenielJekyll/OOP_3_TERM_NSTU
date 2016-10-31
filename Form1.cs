using System;
using System.Drawing;
using System.Windows.Forms;

using Tao.OpenGl;
using Tao.FreeGlut;
using System.Collections;
using System.IO;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        private short count_click = 0;
        private uint count_figures = 0;
        private ArrayList figures = new ArrayList();
        private Point[] temp_points = new Point[6];
        private bool flag_input = false;// триггер для произвольного ввода точек
        private Shape GLOBAL = new Shape();
        private int pointer_shape = 0;
        public const int PARALLELOGRAM = 14, PENTAGON = 8, ELLIPSE = 6, RHOMBUS = 4;

        /* 
        параллелограмм = 14
        элипс = 5
        пентагон = 8
        ромб = 4
         Список известных ошибок:
         - если при задании точек все точки будут лежать на одной прямой => вылет
         Тест: не проверено

        - если задать все 3 точки как одну (т.е. жать в одну точку 3 раза), то значение t = NaN; 
        функция myMouse() -> left button
        Тест: вставлен костыль try - cath 
         */

        public Form1()
        {
            InitializeComponent();
            field.InitializeContexts();
            field.MouseWheel += field_Mouse_Wheel;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
                temp_points[i] = new Point(-1, -1);
            // инициализация Glut 
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_SINGLE);
            // очистка окна 
            Gl.glClearColor(0, 0, 0, 1);
            // установка порта вывода в соответствии с размерами элемента anT 
            Gl.glViewport(0, 0, field.Width, field.Height);
            Gl.glPointSize(5);
            Gl.glLineWidth(3f);
            Gl.glEnable(Gl.GL_POINT_SMOOTH);
            // настройка проекции 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            // теперь необходимо корректно настроить 2D ортогональную проекцию 
            Glu.gluOrtho2D(0.0, (float)field.Width, 0.0, (float)field.Height);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            cboxSelectedType.SelectedIndex = 0;
        }

        private void Draw()
        {
            // очистка буфера цвета и буфера глубины 
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glClearColor(255, 255, 255, 1);
            // очищение текущей матрицы 
            Gl.glLoadIdentity();
            // помещаем состояние матрицы в стек матриц 

            Gl.glBegin(Gl.GL_LINES);
            Gl.glColor3d(0, 0, 0);
            Gl.glVertex2d(0.5, 0.5);
            Gl.glVertex2d(0.5, field.Height);
            Gl.glVertex2d(0.5, 0.5);
            Gl.glVertex2d(field.Width, 0.5);
            Gl.glEnd();
            if (count_figures > 0)
            {
                int type;
                foreach (dynamic temp in figures)
                {

                    Gl.glPushMatrix();
                    Gl.glTranslated(temp.translate.X, temp.translate.Y, 0.0f);
                    Gl.glTranslated(temp.center.X, temp.center.Y, 0.0f);
                    Gl.glScaled(temp.scale, temp.scale, 1);
                    Gl.glRotated(temp.angle, 0, 0, 1);
                    Gl.glTranslated(-temp.center.X, -temp.center.Y, 0.0f);
                    type = temp.type;
                    Gl.glColor3ub(temp.color.R, temp.color.G, temp.color.B);
                    switch (type)
                    {
                        case PENTAGON:
                            draw_Pentagon(temp);
                            break;
                        case ELLIPSE:
                            draw_Ellipse(temp);
                            break;
                        default:
                            draw_Quadrangle(temp);
                            break;
                    }
                    Gl.glPopMatrix();
                }
            }
            if (flag_input)
            {
                Gl.glBegin(Gl.GL_POINTS);
                Gl.glColor3d(0, 0, 0);
                Gl.glVertex2d(temp_points[0].X, temp_points[0].Y);
                Gl.glVertex2d(temp_points[1].X, temp_points[1].Y);
                Gl.glEnd();
                Gl.glColor3d(0, 0, 0);
                Gl.glEnable(Gl.GL_LINE_STIPPLE);
                Gl.glLineStipple(2, 0x0103);
                if (count_click > 1)
                    switch (cboxSelectedType.Text.Length)
                    {
                    case PARALLELOGRAM:
                            Gl.glBegin(Gl.GL_LINE_LOOP);
                            Gl.glVertex2f(GLOBAL.static_points[0].X, GLOBAL.static_points[0].Y);
                            Gl.glVertex2f(GLOBAL.static_points[1].X, GLOBAL.static_points[1].Y);
                            Gl.glVertex2f(GLOBAL.static_points[2].X, GLOBAL.static_points[2].Y);
                            Gl.glVertex2f(GLOBAL.static_points[3].X, GLOBAL.static_points[3].Y);
                            Gl.glEnd();
                        break;
                        case RHOMBUS:
                            Gl.glBegin(Gl.GL_LINE_LOOP);
                            Gl.glVertex2f(GLOBAL.static_points[1].X, GLOBAL.static_points[1].Y);
                            Gl.glVertex2f(GLOBAL.static_points[3].X, GLOBAL.static_points[3].Y);
                            Gl.glVertex2f(GLOBAL.static_points[2].X, GLOBAL.static_points[2].Y);
                            Gl.glVertex2f(GLOBAL.static_points[4].X, GLOBAL.static_points[4].Y);
                            Gl.glEnd();
                            break;
                        case PENTAGON:
                            Gl.glBegin(Gl.GL_LINE_LOOP);
                            Gl.glVertex2f(GLOBAL.static_points[1].X, GLOBAL.static_points[1].Y);
                            Gl.glVertex2f(GLOBAL.static_points[2].X, GLOBAL.static_points[2].Y);
                            Gl.glVertex2f(GLOBAL.static_points[3].X, GLOBAL.static_points[3].Y);
                            Gl.glVertex2f(GLOBAL.static_points[4].X, GLOBAL.static_points[4].Y);
                            Gl.glVertex2f(GLOBAL.static_points[5].X, GLOBAL.static_points[5].Y);
                            Gl.glEnd();
                            break;
                    default:
                        break;
                    }
                Gl.glDisable(Gl.GL_LINE_STIPPLE);
            }
            // отрисовываем геометрию 
            Gl.glFlush();
            // обновляем состояние элемента 
            field.Invalidate();

        }

        private void lock_Interface(bool i)
        {
            cboxSelectedType.Enabled = i;
            cboxCountFigures.Enabled = i;
            btnCreatePrimitive.Enabled = i;
            btnCreateArbitrarily.Enabled = i;
        }

        private void draw_Quadrangle(dynamic shape)
        {

            Point[] points = shape.static_points;
            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glVertex2f(points[0].X, points[0].Y);
            Gl.glVertex2f(points[1].X, points[1].Y);
            Gl.glVertex2f(points[2].X, points[2].Y);
            Gl.glVertex2f(points[3].X, points[3].Y);
            Gl.glEnd();
            if (shape.active)
            {
                Gl.glColor3ub((byte)(255 - shape.color.R), (byte)(255 - shape.color.G), (byte)(255 - shape.color.B));
                Gl.glBegin(Gl.GL_LINE_LOOP);
                Gl.glVertex2f(points[3].X, points[3].Y);
                Gl.glVertex2f(points[0].X, points[0].Y);
                Gl.glVertex2f(points[1].X, points[1].Y);
                Gl.glVertex2f(points[2].X, points[2].Y);
                Gl.glEnd();
                Gl.glBegin(Gl.GL_POINTS);
                Gl.glColor3d(0, 0, 0);
                Gl.glVertex2d(shape.center.X, shape.center.Y);
                Gl.glEnd();
            }
        }
        private void draw_Ellipse(dynamic ellipse)
        {
            double x, y;
            int t;
            Gl.glBegin(Gl.GL_POLYGON);
            for (t = 0; t <= 360; t += 1)
            {
                x = 150 * Math.Sin(t) + ellipse.center.X;
                y = 50 * Math.Cos(t) + ellipse.center.Y;
                Gl.glVertex2d(x, y);
            }
            Gl.glEnd();
            if (ellipse.active)
            {
                Gl.glPointSize(3);
                Gl.glColor3ub((byte)(255 - ellipse.color.R), (byte)(255 - ellipse.color.G), (byte)(255 - ellipse.color.B));
                Gl.glBegin(Gl.GL_POINTS);
                for (t = 0; t <= 360; t += 1)
                {
                    x = 150 * Math.Sin(t) + ellipse.center.X;
                    y = 50 * Math.Cos(t) + ellipse.center.Y;
                    Gl.glVertex2d(x, y);
                }
                Gl.glEnd();
                Gl.glPointSize(5);
                Gl.glBegin(Gl.GL_POINTS);
                Gl.glColor3d(0, 0, 0);
                Gl.glVertex2d(ellipse.center.X, ellipse.center.Y);
                Gl.glEnd();
            }
        }
        private void draw_Pentagon(dynamic pentagon)
        {
            Point[] points = pentagon.static_points;
            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glVertex2f(points[0].X, points[0].Y);
            Gl.glVertex2f(points[1].X, points[1].Y);
            Gl.glVertex2f(points[2].X, points[2].Y);
            Gl.glVertex2f(points[3].X, points[3].Y);
            Gl.glVertex2f(points[4].X, points[4].Y);
            Gl.glEnd();
            if (pentagon.active)
            {
                Gl.glColor3ub((byte)(255 - pentagon.color.R), (byte)(255 - pentagon.color.G), (byte)(255 - pentagon.color.B));
                Gl.glBegin(Gl.GL_LINE_LOOP);
                Gl.glVertex2f(points[0].X, points[0].Y);
                Gl.glVertex2f(points[1].X, points[1].Y);
                Gl.glVertex2f(points[2].X, points[2].Y);
                Gl.glVertex2f(points[3].X, points[3].Y);
                Gl.glVertex2f(points[4].X, points[4].Y);
                Gl.glEnd();
                Gl.glBegin(Gl.GL_POINTS);
                Gl.glColor3d(0, 0, 0);
                Gl.glVertex2d(pentagon.center.X, pentagon.center.Y);
                Gl.glEnd();
            }
        }

        private void create_Parallelogram(MouseEventArgs e)
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
                lock_Interface(true);
                try
                {
                    double t = (double)(temp_points[0].Y - temp_points[2].Y) / (temp_points[0].Y - temp_points[1].Y - temp_points[2].Y + temp_points[1].Y);
                    temp_points[3].X = temp_points[0].X + Convert.ToInt32((temp_points[2].X - temp_points[1].X) * t);
                    temp_points[3].Y = temp_points[0].Y + Convert.ToInt32((temp_points[2].Y - temp_points[1].Y) * t);
                    t = (double)(temp_points[1].Y - temp_points[2].Y) / (temp_points[2].Y - temp_points[0].Y - temp_points[1].Y + temp_points[3].Y);
                    figures.Add(new Parallelogram(temp_points, new Point(temp_points[1].X + Convert.ToInt32((temp_points[1].X - temp_points[3].X) * t),
                    temp_points[1].Y + Convert.ToInt32((temp_points[1].Y - temp_points[3].Y) * t))));

                    cboxCountFigures.Items.Add(count_figures++);
                    cboxCountFigures.SelectedItem = count_figures - 1;
                    for (int i = 0; i < 5; i++)
                        temp_points[i] = new Point(-1, -1);
                }
                catch (Exception)
                {
                    exeption_label.Text = "Некоректно заданы точки, фигура не построена";
                    for (int i = 0; i < 5; i++)
                        temp_points[i] = new Point(-1, -1);
                }

            }
        }

        private void create_Ellipse(MouseEventArgs e) { }

        private void create_Pentagon(MouseEventArgs e)
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
                lock_Interface(true);
                try { 
                double u, t;
                double pX, pY;
                t = Math.Sqrt(Math.Pow(e.X - temp_points[0].X, 2) + Math.Pow(field.Height - e.Y - temp_points[0].Y, 2));
                u = Math.Acos((temp_points[1].X - temp_points[0].X) / Math.Sqrt(Math.Pow((temp_points[1].X - temp_points[0].X), 2) + Math.Pow((temp_points[1].Y - temp_points[0].Y), 2)));
                for (int i = 1; i < 6; i++)
                {
                    pX = Math.Cos(u);
                    pY = Math.Sin(u);
                    temp_points[i].X = temp_points[0].X + Convert.ToInt32(pX * t);
                    temp_points[i].Y = temp_points[0].Y + Convert.ToInt32(pY * t);
                    u += Math.PI * 0.4;
                }

                figures.Add(new Pentagon(temp_points));
                cboxCountFigures.Items.Add(count_figures++);
                cboxCountFigures.SelectedItem = count_figures - 1;
                }
                catch (Exception)
                {
                    exeption_label.Text = "Некоректно заданы точки, фигура не построена";
                }
                for (int i = 0; i < 6; i++)
                    temp_points[i] = new Point(-1, -1);
            }

        }

        private void create_Rhombus(MouseEventArgs e)
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
                lock_Interface(true);

                double u, t;
                double pX, pY;
                try
                {
                u = Math.Acos((temp_points[1].X - temp_points[0].X) / Math.Sqrt(Math.Pow((temp_points[1].X - temp_points[0].X), 2) + Math.Pow((temp_points[1].Y - temp_points[0].Y), 2))) + Math.PI / 2;
                pX = Math.Cos(u);
                pY = Math.Sin(u);
                t = Math.Sqrt(Math.Pow(e.X - temp_points[0].X, 2) + Math.Pow(field.Height - e.Y - temp_points[0].Y, 2));
                temp_points[2].X = temp_points[0].X + temp_points[0].X - temp_points[1].X;
                temp_points[2].Y = temp_points[0].Y + temp_points[0].Y - temp_points[1].Y;

                temp_points[3].X = temp_points[0].X + Convert.ToInt32(pX * t);
                temp_points[3].Y = temp_points[0].Y + Convert.ToInt32(pY * t);

                temp_points[4].X = temp_points[0].X + Convert.ToInt32(pX * (-t));
                temp_points[4].Y = temp_points[0].Y + Convert.ToInt32(pY * (-t));

                figures.Add(new Rhombus(temp_points));
                cboxCountFigures.Items.Add(count_figures++);
                cboxCountFigures.SelectedItem = count_figures - 1;
                }
                catch (Exception)
                {
                    exeption_label.Text = "Некоректно заданы точки, фигура не построена";
                }
                for (int i = 0; i < 5; i++)
                    temp_points[i] = new Point(-1, -1);
            }
        }

        private void print_Text_2D(double x, double y, string text)
        {

            // устанавливаем позицию вывода растровых символов 
            // в переданных координатах x и y. 
            Gl.glRasterPos2d(x, y);

            // в цикле foreach перебираем значения из массива text, 
            // который содержит значение строки для визуализации 
            foreach (char char_for_draw in text)
            {
                // символ C визуализируем с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
                Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
            }

        }


        //************* field обработчики **************


        private void shift()
        {
            for(int i=5; i>0; i--)
            {
                temp_points[i] = temp_points[i - 1];
            }
        }

    

        private void field_Key_Down(object sender, KeyEventArgs e)
        {
            if (count_figures > 0)
            {
                dynamic temp = figures[pointer_shape];
                switch (e.KeyCode)
                {
                    // W A S D отвечают за перемещение по x и y 
                    case Keys.W:
                        temp.setTranslate(0, temp.move_speed);
                        break;
                    case Keys.S:
                        temp.setTranslate(0, -temp.move_speed);
                        break;
                    case Keys.A:
                        temp.setTranslate(-temp.move_speed, 0);
                        break;
                    case Keys.D:
                        temp.setTranslate(temp.move_speed, 0);
                        break;
                    // Е и Q отвечают за поворот
                    case Keys.E:
                        temp.toRotate(1.0f);
                        break;
                    case Keys.Q:
                        temp.toRotate(-1.0f);
                        break;
                }
            }
        }

        private void field_Mouse_Move(object sender, MouseEventArgs e)
        {
            double t, u, pX, pY;
            try
            {
            if (flag_input)
                if (count_click > 1)
                    switch (cboxSelectedType.Text.Length)
                    {
                        case PARALLELOGRAM:
                            temp_points[2].X = e.X;
                            temp_points[2].Y = field.Height - e.Y;
                            t = (double)(temp_points[0].Y - temp_points[2].Y) / (temp_points[0].Y - temp_points[1].Y - temp_points[2].Y + temp_points[1].Y);
                            temp_points[3].X = temp_points[0].X + Convert.ToInt32((temp_points[2].X - temp_points[1].X) * t);
                            temp_points[3].Y = temp_points[0].Y + Convert.ToInt32((temp_points[2].Y - temp_points[1].Y) * t);
                            break;
                        case RHOMBUS:
                            t = Math.Sqrt(Math.Pow(e.X - temp_points[0].X, 2) + Math.Pow(field.Height - e.Y - temp_points[0].Y, 2));

                            u = Math.Acos((temp_points[1].X - temp_points[0].X) / Math.Sqrt(Math.Pow((temp_points[1].X - temp_points[0].X), 2) + Math.Pow((temp_points[1].Y - temp_points[0].Y), 2))) + Math.PI / 2;
                            pX = Math.Cos(u);
                            pY = Math.Sin(u);

                            temp_points[2].X = temp_points[0].X + temp_points[0].X - temp_points[1].X;
                            temp_points[2].Y = temp_points[0].Y + temp_points[0].Y - temp_points[1].Y;

                            temp_points[3].X = temp_points[0].X + Convert.ToInt32(pX * t);
                            temp_points[3].Y = temp_points[0].Y + Convert.ToInt32(pY * t);

                            temp_points[4].X = temp_points[0].X + Convert.ToInt32(pX * (-t));
                            temp_points[4].Y = temp_points[0].Y + Convert.ToInt32(pY * (-t));
                            break;
                        case PENTAGON:
                            t = Math.Sqrt(Math.Pow(e.X - temp_points[0].X, 2) + Math.Pow(field.Height - e.Y - temp_points[0].Y, 2));
                            u = Math.Acos((temp_points[1].X - temp_points[0].X) / Math.Sqrt(Math.Pow((temp_points[1].X - temp_points[0].X), 2) + Math.Pow((temp_points[1].Y - temp_points[0].Y), 2)));
                            for (int i = 1; i < 6; i++)
                            {
                                pX = Math.Cos(u);
                                pY = Math.Sin(u);
                                temp_points[i].X = temp_points[0].X + Convert.ToInt32(pX * t);
                                temp_points[i].Y = temp_points[0].Y + Convert.ToInt32(pY * t);
                                u += Math.PI * 0.4;
                            }
                            break;
                        default:
                            break;
                    }
                GLOBAL.static_points = temp_points;
            }
            catch (Exception)
            {
                GLOBAL.static_points = temp_points;
            }

        }

        private void field_Mouse_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (flag_input)
                    switch (cboxSelectedType.Text.Length)
                    {
                        case PARALLELOGRAM:
                            create_Parallelogram(e);
                            break;
                        case RHOMBUS:
                            create_Rhombus(e);
                            break;
                        case PENTAGON:
                            create_Pentagon(e);
                            break;
                        default:
                            exeption_label.Text = "Ошибка не выбран тип создаваемой фигуры";
                            break;
                    }
            }
        }

        private void field_Mouse_Wheel(object sender, MouseEventArgs e)
        {
            if (count_figures > 0)
            {
                dynamic temp = figures[pointer_shape];
                if (e.Delta > 0)
                    if (count_figures > 0)
                        temp.scale = -0.05;
                if (e.Delta < 0)
                    if (count_figures > 0)
                        temp.scale = 0.05;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Draw();
        }

        //*************                 **************



        //************* элементы нижней панели **************
        private void rotating_Speed_Change(object sender, EventArgs e)
        {
            if (count_figures > 0)
            {
                dynamic temp = figures[pointer_shape];
                temp.rotating_speed = barRotatingSpeed.Value;
                field.Focus();
            }
        }

        private void move_Speed_Change(object sender, EventArgs e)
        {
            if (count_figures > 0)
            {
                dynamic temp = figures[pointer_shape];
                temp.move_speed = barMoveSpeed.Value;
                field.Focus();
            }
        }

        private void btn_Set_Color_Click(object sender, EventArgs e)
        {
            if (count_figures > 0)
            {
                dynamic temp = figures[pointer_shape];
                ColorDialog MyDialog = new ColorDialog();
                MyDialog.AllowFullOpen = false;
                MyDialog.ShowHelp = true;
                MyDialog.Color = temp.color;
                if (MyDialog.ShowDialog() == DialogResult.OK)
                {
                    btnSetColor.BackColor = MyDialog.Color;
                    temp.color = MyDialog.Color;
                }
                field.Focus();
            }
        }
        //*************               **************


        //************* элементы правой панели **************
        private void btn_Create_Primitive(object sender, EventArgs e)
        {
            bool flag = true;
            switch (cboxSelectedType.Text.Length)
            {
                case PARALLELOGRAM:
                    figures.Add(new Parallelogram());
                    break;
                case PENTAGON:
                    figures.Add(new Pentagon());
                    break;
                case ELLIPSE:
                    figures.Add(new Ellipse());
                    break;
                case RHOMBUS:
                    figures.Add(new Rhombus());
                    break;
                default:
                    flag = false;
                    exeption_label.Text = "Ошибка не выбран тип создаваемой фигуры";
                    break;
            }
            field.Focus();
            if (flag)
            { 
                cboxCountFigures.Items.Add(count_figures++);
                cboxCountFigures.SelectedItem = count_figures - 1;
                exeption_label.Text = "";
            }
        }

        private void сортировкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.figures = this.figures;
            f2.Show();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileStream file = new FileStream("shapes.txt", FileMode.Create);
            StreamWriter writer = new StreamWriter(file);

            foreach (dynamic temp in figures)
            {
                writer.Write("#" + Environment.NewLine + temp.type + Environment.NewLine);
                foreach (Point p in temp.static_points)
                    writer.Write(p.X + " " + p.Y + " ");
                writer.Write(Environment.NewLine + temp.center.X + " " + temp.center.Y + Environment.NewLine + temp.angle + Environment.NewLine
                    + temp.move_speed + " " + temp.rotating_speed + Environment.NewLine + temp.color.R + " " + temp.color.G + " " + temp.color.B
                    + Environment.NewLine + temp.scale + Environment.NewLine + temp.translate.X + " " + temp.translate.Y + Environment.NewLine);
            }
            writer.Close();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileStream file = new FileStream("shapes.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                count_click = 4;
                if (line != "#")
                    return;
                line = reader.ReadLine();
                int type = Convert.ToInt32(line);
                line = reader.ReadLine();
                string[] s = line.Split(' ');
                for (int C = 0; s[C] != ""; C++)
                    if (C % 2 == 0)
                        temp_points[C / 2].X = Convert.ToInt32(s[C]);
                    else
                        temp_points[(C - 1) / 2].Y = Convert.ToInt32(s[C]);
                line = reader.ReadLine();
                switch (type)
                {
                    case PARALLELOGRAM:
                        Parallelogram p = new Parallelogram(temp_points, new Point(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1])));
                        p.angle = Convert.ToDouble(reader.ReadLine());
                        line = reader.ReadLine();
                        p.rotating_speed = Convert.ToInt32(line.Split(' ')[1]);
                        p.move_speed = Convert.ToInt32(line.Split(' ')[0]);
                        line = reader.ReadLine();
                        p.color = Color.FromArgb(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]), Convert.ToInt32(line.Split(' ')[2]));
                        p.scale = Convert.ToDouble(reader.ReadLine()) - 1;
                        line = reader.ReadLine();
                        p.setTranslate(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]));
                        figures.Add(p);
                        cboxCountFigures.Items.Add(count_figures++);
                        cboxCountFigures.SelectedItem = count_figures - 1;
                        for (int i = 0; i < 5; i++)
                            temp_points[i] = new Point(-1, -1);
                        break;
                    case PENTAGON:
                        shift();
                        Pentagon pentagon = new Pentagon(temp_points);
                        Point center = new Point(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]));
                        pentagon.center = center;
                        pentagon.angle = Convert.ToDouble(reader.ReadLine());
                        line = reader.ReadLine();
                        pentagon.rotating_speed = Convert.ToInt32(line.Split(' ')[1]);
                        pentagon.move_speed = Convert.ToInt32(line.Split(' ')[0]);
                        line = reader.ReadLine();
                        pentagon.color = Color.FromArgb(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]), Convert.ToInt32(line.Split(' ')[2]));
                        pentagon.scale = Convert.ToDouble(reader.ReadLine()) - 1;
                        line = reader.ReadLine();
                        pentagon.setTranslate(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]));
                        figures.Add(pentagon);
                        cboxCountFigures.Items.Add(count_figures++);
                        cboxCountFigures.SelectedItem = count_figures - 1;
                        for (int i = 0; i < 5; i++)
                            temp_points[i] = new Point(-1, -1);
                        break;
                    case RHOMBUS:
                        shift();
                        Rhombus rhombus = new Rhombus(temp_points);
                        Point cntr = new Point(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]));
                        rhombus.center = cntr;
                        rhombus.angle = Convert.ToDouble(reader.ReadLine());
                        line = reader.ReadLine();
                        rhombus.rotating_speed = Convert.ToInt32(line.Split(' ')[1]);
                        rhombus.move_speed = Convert.ToInt32(line.Split(' ')[0]);
                        line = reader.ReadLine();
                        rhombus.color = Color.FromArgb(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]), Convert.ToInt32(line.Split(' ')[2]));
                        rhombus.scale = Convert.ToDouble(reader.ReadLine()) - 1;
                        line = reader.ReadLine();
                        rhombus.setTranslate(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]));
                        figures.Add(rhombus);
                        cboxCountFigures.Items.Add(count_figures++);
                        cboxCountFigures.SelectedItem = count_figures - 1;
                        for (int i = 0; i < 5; i++)
                            temp_points[i] = new Point(-1, -1);
                        break;
                    default:
                        exeption_label.Text = "Ошибка не выбран тип создаваемой фигуры";
                        break;
                }
            }

            reader.Close();
            Console.ReadLine();
        }

        private void enable_Input_Shape(object sender, EventArgs e)
        {
            exeption_label.Text = "";
            flag_input = true;
            lock_Interface(false);
            field.Focus();
        }

        private void cbox_Selected_Item_Change(object sender, EventArgs e)
        {

          dynamic temp = figures[pointer_shape];
          temp.active = false;
          pointer_shape = Convert.ToInt32(cboxCountFigures.Text);
          temp = figures[pointer_shape];
          temp.active = true;
          barMoveSpeed.Value = temp.move_speed;
          barRotatingSpeed.Value = temp.rotating_speed;
          btnSetColor.BackColor = temp.color;
          field.Focus();

        }

        private void btn_Delete_Sel_Shape(object sender, EventArgs e)
        {
            if (count_figures > 0)
            {
                figures.Remove((Shape)figures[pointer_shape]);
                cboxCountFigures.Items.RemoveAt((int)count_figures - 1);
                count_figures--;
                pointer_shape = ((int)count_figures - 1 < 0) ? 0 : (int)count_figures - 1;
            }
            field.Focus();
        }

        //*************               **************
    }

    public class Shape
    {
        private double _scale;
        private Point _translate;
        public Color color { set; get; }
        public Point center { set; get; }
        public double angle { set; get; }
        public bool active { set; get; }
        public int move_speed { set; get; }
        public int rotating_speed { set; get; }
        public Point[] static_points { set; get; }
        public Point translate { get { return _translate; } }
        public double scale { set { _scale += value; } get { return _scale; } }

        public void toRotate(double angle)
        {
            this.angle += angle * Math.PI/10 * rotating_speed;
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
        public Parallelogram()
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

            type = 14;
            angle = 0;
            scale = 1;
            active = true;
            move_speed = 2;
            rotating_speed = 1;
            _translate = new Point(0, 0);
            center = new Point(322, 253);

            Random rand = new Random();
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }

        public Parallelogram(Point[] new_points, Point c)
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
            scale = 1;
            center = c;
            active = true;
            move_speed = 2;
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

        public Pentagon()
        {
            static_points = new Point[5];
            static_points[0].X = 200;
            static_points[0].Y = 300;

            static_points[1].X = 350;
            static_points[1].Y = 400;

            static_points[2].X = 500;
            static_points[2].Y = 300;

            static_points[3].X = 450;
            static_points[3].Y = 150;

            static_points[4].X = 250;
            static_points[4].Y = 150;

            type = 8;
            angle = 0;
            scale = 1;
            active = true;
            move_speed = 2;
            rotating_speed = 1;
            _translate = new Point(0, 0);
            center = new Point(350, 275);

            Random rand = new Random();
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            
        }

        public Pentagon(Point[] new_points)
        {
            static_points = new Point[5];
            static_points[0].X = new_points[1].X;
            static_points[0].Y = new_points[1].Y;

            static_points[1].X = new_points[2].X;
            static_points[1].Y = new_points[2].Y;

            static_points[2].X = new_points[3].X;
            static_points[2].Y = new_points[3].Y;

            static_points[3].X = new_points[4].X;
            static_points[3].Y = new_points[4].Y;

            static_points[4].X = new_points[5].X;
            static_points[4].Y = new_points[5].Y;

            type = 8;
            angle = 0;
            scale = 1;
            center = new Point(new_points[0].X, new_points[0].Y);
            active = true;
            move_speed = 2;
            rotating_speed = 1;
            _translate = new Point(0, 0);

            Random rand = new Random();
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }
    }

    public class Ellipse : Shape
    {
        private Point _translate;
        public int type { get; }
        public Ellipse()
        {
            static_points = new Point[2];

            type = 6;
            angle = 0;
            scale = 1;
            active = true;
            move_speed = 2;
            rotating_speed = 1;
            _translate = new Point(0, 0);
            center = new Point(300, 300);

            Random rand = new Random();
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));

        }

        public Ellipse(Point[] new_points, Point c)
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

            angle = 0;
            scale = 1;
            center = c;
            active = true;
            move_speed = 2;
            rotating_speed = 1;
            _translate = new Point(0, 0);

            Random rand = new Random();
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }
    }
    
    public class Rhombus : Shape
    {
        private Point _translate;
        public int type { get; }
        public Rhombus()
        {
            
            static_points = new Point[4];
            static_points[0].X = 200;
            static_points[0].Y = 250;

            static_points[1].X = 300;
            static_points[1].Y = 350;

            static_points[2].X = 400;
            static_points[2].Y = 250;

            static_points[3].X = 300;
            static_points[3].Y = 150;

            type = 4;
            angle = 0;
            scale = 1;
            active = true;
            move_speed = 2;
            rotating_speed = 1;
            _translate = new Point(0, 0);
            center = new Point(300, 250);

            Random rand = new Random();
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));

        }

        public Rhombus(Point[] new_points)
        {
            static_points = new Point[4];
            static_points[0].X = new_points[1].X;
            static_points[0].Y = new_points[1].Y;

            static_points[1].X = new_points[3].X;
            static_points[1].Y = new_points[3].Y;

            static_points[2].X = new_points[2].X;
            static_points[2].Y = new_points[2].Y;

            static_points[3].X = new_points[4].X;
            static_points[3].Y = new_points[4].Y;

            angle = 0;
            scale = 1;
            center = new Point(new_points[0].X, new_points[0].Y);
            active = true;
            move_speed = 2;
            rotating_speed = 1;
            _translate = new Point(0, 0);

            Random rand = new Random();
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }
    }

}
