namespace WindowsFormsApplication3
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.field = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.create_primitive = new System.Windows.Forms.Button();
            this.RenderTime = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_x_cord = new System.Windows.Forms.Label();
            this.label_y_cord = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.barRotatingSpeed = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.barMoveSpeed = new System.Windows.Forms.TrackBar();
            this.button1 = new System.Windows.Forms.Button();
            this.info_label = new System.Windows.Forms.Label();
            this.center_label = new System.Windows.Forms.Label();
            this.exeption_label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.barRotatingSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barMoveSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // field
            // 
            this.field.AccumBits = ((byte)(0));
            this.field.AutoCheckErrors = false;
            this.field.AutoFinish = false;
            this.field.AutoMakeCurrent = true;
            this.field.AutoSwapBuffers = true;
            this.field.BackColor = System.Drawing.SystemColors.Control;
            this.field.ColorBits = ((byte)(32));
            this.field.DepthBits = ((byte)(16));
            this.field.Location = new System.Drawing.Point(7, 8);
            this.field.Name = "field";
            this.field.Size = new System.Drawing.Size(600, 500);
            this.field.StencilBits = ((byte)(0));
            this.field.TabIndex = 0;
            this.field.Load += new System.EventHandler(this.field_Load);
            this.field.KeyDown += new System.Windows.Forms.KeyEventHandler(this.field_KeyDown);
            this.field.MouseClick += new System.Windows.Forms.MouseEventHandler(this.myMouse);
            // 
            // create_primitive
            // 
            this.create_primitive.Location = new System.Drawing.Point(614, 12);
            this.create_primitive.Name = "create_primitive";
            this.create_primitive.Size = new System.Drawing.Size(104, 38);
            this.create_primitive.TabIndex = 2;
            this.create_primitive.Text = "Создать параллелограмм";
            this.create_primitive.UseVisualStyleBackColor = true;
            this.create_primitive.Click += new System.EventHandler(this.btn_create_primitive);
            // 
            // RenderTime
            // 
            this.RenderTime.Enabled = true;
            this.RenderTime.Interval = 30;
            this.RenderTime.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(767, 444);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "x:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(767, 457);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "y:";
            // 
            // label_x_cord
            // 
            this.label_x_cord.AutoSize = true;
            this.label_x_cord.Location = new System.Drawing.Point(788, 444);
            this.label_x_cord.Name = "label_x_cord";
            this.label_x_cord.Size = new System.Drawing.Size(36, 13);
            this.label_x_cord.TabIndex = 5;
            this.label_x_cord.Text = "x cord";
            // 
            // label_y_cord
            // 
            this.label_y_cord.AutoSize = true;
            this.label_y_cord.Location = new System.Drawing.Point(788, 457);
            this.label_y_cord.Name = "label_y_cord";
            this.label_y_cord.Size = new System.Drawing.Size(36, 13);
            this.label_y_cord.TabIndex = 6;
            this.label_y_cord.Text = "y cord";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(640, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 39);
            this.label3.TabIndex = 8;
            this.label3.Text = "Скорость\r\nповорота\r\nфигуры";
            // 
            // barRotatingSpeed
            // 
            this.barRotatingSpeed.LargeChange = 1;
            this.barRotatingSpeed.Location = new System.Drawing.Point(643, 153);
            this.barRotatingSpeed.Maximum = 100;
            this.barRotatingSpeed.Minimum = 1;
            this.barRotatingSpeed.Name = "barRotatingSpeed";
            this.barRotatingSpeed.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.barRotatingSpeed.Size = new System.Drawing.Size(45, 304);
            this.barRotatingSpeed.TabIndex = 9;
            this.barRotatingSpeed.Value = 56;
            this.barRotatingSpeed.Scroll += new System.EventHandler(this.rotatingSpeed);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(701, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 39);
            this.label4.TabIndex = 10;
            this.label4.Text = "Скорость\r\nдвижения\r\nфигуры";
            // 
            // barMoveSpeed
            // 
            this.barMoveSpeed.LargeChange = 1;
            this.barMoveSpeed.Location = new System.Drawing.Point(704, 153);
            this.barMoveSpeed.Minimum = 1;
            this.barMoveSpeed.Name = "barMoveSpeed";
            this.barMoveSpeed.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.barMoveSpeed.Size = new System.Drawing.Size(45, 304);
            this.barMoveSpeed.TabIndex = 11;
            this.barMoveSpeed.Value = 6;
            this.barMoveSpeed.Scroll += new System.EventHandler(this.moveSpeed);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(726, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 38);
            this.button1.TabIndex = 12;
            this.button1.Text = "Задать произвольно";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.enableInput);
            // 
            // info_label
            // 
            this.info_label.AutoSize = true;
            this.info_label.BackColor = System.Drawing.SystemColors.Window;
            this.info_label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.info_label.Location = new System.Drawing.Point(12, 9);
            this.info_label.Name = "info_label";
            this.info_label.Size = new System.Drawing.Size(173, 13);
            this.info_label.TabIndex = 13;
            this.info_label.Text = "Красная точка - центр вращения";
            // 
            // center_label
            // 
            this.center_label.AutoSize = true;
            this.center_label.BackColor = System.Drawing.SystemColors.Window;
            this.center_label.Location = new System.Drawing.Point(191, 9);
            this.center_label.Name = "center_label";
            this.center_label.Size = new System.Drawing.Size(31, 13);
            this.center_label.TabIndex = 14;
            this.center_label.Text = "(0, 0)";
            // 
            // exeption_label
            // 
            this.exeption_label.AutoSize = true;
            this.exeption_label.BackColor = System.Drawing.SystemColors.Window;
            this.exeption_label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.exeption_label.Location = new System.Drawing.Point(12, 485);
            this.exeption_label.Name = "exeption_label";
            this.exeption_label.Size = new System.Drawing.Size(21, 13);
            this.exeption_label.TabIndex = 15;
            this.exeption_label.Text = "Ok";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 520);
            this.Controls.Add(this.exeption_label);
            this.Controls.Add(this.center_label);
            this.Controls.Add(this.info_label);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.barMoveSpeed);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.barRotatingSpeed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label_y_cord);
            this.Controls.Add(this.label_x_cord);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.create_primitive);
            this.Controls.Add(this.field);
            this.MinimumSize = new System.Drawing.Size(831, 549);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barRotatingSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barMoveSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Tao.Platform.Windows.SimpleOpenGlControl field;
        private System.Windows.Forms.Button create_primitive;
        private System.Windows.Forms.Timer RenderTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_x_cord;
        private System.Windows.Forms.Label label_y_cord;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar barRotatingSpeed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar barMoveSpeed;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label info_label;
        private System.Windows.Forms.Label center_label;
        private System.Windows.Forms.Label exeption_label;
    }
}

