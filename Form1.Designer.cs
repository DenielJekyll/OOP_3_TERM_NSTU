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
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // field
            // 
            this.field.AccumBits = ((byte)(0));
            this.field.AutoCheckErrors = false;
            this.field.AutoFinish = false;
            this.field.AutoMakeCurrent = true;
            this.field.AutoSwapBuffers = true;
            this.field.BackColor = System.Drawing.Color.Black;
            this.field.ColorBits = ((byte)(32));
            this.field.DepthBits = ((byte)(16));
            this.field.Location = new System.Drawing.Point(-25, -2);
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
            this.create_primitive.Location = new System.Drawing.Point(616, 12);
            this.create_primitive.Name = "create_primitive";
            this.create_primitive.Size = new System.Drawing.Size(104, 38);
            this.create_primitive.TabIndex = 2;
            this.create_primitive.Text = "Создать параллелограмм";
            this.create_primitive.UseVisualStyleBackColor = true;
            this.create_primitive.Click += new System.EventHandler(this.btn_create_primitive);
            // 
            // RenderTime
            // 
            this.RenderTime.Interval = 30;
            this.RenderTime.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(634, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "x:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(634, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "y:";
            // 
            // label_x_cord
            // 
            this.label_x_cord.AutoSize = true;
            this.label_x_cord.Location = new System.Drawing.Point(655, 53);
            this.label_x_cord.Name = "label_x_cord";
            this.label_x_cord.Size = new System.Drawing.Size(36, 13);
            this.label_x_cord.TabIndex = 5;
            this.label_x_cord.Text = "x cord";
            // 
            // label_y_cord
            // 
            this.label_y_cord.AutoSize = true;
            this.label_y_cord.Location = new System.Drawing.Point(655, 66);
            this.label_y_cord.Name = "label_y_cord";
            this.label_y_cord.Size = new System.Drawing.Size(36, 13);
            this.label_y_cord.TabIndex = 6;
            this.label_y_cord.Text = "y cord";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(613, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 39);
            this.label3.TabIndex = 8;
            this.label3.Text = "Скорость\r\nповорота\r\nфигуры";
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(616, 153);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size(45, 304);
            this.trackBar1.TabIndex = 9;
            this.trackBar1.Value = 1;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 510);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label_y_cord);
            this.Controls.Add(this.label_x_cord);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.create_primitive);
            this.Controls.Add(this.field);
            this.MaximumSize = new System.Drawing.Size(831, 549);
            this.MinimumSize = new System.Drawing.Size(831, 549);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
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
        private System.Windows.Forms.TrackBar trackBar1;
    }
}

