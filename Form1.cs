using System;
using System.Drawing;
using System.Windows.Forms;

using Tao.OpenGl;
using Tao.FreeGlut;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        private short count_click = 0;
        private uint count_figures = 0;
        private Dictionary<uint, double> figuresList = new Dictionary<uint, double>();
        private BinaryTree Figures = new BinaryTree();
        private Point[] temp_points = new Point[6];
        private bool flag_input = false;// триггер для произвольного ввода точек
        private Shape GLOBAL = new Shape();
        private uint pointer_shape = 1;
        public const int PARALLELOGRAM = 14, PENTAGON = 8, RHOMBUS = 4;

        /* 
        параллелограмм = 14
        пентагон = 8
        ромб = 4
         Список известных ошибок:
         - если при задании точек все точки будут лежать на одной прямой => вылет
         Тест: не проверено

        - если задать все 3 точки как одну (т.е. жать в одну точку 3 раза), то значение t = NaN; 
        функция myMouse() -> left button
        Тест: вставлен костыль try - catch 
         */

        public Form1()
        {
            InitializeComponent();
            field.InitializeContexts();
            field.MouseWheel += field_Mouse_Wheel;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clear();
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
        private void clear()
        {
            for (int i = 0; i < 6; i++)
                temp_points[i] = new Point(-1, -1);
        }
        private void Draw()
        {
            dynamic queue = new Queue(); // создать новую очередь
            queue.Enqueue(Figures); // поместить в очередь первый уровень


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
                dynamic temp;
                while (queue.Count != 0) // пока очередь не пуста
                {
                    if (queue.Peek().Left != null)
                        queue.Enqueue(queue.Peek().Left);
                    if (queue.Peek().Right != null)
                        queue.Enqueue(queue.Peek().Right);
                    temp = queue.Dequeue().Data;

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

        public double norma(double x, double y)
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        private void lock_Interface(bool i)
        {
            cboxSelectedType.Enabled = i;
            cboxCountFigures.Enabled = i;
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

        private uint index()
        {
            if (count_figures > 0)
            {
                uint i;
                // починить поиск индекса
                for (i = 1; i <= cboxCountFigures.Items.Count; i++)
                    if (!figuresList.ContainsKey(i)) return i;
                return i;
            }
            return 1;
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
                    Point center = new Point(temp_points[1].X + Convert.ToInt32((temp_points[1].X - temp_points[3].X) * t),
                    temp_points[1].Y + Convert.ToInt32((temp_points[1].Y - temp_points[3].Y) * t));
                    double R = norma(temp_points[1].X - center.X, temp_points[1].Y - center.Y);
                    uint q = index();
                    Figures.Insert(new Parallelogram(temp_points, center, R, q));
                    figuresList.Add(q, R);
                    cboxCountFigures.Items.Add(q);
                    cboxCountFigures.SelectedItem = q;
                    count_figures++;
                    clear();

                }
                catch (Exception)
                {
                    exeption_label.Text = "Некоректно заданы точки, фигура не построена";
                }
                clear();
            }
        }

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
                try
                {
                    double u, t;
                    double pX, pY;
                    t = norma(e.X - temp_points[0].X, field.Height - e.Y - temp_points[0].Y);
                    u = Math.Acos((temp_points[1].X - temp_points[0].X) / norma(temp_points[1].X - temp_points[0].X, temp_points[1].Y - temp_points[0].Y));
                    for (int i = 1; i < 6; i++)
                    {
                        pX = Math.Cos(u);
                        pY = Math.Sin(u);
                        temp_points[i].X = temp_points[0].X + Convert.ToInt32(pX * t);
                        temp_points[i].Y = temp_points[0].Y + Convert.ToInt32(pY * t);
                        u += Math.PI * 0.4;
                    }
                    Point center = temp_points[0];
                    for (int i = 0; i < 5; i++)
                        temp_points[i] = temp_points[i + 1];
                    double R = norma(temp_points[0].X - center.X, temp_points[0].Y - center.Y);
                    uint q = index();
                    Figures.Insert(new Pentagon(temp_points, center, R, q));
                    figuresList.Add(q, R);
                    cboxCountFigures.Items.Add(q);
                    cboxCountFigures.SelectedItem = q;
                    count_figures++;
                }
                catch (Exception)
                {
                    exeption_label.Text = "Некоректно заданы точки, фигура не построена";
                }
                clear();
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
                Point center = new Point(temp_points[0].X, temp_points[0].Y);
                temp_points[0] = temp_points[1];
                temp_points[1].X = e.X;
                temp_points[1].Y = field.Height - e.Y;
                count_click = 0;
                flag_input = false;
                lock_Interface(true);

                double u, t;
                double pX, pY;
                try
                {
                    u = Math.Acos((temp_points[0].X - center.X) / norma(temp_points[0].X - center.X,temp_points[0].Y - center.Y)) + Math.PI / 2;
                    pX = Math.Cos(u);
                    pY = Math.Sin(u);
                    t = norma(e.X - center.X, field.Height - e.Y - center.Y);

                    temp_points[2].X = center.X + center.X - temp_points[0].X;
                    temp_points[2].Y = center.Y + center.Y - temp_points[0].Y;
                    temp_points[3].X = center.X + Convert.ToInt32(pX * t);
                    temp_points[3].Y = center.Y + Convert.ToInt32(pY * t);
                    temp_points[1].X = center.X + Convert.ToInt32(pX * (-t));
                    temp_points[1].Y = center.Y + Convert.ToInt32(pY * (-t));
                    double R = (norma(temp_points[1].X - center.X, temp_points[1].Y - center.Y) < norma(temp_points[0].X - center.X, temp_points[0].Y - center.Y)) ? norma(temp_points[0].X - center.X, temp_points[0].Y - center.Y) : norma(temp_points[1].X - center.X, temp_points[1].Y - center.Y);
                    uint q = index();
                    Figures.Insert(new Rhombus(temp_points, center, R, q));
                    figuresList.Add(q, R);
                    cboxCountFigures.Items.Add(q);
                    cboxCountFigures.SelectedItem = q;
                    count_figures++;
                }
                catch (Exception)
                {
                    exeption_label.Text = "Некоректно заданы точки, фигура не построена";
                }
                clear();
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



        private void field_Key_Down(object sender, KeyEventArgs e)
        {
            if (count_figures > 0)
            {
                dynamic temp = Figures.Find(figuresList[pointer_shape], pointer_shape).Data;
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
                                t = norma(e.X - temp_points[0].X, field.Height - e.Y - temp_points[0].Y);
                                u = Math.Acos((temp_points[1].X - temp_points[0].X) / norma(temp_points[1].X - temp_points[0].X, temp_points[1].Y - temp_points[0].Y)) + Math.PI / 2;
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
                                t = norma(e.X - temp_points[0].X, field.Height - e.Y - temp_points[0].Y);
                                u = Math.Acos((temp_points[1].X - temp_points[0].X) / norma(temp_points[1].X - temp_points[0].X, temp_points[1].Y - temp_points[0].Y)) + Math.PI / 2;
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
                            lock_Interface(true);
                            break;
                    }
            }
        }

        private void field_Mouse_Wheel(object sender, MouseEventArgs e)
        {
            if (count_figures > 0)
            {
                dynamic temp = Figures.Find(figuresList[pointer_shape], pointer_shape).Data;
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
                dynamic temp = Figures.Find(figuresList[pointer_shape], pointer_shape).Data;
                temp.rotating_speed = barRotatingSpeed.Value;
                field.Focus();
            }
        }

        private void move_Speed_Change(object sender, EventArgs e)
        {
            if (count_figures > 0)
            {
                dynamic temp = Figures.Find(figuresList[pointer_shape], pointer_shape).Data;
                temp.move_speed = barMoveSpeed.Value;
                field.Focus();
            }
        }

        private void btn_Set_Color_Click(object sender, EventArgs e)
        {
            if (count_figures > 0)
            {
                dynamic temp = Figures.Find(figuresList[pointer_shape], pointer_shape).Data;
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

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileStream file = new FileStream("shapes.txt", FileMode.Create);
            StreamWriter writer = new StreamWriter(file);
            dynamic queue = new Queue();
            dynamic temp;
            queue.Enqueue(Figures);
            while (queue.Count != 0) // пока очередь не пуста
            {
                if (queue.Peek().Left != null)
                    queue.Enqueue(queue.Peek().Left);
                if (queue.Peek().Right != null)
                    queue.Enqueue(queue.Peek().Right);
                temp = queue.Dequeue().Data;
                writer.Write("#" + Environment.NewLine + temp.type + Environment.NewLine);
                foreach (Point p in temp.static_points)
                    writer.Write(p.X + " " + p.Y + " ");
                writer.Write(Environment.NewLine + temp.center.X + " " + temp.center.Y + Environment.NewLine + temp.angle + Environment.NewLine
                    + temp.move_speed + " " + temp.rotating_speed + Environment.NewLine + temp.color.R + " " + temp.color.G + " " + temp.color.B
                    + Environment.NewLine + temp.scale + Environment.NewLine + temp.translate.X + " " + temp.translate.Y + Environment.NewLine);
            }
            writer.Close();
            file.Close();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileStream file = new FileStream("shapes.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file);
            string line;
            Point center;
            double R;
            uint q;
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
                        center = new Point(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]));
                        R = norma(temp_points[1].X - center.X, temp_points[1].Y - center.Y);
                        q = index();
                        Figures.Insert(new Parallelogram(temp_points, center, R, q));
                        figuresList.Add(q, R);
                        cboxCountFigures.Items.Add(q);
                        cboxCountFigures.SelectedItem = q;
                        count_figures++;
                        Parallelogram p = Figures.Find(figuresList[q], q).Data;
                        p.angle = Convert.ToDouble(reader.ReadLine());
                        line = reader.ReadLine();
                        p.rotating_speed = Convert.ToInt32(line.Split(' ')[1]);
                        p.move_speed = Convert.ToInt32(line.Split(' ')[0]);
                        line = reader.ReadLine();
                        p.color = Color.FromArgb(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]), Convert.ToInt32(line.Split(' ')[2]));
                        p.scale = Convert.ToDouble(reader.ReadLine()) - 1;
                        line = reader.ReadLine();
                        p.setTranslate(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]));
                        break;
                    case PENTAGON:
                        center = new Point(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]));
                        R = norma(temp_points[1].X - center.X, temp_points[1].Y - center.Y);
                        q = index();
                        Figures.Insert(new Pentagon(temp_points, center, R, q));
                        figuresList.Add(q, R);
                        cboxCountFigures.Items.Add(q);
                        cboxCountFigures.SelectedItem = q;
                        count_figures++;
                        Pentagon pentagon = Figures.Find(figuresList[q], q).Data;
                        pentagon.angle = Convert.ToDouble(reader.ReadLine());
                        line = reader.ReadLine();
                        pentagon.rotating_speed = Convert.ToInt32(line.Split(' ')[1]);
                        pentagon.move_speed = Convert.ToInt32(line.Split(' ')[0]);
                        line = reader.ReadLine();
                        pentagon.color = Color.FromArgb(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]), Convert.ToInt32(line.Split(' ')[2]));
                        pentagon.scale = Convert.ToDouble(reader.ReadLine()) - 1;
                        line = reader.ReadLine();
                        pentagon.setTranslate(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]));
                        break;
                    case RHOMBUS:
                        center = new Point(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]));
                        R = norma(temp_points[1].X - center.X, temp_points[1].Y - center.Y);
                        q = index();
                        Figures.Insert(new Rhombus(temp_points, center, R, q));
                        figuresList.Add(q, R);
                        cboxCountFigures.Items.Add(q);
                        cboxCountFigures.SelectedItem = q;
                        count_figures++;
                        Rhombus rhombus = Figures.Find(figuresList[q], q).Data;
                        rhombus.angle = Convert.ToDouble(reader.ReadLine());
                        line = reader.ReadLine();
                        rhombus.rotating_speed = Convert.ToInt32(line.Split(' ')[1]);
                        rhombus.move_speed = Convert.ToInt32(line.Split(' ')[0]);
                        line = reader.ReadLine();
                        rhombus.color = Color.FromArgb(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]), Convert.ToInt32(line.Split(' ')[2]));
                        rhombus.scale = Convert.ToDouble(reader.ReadLine()) - 1;
                        line = reader.ReadLine();
                        rhombus.setTranslate(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]));
                        break;
                    default:
                        exeption_label.Text = "Ошибка файл содержит не определенный тип фигуры " + type;
                        break;
                }
                clear();
            }
            reader.Close();
            file.Close();
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
            dynamic temp = Figures.Find(figuresList[pointer_shape], pointer_shape).Data;
            temp.active = false;
            pointer_shape = Convert.ToUInt32(cboxCountFigures.Text);
            temp = Figures.Find(figuresList[pointer_shape], pointer_shape).Data;
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
                Figures.Remove(Figures.Find(figuresList[pointer_shape], pointer_shape));
                figuresList.Remove(pointer_shape);
                cboxCountFigures.Items.RemoveAt(cboxCountFigures.SelectedIndex);
                count_figures--;
                pointer_shape = (count_figures > 0) ? (uint)cboxCountFigures.Items[(int)count_figures - 1] : 1;
                cboxCountFigures.SelectedItem = pointer_shape;
            }
            field.Focus();
        }

        //*************               **************
    }

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

            type = 14;

            this.index = index;
            angle = 0;
            scale = 1;
            center = c;
            this.R = R;
            active = false;
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

            this.index = index;
            type = 8;
            angle = 0;
            scale = 1;
            this.center = new Point(center.X, center.Y);
            this.R = R;
            active = false;
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
           

        }

        public Ellipse(Point[] new_points, Point c)
        {
           
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

            this.index = index;
            type = 4;
            angle = 0;
            scale = 1;
            center = c;
            this.R = R;
            active = false;
            move_speed = 2;
            rotating_speed = 1;
            _translate = new Point(0, 0);


            Random rand = new Random();
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }
    }

    public enum BinSide
    {
        Left,
        Right
    }
    /// <summary>
    /// Бинарное дерево поиска
    /// </summary>
    public class BinaryTree
    {

        public dynamic Data { get; private set; }
        public BinaryTree Left { get; set; }
        public BinaryTree Right { get; set; }
        public BinaryTree Parent { get; set; }

        /// <summary>
        /// Вставляет целочисленное значение в дерево
        /// </summary>
        /// <param name="data">Значение, которое добавится в дерево</param>
        public void Insert(dynamic data)
        {
            if (Data == null)
            {
                Data = data;
                return;
            }
            if (Data.R <= data.R)
            {
                if (Right == null) Right = new BinaryTree();
                Insert(data, Right, this);
                return;
            }
            else
            {
                if (Left == null) Left = new BinaryTree();
                Insert(data, Left, this);
            }
        }

        /// <summary>
        /// Вставляет значение в определённый узел дерева
        /// </summary>
        /// <param name="data">Значение</param>
        /// <param name="node">Целевой узел для вставки</param>
        /// <param name="parent">Родительский узел</param>
        private void Insert(dynamic data, BinaryTree node, BinaryTree parent)
        {
            if (node.Data == null)
            {
                node.Data = data;
                node.Parent = parent;
                return;
            }
            if (node.Data.R <= data.R)
            {
                if (node.Right == null) node.Right = new BinaryTree();
                Insert(data, node.Right, node);
            }
            else
            {
                if (node.Left == null) node.Left = new BinaryTree();
                Insert(data, node.Left, node);
            }
        }
        /// <summary>
        /// Уставляет узел в определённый узел дерева
        /// </summary>
        /// <param name="data">Вставляемый узел</param>
        /// <param name="node">Целевой узел</param>
        /// <param name="parent">Родительский узел</param>
        private void Insert(BinaryTree data, BinaryTree node, BinaryTree parent)
        {
            if (node.Data == null)
            {
                node.Data = data.Data;
                node.Left = data.Left;
                node.Right = data.Right;
                node.Parent = parent;
                return;
            }
            if (node.Data.R <= data.Data.R)
            {
                if (node.Right == null) node.Right = new BinaryTree();
                Insert(data, node.Right, node);
            }
            else
            {
                if (node.Left == null) node.Left = new BinaryTree();
                Insert(data, node.Left, node);
            }
        }
        /// <summary>
        /// Определяет, в какой ветви для родительского лежит данный узел
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private BinSide? MeForParent(BinaryTree node)
        {
            if (node.Parent == null) return null;
            if (node.Parent.Left == node) return BinSide.Left;
            if (node.Parent.Right == node) return BinSide.Right;
            return null;
        }

        /// <summary>
        /// Удаляет узел из дерева
        /// </summary>
        /// <param name="node">Удаляемый узел</param>
        public void Remove(BinaryTree node)
        {
            if (node == null) return;
            var me = MeForParent(node);

            if (me == null)
            {
                BinaryTree bufRightLeft = null;
                BinaryTree bufRightRight = null;
                node.Data = null;
                if (node.Right != null)
                {
                    bufRightLeft = node.Right.Left;
                    bufRightRight = node.Right.Right;
                    node.Data = node.Right.Data;
                }
                var bufLeft = node.Left;
                node.Right = bufRightRight;
                node.Left = bufRightLeft;
                if (bufLeft != null)
                    Insert(bufLeft, node, node);
                return;
            }


            //Если у узла нет дочерних элементов, его можно смело удалять
            if (node.Left == null && node.Right == null)
            {
                if (me == BinSide.Left)
                {
                    node.Parent.Left = null;
                }
                else
                {
                    node.Parent.Right = null;
                }
                return;
            }
            //Если нет левого дочернего, то правый дочерний становится на место удаляемого
            if (node.Left == null)
            {
                if (me == BinSide.Left)
                {
                    node.Parent.Left = node.Right;
                }
                else
                {
                    node.Parent.Right = node.Right;
                }

                node.Right.Parent = node.Parent;
                return;
            }
            //Если нет правого дочернего, то левый дочерний становится на место удаляемого
            if (node.Right == null)
            {
                if (me == BinSide.Left)
                {
                    node.Parent.Left = node.Left;
                }
                else
                {
                    node.Parent.Right = node.Left;
                }

                node.Left.Parent = node.Parent;
                return;
            }

            //Если присутствуют оба дочерних узла
            //то правый ставим на место удаляемого
            //а левый вставляем в правый

            if (me == BinSide.Left)
            {
                node.Parent.Left = node.Right;
            }
            if (me == BinSide.Right)
            {
                node.Parent.Right = node.Right;
            }
            if (me == null)
            {
                var bufLeft = node.Left;
                var bufRightLeft = node.Right.Left;
                var bufRightRight = node.Right.Right;
                node.Data = node.Right.Data;
                node.Right = bufRightRight;
                node.Left = bufRightLeft;
                Insert(bufLeft, node, node);
            }
            else
            {
                node.Right.Parent = node.Parent;
                Insert(node.Left, node.Right, node.Right);
            }
        }
        /// <summary>
        /// Удаляет значение из дерева
        /// </summary>
        /// <param name="data">Удаляемое значение</param>
        public void Remove(double R, uint index)
        {
            var removeNode = Find(R, index);
            if (removeNode != null)
            {
                Remove(removeNode);
            }
        }
        /// <summary>
        /// Ищет узел с заданным значением
        /// </summary>
        /// <param name="data">Значение для поиска</param>
        /// <returns></returns>
        public BinaryTree Find(double R, uint index)
        {
            if (Data.R == R && Data.index == index) return this;
            if (Data.R > R)
            {
                return Find(R, Left, index);
            }
            return Find(R, Right, index);
        }
        /// <summary>
        /// Ищет значение в определённом узле
        /// </summary>
        /// <param name="data">Значение для поиска</param>
        /// <param name="node">Узел для поиска</param>
        /// <returns></returns>
        private BinaryTree Find(double R, BinaryTree node, uint index)
        {
            if (node == null) return null;

            if (node.Data.R == R && node.Data.index == index) return node;
            if (node.Data.R > R)
            {
                return Find(R, node.Left, index);
            }
            return Find(R, node.Right, index);
        }

        /// <summary>
        /// Количество элементов в дереве
        /// </summary>
        /// <returns></returns>
        public long CountElements()
        {
            return CountElements(this);
        }
        /// <summary>
        /// Количество элементов в определённом узле
        /// </summary>
        /// <param name="node">Узел для подсчета</param>
        /// <returns></returns>
        private long CountElements(BinaryTree node)
        {
            long count = 1;
            if (node.Right != null)
            {
                count += CountElements(node.Right);
            }
            if (node.Left != null)
            {
                count += CountElements(node.Left);
            }
            return count;
        }

        public ArrayList Across(BinaryTree node)
        {
            ArrayList BinaryTree = new ArrayList();
            /*
             Аргументы метода:
             1. TreeNode node - текущий "элемент дерева" (ref  передача по ссылке)       
             2. ref string s - строка, в которой накапливается результат (ref - передача по ссылке)
            */
            dynamic queue = new Queue(); // создать новую очередь
            queue.Enqueue(node); // поместить в очередь первый уровень
            while (queue.Count != 0) // пока очередь не пуста
            {
                if (queue.Peek().Left != null)
                    queue.Enqueue(queue.Peek().Left);
                if (queue.Peek().Right != null)
                    queue.Enqueue(queue.Peek().Right);
                BinaryTree.Add(queue.Dequeue().Data);
            }
            return BinaryTree;
        }
    }
}
