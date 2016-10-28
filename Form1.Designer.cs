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
            this.btnCreatePrimitive = new System.Windows.Forms.Button();
            this.RenderTime = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.barRotatingSpeed = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.barMoveSpeed = new System.Windows.Forms.TrackBar();
            this.btnCreateArbitrarily = new System.Windows.Forms.Button();
            this.exeption_label = new System.Windows.Forms.Label();
            this.cboxCountFigures = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSetColor = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_Delete_Shape = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cboxSelectedType = new System.Windows.Forms.ComboBox();
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
            this.field.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.field.ColorBits = ((byte)(32));
            this.field.DepthBits = ((byte)(16));
            this.field.Location = new System.Drawing.Point(7, 8);
            this.field.Name = "field";
            this.field.Size = new System.Drawing.Size(600, 500);
            this.field.StencilBits = ((byte)(0));
            this.field.TabIndex = 0;
            this.field.Load += new System.EventHandler(this.field_Load);
            this.field.KeyDown += new System.Windows.Forms.KeyEventHandler(this.field_Key_Down);
            this.field.MouseClick += new System.Windows.Forms.MouseEventHandler(this.field_Mouse_Click);
            this.field.MouseMove += new System.Windows.Forms.MouseEventHandler(this.field_Mouse_Move);
            // 
            // btnCreatePrimitive
            // 
            this.btnCreatePrimitive.Location = new System.Drawing.Point(614, 39);
            this.btnCreatePrimitive.Name = "btnCreatePrimitive";
            this.btnCreatePrimitive.Size = new System.Drawing.Size(104, 38);
            this.btnCreatePrimitive.TabIndex = 2;
            this.btnCreatePrimitive.Text = "Создать примитив";
            this.btnCreatePrimitive.UseVisualStyleBackColor = true;
            this.btnCreatePrimitive.Click += new System.EventHandler(this.btn_Create_Primitive);
            // 
            // RenderTime
            // 
            this.RenderTime.Enabled = true;
            this.RenderTime.Interval = 30;
            this.RenderTime.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 613);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Скорость поворота";
            // 
            // barRotatingSpeed
            // 
            this.barRotatingSpeed.LargeChange = 1;
            this.barRotatingSpeed.Location = new System.Drawing.Point(38, 636);
            this.barRotatingSpeed.Minimum = 1;
            this.barRotatingSpeed.Name = "barRotatingSpeed";
            this.barRotatingSpeed.Size = new System.Drawing.Size(304, 45);
            this.barRotatingSpeed.TabIndex = 9;
            this.barRotatingSpeed.Value = 1;
            this.barRotatingSpeed.Scroll += new System.EventHandler(this.rotating_Speed_Change);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 549);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Скорость движения";
            // 
            // barMoveSpeed
            // 
            this.barMoveSpeed.LargeChange = 1;
            this.barMoveSpeed.Location = new System.Drawing.Point(38, 567);
            this.barMoveSpeed.Maximum = 11;
            this.barMoveSpeed.Minimum = 2;
            this.barMoveSpeed.Name = "barMoveSpeed";
            this.barMoveSpeed.Size = new System.Drawing.Size(304, 45);
            this.barMoveSpeed.TabIndex = 11;
            this.barMoveSpeed.Value = 2;
            this.barMoveSpeed.Scroll += new System.EventHandler(this.move_Speed_Change);
            // 
            // btnCreateArbitrarily
            // 
            this.btnCreateArbitrarily.Location = new System.Drawing.Point(614, 83);
            this.btnCreateArbitrarily.Name = "btnCreateArbitrarily";
            this.btnCreateArbitrarily.Size = new System.Drawing.Size(104, 38);
            this.btnCreateArbitrarily.TabIndex = 12;
            this.btnCreateArbitrarily.Text = "Задать произвольно";
            this.btnCreateArbitrarily.UseVisualStyleBackColor = true;
            this.btnCreateArbitrarily.Click += new System.EventHandler(this.enable_Input_Shape);
            // 
            // exeption_label
            // 
            this.exeption_label.AutoSize = true;
            this.exeption_label.BackColor = System.Drawing.SystemColors.Window;
            this.exeption_label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.exeption_label.Location = new System.Drawing.Point(232, 526);
            this.exeption_label.Name = "exeption_label";
            this.exeption_label.Size = new System.Drawing.Size(21, 13);
            this.exeption_label.TabIndex = 15;
            this.exeption_label.Text = "Ok";
            // 
            // cboxCountFigures
            // 
            this.cboxCountFigures.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxCountFigures.FormattingEnabled = true;
            this.cboxCountFigures.Location = new System.Drawing.Point(614, 166);
            this.cboxCountFigures.Name = "cboxCountFigures";
            this.cboxCountFigures.Size = new System.Drawing.Size(104, 21);
            this.cboxCountFigures.TabIndex = 16;
            this.cboxCountFigures.SelectedIndexChanged += new System.EventHandler(this.cbox_Selected_Item_Change);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(613, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 26);
            this.label1.TabIndex = 17;
            this.label1.Text = "Выбранная фигура\r\nдля редактирования";
            // 
            // btnSetColor
            // 
            this.btnSetColor.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSetColor.Location = new System.Drawing.Point(348, 567);
            this.btnSetColor.Name = "btnSetColor";
            this.btnSetColor.Size = new System.Drawing.Size(50, 50);
            this.btnSetColor.TabIndex = 18;
            this.btnSetColor.UseVisualStyleBackColor = false;
            this.btnSetColor.Click += new System.EventHandler(this.btn_Set_Color_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 526);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Свойства фигуры";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(345, 549);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Цвет";
            // 
            // btn_Delete_Shape
            // 
            this.btn_Delete_Shape.Location = new System.Drawing.Point(613, 204);
            this.btn_Delete_Shape.Name = "btn_Delete_Shape";
            this.btn_Delete_Shape.Size = new System.Drawing.Size(104, 47);
            this.btn_Delete_Shape.TabIndex = 21;
            this.btn_Delete_Shape.Text = "Удалить выбранную фигуру";
            this.btn_Delete_Shape.UseVisualStyleBackColor = true;
            this.btn_Delete_Shape.Click += new System.EventHandler(this.btn_Delete_Sel_Shape);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.Window;
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(597, 493);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "x";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.Window;
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.Location = new System.Drawing.Point(9, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "y";
            // 
            // cboxSelectedType
            // 
            this.cboxSelectedType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSelectedType.FormattingEnabled = true;
            this.cboxSelectedType.Items.AddRange(new object[] {
            "Параллелограмм",
            "Пентагон",
            "Эллипс",
            "Ромб"});
            this.cboxSelectedType.Location = new System.Drawing.Point(614, 12);
            this.cboxSelectedType.Name = "cboxSelectedType";
            this.cboxSelectedType.Size = new System.Drawing.Size(104, 21);
            this.cboxSelectedType.TabIndex = 24;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 716);
            this.Controls.Add(this.cboxSelectedType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_Delete_Shape);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSetColor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboxCountFigures);
            this.Controls.Add(this.exeption_label);
            this.Controls.Add(this.btnCreateArbitrarily);
            this.Controls.Add(this.barMoveSpeed);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.barRotatingSpeed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCreatePrimitive);
            this.Controls.Add(this.field);
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
        private System.Windows.Forms.Button btnCreatePrimitive;
        private System.Windows.Forms.Timer RenderTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar barRotatingSpeed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar barMoveSpeed;
        private System.Windows.Forms.Button btnCreateArbitrarily;
        private System.Windows.Forms.Label exeption_label;
        private System.Windows.Forms.ComboBox cboxCountFigures;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSetColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_Delete_Shape;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboxSelectedType;
    }
}

