using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Work_3
{
    class View
    {
        private int vbo_position;
        private int BasicProgramID;
        private int BasicVertexShader;
        private int BasicFragmentShader;
        private int attribute_vpos;
        private int uniform_aspect;

        public int ProgramID => BasicProgramID;

        private Vector3[] vertdata = new Vector3[]
        {
            new Vector3(-1f, -1f, 0f),
            new Vector3(1f, -1f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(-1f, 1f, 0f)
        };

        public void InitShaders(float aspectRatio)
        {
            try
            {
                BasicProgramID = GL.CreateProgram();
                loadShader("C:\\Users\\Серго\\Desktop\\source\\github\\KG\\Work_3\\Shaders\\raytracing.vert",
                         ShaderType.VertexShader, BasicProgramID, out BasicVertexShader);
                loadShader("C:\\Users\\Серго\\Desktop\\source\\github\\KG\\Work_3\\Shaders\\raytracing.frag",
                         ShaderType.FragmentShader, BasicProgramID, out BasicFragmentShader);

                GL.LinkProgram(BasicProgramID);

                int status = 0;
                GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
                Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));

                attribute_vpos = GL.GetAttribLocation(BasicProgramID, "vPosition");
                uniform_aspect = GL.GetUniformLocation(BasicProgramID, "aspect");

                GL.GenBuffers(1, out vbo_position);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
                GL.BufferData(BufferTarget.ArrayBuffer,
                            (IntPtr)(vertdata.Length * Vector3.SizeInBytes),
                            vertdata, BufferUsageHint.StaticDraw);

                GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);
                GL.EnableVertexAttribArray(attribute_vpos);

                GL.UseProgram(BasicProgramID);
                uniform_aspect = GL.GetUniformLocation(BasicProgramID, "aspect");
                if (uniform_aspect != -1)
                {
                    GL.Uniform1(uniform_aspect, aspectRatio);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в InitShaders: {ex.Message}");
                throw;
            }
        }

        private void loadShader(string filename, ShaderType type, int program, out int address)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"Shader file not found: {filename}");
            }

            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);

            string info = GL.GetShaderInfoLog(address);
            if (!string.IsNullOrEmpty(info))
                Console.WriteLine($"GL.CompileShader [{type}] had info log: {info}");

            GL.AttachShader(program, address);
        }

        public void Draw()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(BasicProgramID);
            GL.DrawArrays(PrimitiveType.Quads, 0, 4);
        }
    }
}