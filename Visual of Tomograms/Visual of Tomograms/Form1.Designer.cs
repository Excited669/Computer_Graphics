namespace Visual_of_Tomograms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            glControl1 = new OpenTK.GLControl.GLControl();
            trackBar1 = new TrackBar();
            menuStrip1 = new MenuStrip();
            открытьToolStripMenuItem = new ToolStripMenuItem();
            QUADS = new RadioButton();
            Texture = new RadioButton();
            trackBarTFWIDTH = new TrackBar();
            trackBarTFMIN = new TrackBar();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarTFWIDTH).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarTFMIN).BeginInit();
            SuspendLayout();
            // 
            // glControl1
            // 
            glControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            glControl1.APIVersion = new Version(3, 3, 0, 0);
            glControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            glControl1.IsEventDriven = true;
            glControl1.Location = new Point(12, 180);
            glControl1.Name = "glControl1";
            glControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            glControl1.SharedContext = null;
            glControl1.Size = new Size(776, 496);
            glControl1.TabIndex = 0;
            glControl1.Paint += glControl1_Paint;
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(12, 27);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(676, 45);
            trackBar1.TabIndex = 1;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { открытьToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // открытьToolStripMenuItem
            // 
            открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            открытьToolStripMenuItem.Size = new Size(66, 20);
            открытьToolStripMenuItem.Text = "Открыть";
            открытьToolStripMenuItem.Click += открытьToolStripMenuItem_Click;
            // 
            // QUADS
            // 
            QUADS.AutoSize = true;
            QUADS.Location = new Point(694, 27);
            QUADS.Name = "QUADS";
            QUADS.Size = new Size(59, 19);
            QUADS.TabIndex = 3;
            QUADS.TabStop = true;
            QUADS.Text = "Quads";
            QUADS.UseVisualStyleBackColor = true;
            QUADS.CheckedChanged += QUADS_CheckedChanged;
            // 
            // Texture
            // 
            Texture.AutoSize = true;
            Texture.Location = new Point(694, 52);
            Texture.Name = "Texture";
            Texture.Size = new Size(63, 19);
            Texture.TabIndex = 4;
            Texture.TabStop = true;
            Texture.Text = "Texture";
            Texture.UseVisualStyleBackColor = true;
            Texture.CheckedChanged += Texture_CheckedChanged;
            // 
            // trackBarTFWIDTH
            // 
            trackBarTFWIDTH.Location = new Point(12, 78);
            trackBarTFWIDTH.Name = "trackBarTFWIDTH";
            trackBarTFWIDTH.Size = new Size(676, 45);
            trackBarTFWIDTH.TabIndex = 5;
            trackBarTFWIDTH.Scroll += trackBarTFWIDTH_Scroll;
            // 
            // trackBarTFMIN
            // 
            trackBarTFMIN.Location = new Point(12, 129);
            trackBarTFMIN.Name = "trackBarTFMIN";
            trackBarTFMIN.Size = new Size(676, 45);
            trackBarTFMIN.TabIndex = 6;
            trackBarTFMIN.Scroll += trackBarTFMIN_Scroll;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(694, 129);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 7;
            textBox1.Text = "Min";
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(694, 78);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 8;
            textBox2.Text = "Width";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 688);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(trackBarTFMIN);
            Controls.Add(trackBarTFWIDTH);
            Controls.Add(Texture);
            Controls.Add(QUADS);
            Controls.Add(trackBar1);
            Controls.Add(glControl1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarTFWIDTH).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarTFMIN).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private OpenTK.GLControl.GLControl glControl1;
        private TrackBar trackBar1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem открытьToolStripMenuItem;
        private RadioButton QUADS;
        private RadioButton Texture;
        private TrackBar trackBarTFWIDTH;
        private TrackBar trackBarTFMIN;
        private TextBox textBox1;
        private TextBox textBox2;
    }
}
