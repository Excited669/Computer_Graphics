using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace Visual_of_Tomograms
{
    public partial class Form1 : Form
    {
        private bool loaded = false;
        private bool useTexture = false;

        private int currentLayer = 0;
        private View view = new View();
        private Stopwatch stopwatch = new Stopwatch();

        int FrameCount;
        DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);
        void displayFPS()
        {
            if (DateTime.Now >= NextFPSUpdate)
            {
                this.Text = String.Format("CT Visualizer (fps = {0})", FrameCount);
                NextFPSUpdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }
            FrameCount++;
        }

        public Form1()
        {
            InitializeComponent();
            glControl1.Paint += glControl1_Paint;
            Application.Idle += Application_Idle;
            stopwatch.Start();
            NextFPSUpdate = DateTime.Now.AddSeconds(1);
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.FileName;
                Bin.readBIN(path);
                view.SetupView(glControl1.Width, glControl1.Height);
                loaded = true;
                glControl1.Invalidate();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
            needReload = true;
        }

        bool needReload = false;
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            // Начнем с очистки экрана
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Проверка на ошибки OpenGL перед отрисовкой
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("OpenGL Error: " + error.ToString());
            }

            // Если мы загрузили данные
            if (loaded)
            {
                if (needReload)
                {
                    view.generateTextureImage(currentLayer);
                    view.Load2DTexture();
                    needReload = false;
                }

                // Отображение изображения через текстуру
                if (useTexture)
                {
                    view.DrawTexture();
                }
                else
                {
                    view.DrawQuads(currentLayer);
                }

                // Отображение количества кадров (FPS)
                glControl1.SwapBuffers();
                FrameCount++;
                if (DateTime.Now >= NextFPSUpdate)
                {
                    displayFPS();
                    NextFPSUpdate = DateTime.Now.AddSeconds(1);
                    FrameCount = 0;
                }
            }
        }


        private void Application_Idle(object sender, EventArgs e)
        {
            if (loaded)
            {
                displayFPS();
                glControl1.Invalidate();
            }
        }

        class Bin
        {
            public static int X, Y, Z;
            public static short[] array;

            public static void readBIN(string path)
            {
                if (File.Exists(path))
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                    {
                        X = reader.ReadInt32();
                        Y = reader.ReadInt32();
                        Z = reader.ReadInt32();

                        int arraySize = X * Y * Z;
                        array = new short[arraySize];

                        // Выводим первые несколько значений
                        Console.WriteLine("Array Size: " + arraySize);
                        for (int i = 0; i < 10 && i < arraySize; i++)
                        {
                            Console.WriteLine("Array[" + i + "]: " + array[i]);
                        }

                        for (int i = 0; i < arraySize; ++i)
                        {
                            array[i] = reader.ReadInt16();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Файл не найден!");
                }
            }
        }

        class View
        {
            private int tfMin = 0;
            private int tfWidth = 2000;

            public void SetTF(int min, int width)
            {
                tfMin = min;
                tfWidth = width;
            }

            public void SetupView(int width, int height)
            {
                GL.ShadeModel(ShadingModel.Smooth);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                GL.Ortho(0, Bin.X, Bin.Y, 0, -1, 1); // Обновлено для правильной ориентации
                GL.Viewport(0, 0, width, height);
            }

            private Color TransferFunction(short value)
            {
                int tfMax = tfMin + tfWidth;
                int newVal = Math.Clamp((value - tfMin) * 255 / (tfMax - tfMin), 0, 255);
                return Color.FromArgb(255, newVal, newVal, newVal);
            }

            public void DrawQuads(int layerNumber)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                GL.Begin(BeginMode.Quads);

                for (int x = 0; x < Bin.X - 1; x++)
                {
                    for (int y = 0; y < Bin.Y - 1; y++)
                    {
                        int offset = layerNumber * Bin.X * Bin.Y;

                        short val1 = Bin.array[x + y * Bin.X + offset];
                        short val2 = Bin.array[x + (y + 1) * Bin.X + offset];
                        short val3 = Bin.array[(x + 1) + (y + 1) * Bin.X + offset];
                        short val4 = Bin.array[(x + 1) + y * Bin.X + offset];

                        GL.Color3(TransferFunction(val1));
                        GL.Vertex2(x, y);

                        GL.Color3(TransferFunction(val2));
                        GL.Vertex2(x, y + 1);

                        GL.Color3(TransferFunction(val3));
                        GL.Vertex2(x + 1, y + 1);

                        GL.Color3(TransferFunction(val4));
                        GL.Vertex2(x + 1, y);
                    }
                }

                GL.End();
            }

            Bitmap textureImage;
            int VBOtexture;

            public void generateTextureImage(int layerNumber)
            {
                textureImage = new Bitmap(Bin.X, Bin.Y);

                for (int i = 0; i < Bin.X; i++)
                {
                    for (int j = 0; j < Bin.Y; j++)
                    {
                        int pixelNumber = i + j * Bin.X + layerNumber * Bin.X * Bin.Y;
                        textureImage.SetPixel(i, j, TransferFunction(Bin.array[pixelNumber]));
                    }
                }
            }

            public void DrawTexture()
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, VBOtexture);

                GL.Begin(BeginMode.Quads);
                GL.Color3(Color.White);

                GL.TexCoord2(0f, 0f);
                GL.Vertex2(0, 0);

                GL.TexCoord2(0f, 1f);
                GL.Vertex2(0, 256);

                GL.TexCoord2(1f, 1f);
                GL.Vertex2(256, 256);

                GL.TexCoord2(1f, 0f);
                GL.Vertex2(256, 0);

                GL.End();

                GL.Disable(EnableCap.Texture2D);
            }

            public void Load2DTexture()
            {
                if (VBOtexture == 0)
                {
                    GL.GenTextures(1, out VBOtexture);
                    Console.WriteLine("Texture ID: " + VBOtexture);
                }

                GL.BindTexture(TextureTarget.Texture2D, VBOtexture);

                BitmapData data = textureImage.LockBits(
                    new System.Drawing.Rectangle(0, 0, textureImage.Width, textureImage.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                    data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    PixelType.UnsignedByte, data.Scan0);

                textureImage.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                    (int)TextureMagFilter.Linear);

                // Проверка на ошибки OpenGL
                ErrorCode error = GL.GetError();
                if (error != ErrorCode.NoError)
                {
                    MessageBox.Show("OpenGL Error: " + error.ToString());
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trackBarTFMIN.Minimum = 0;
            trackBarTFMIN.Maximum = 2000;
            trackBarTFMIN.Value = 0;

            trackBarTFWIDTH.Minimum = 1;
            trackBarTFWIDTH.Maximum = 2000;
            trackBarTFWIDTH.Value = 2000;

            view.SetTF(trackBarTFMIN.Value, trackBarTFWIDTH.Value);
        }

        private void QUADS_CheckedChanged(object sender, EventArgs e)
        {
            useTexture = false;
            glControl1.Invalidate();
        }

        private void Texture_CheckedChanged(object sender, EventArgs e)
        {
            useTexture = true;
            glControl1.Invalidate();
        }

        private void trackBarTFMIN_Scroll(object sender, EventArgs e)
        {
            view.SetTF(trackBarTFMIN.Value, trackBarTFWIDTH.Value);
            if (useTexture)
                needReload = true;
            glControl1.Invalidate();
        }

        private void trackBarTFWIDTH_Scroll(object sender, EventArgs e)
        {
            view.SetTF(trackBarTFMIN.Value, trackBarTFWIDTH.Value);
            if (useTexture)
                needReload = true;
            glControl1.Invalidate();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
