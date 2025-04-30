namespace Tomogram_visualizer
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.glControl1 = new OpenTK.GLControl();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.trackBarMin = new System.Windows.Forms.TrackBar();
            this.trackBarWidth = new System.Windows.Forms.TrackBar();
            this.radioButtonStandart = new System.Windows.Forms.RadioButton();
            this.radioButtonImproved = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(856, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.открытьToolStripMenuItem.Text = "Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // glControl1
            // 
            this.glControl1.AutoSize = true;
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(11, 186);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(833, 440);
            this.glControl1.TabIndex = 1;
            this.glControl1.VSync = false;
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(11, 38);
            this.trackBar1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(658, 45);
            this.trackBar1.TabIndex = 2;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll_1);
            // 
            // trackBarMin
            // 
            this.trackBarMin.Location = new System.Drawing.Point(11, 87);
            this.trackBarMin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.trackBarMin.Name = "trackBarMin";
            this.trackBarMin.Size = new System.Drawing.Size(658, 45);
            this.trackBarMin.TabIndex = 5;
            this.trackBarMin.Scroll += new System.EventHandler(this.trackBarMin_Scroll);
            // 
            // trackBarWidth
            // 
            this.trackBarWidth.Location = new System.Drawing.Point(11, 136);
            this.trackBarWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.trackBarWidth.Name = "trackBarWidth";
            this.trackBarWidth.Size = new System.Drawing.Size(658, 45);
            this.trackBarWidth.TabIndex = 6;
            this.trackBarWidth.Scroll += new System.EventHandler(this.trackBarWidth_Scroll);
            // 
            // radioButtonStandart
            // 
            this.radioButtonStandart.AutoSize = true;
            this.radioButtonStandart.Location = new System.Drawing.Point(753, 26);
            this.radioButtonStandart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioButtonStandart.Name = "radioButtonStandart";
            this.radioButtonStandart.Size = new System.Drawing.Size(97, 17);
            this.radioButtonStandart.TabIndex = 7;
            this.radioButtonStandart.TabStop = true;
            this.radioButtonStandart.Text = "Quads standart";
            this.radioButtonStandart.UseVisualStyleBackColor = true;
            // 
            // radioButtonImproved
            // 
            this.radioButtonImproved.AutoSize = true;
            this.radioButtonImproved.Location = new System.Drawing.Point(753, 47);
            this.radioButtonImproved.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioButtonImproved.Name = "radioButtonImproved";
            this.radioButtonImproved.Size = new System.Drawing.Size(89, 17);
            this.radioButtonImproved.TabIndex = 8;
            this.radioButtonImproved.TabStop = true;
            this.radioButtonImproved.Text = "Quads modify";
            this.radioButtonImproved.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(687, 38);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(62, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Texture";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(684, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Min";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(684, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Width";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 638);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButtonImproved);
            this.Controls.Add(this.radioButtonStandart);
            this.Controls.Add(this.trackBarWidth);
            this.Controls.Add(this.trackBarMin);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TrackBar trackBarMin;
        private System.Windows.Forms.TrackBar trackBarWidth;
        private System.Windows.Forms.RadioButton radioButtonStandart;
        private System.Windows.Forms.RadioButton radioButtonImproved;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

