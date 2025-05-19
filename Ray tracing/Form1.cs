using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Work_3
{
    public partial class Form1 : Form
    {
        private GLControl glControl;
        private View view = new View();

        public Form1()
        {
            InitializeComponent();
            InitializeGLControl();
        }

        private void InitializeGLControl()
        {
            glControl = new GLControl();
            glControl.Dock = DockStyle.Fill;
            this.Controls.Add(glControl);

            glControl.Load += GlControl_Load;
            glControl.Paint += GlControl_Paint;
            glControl.Resize += GlControl_Resize;
        }

        private void GlControl_Load(object sender, EventArgs e)
        {
            try
            {
                float aspect = (float)glControl.Width / glControl.Height;
                view.InitShaders(aspect);
                GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}");
            }
        }

        private void GlControl_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                view?.Draw();
                glControl.SwapBuffers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отрисовки: {ex.Message}");
            }
        }

        private void GlControl_Resize(object sender, EventArgs e)
        {
            if (glControl.ClientSize.Height == 0)
                glControl.ClientSize = new System.Drawing.Size(glControl.ClientSize.Width, 1);

            GL.Viewport(0, 0, glControl.ClientSize.Width, glControl.ClientSize.Height);

            try
            {
                float aspect = (float)glControl.Width / glControl.Height;
                GL.UseProgram(view.ProgramID);
                int aspectLoc = GL.GetUniformLocation(view.ProgramID, "aspect");
                if (aspectLoc != -1)
                {
                    GL.Uniform1(aspectLoc, aspect);
                }
                glControl.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении размера: {ex.Message}");
            }
        }
    }
}