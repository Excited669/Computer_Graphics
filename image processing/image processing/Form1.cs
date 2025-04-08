using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Filters_Peryashkin.Filters;
using Filters_Peryashkin;

namespace image_processing
{
    public partial class Form1 : Form
    {

        Bitmap image;
        public Form1()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files|*.png;*.jpg;*.bmp|All files(*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // создаём копию, чтобы не было конфликта доступа
                using (var temp = new Bitmap(dialog.FileName))
                {
                    image = new Bitmap(temp);
                }

                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
        }


        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                Filters filter = new InvertFilter();
                backgroundWorker1.RunWorkerAsync(filter); 
            }
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap newImage = ((Filters)e.Argument).processImage(image, backgroundWorker1);
            if (backgroundWorker1.CancellationPending != true)
           
                image = newImage;
            
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
            progressBar1.Value = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void размытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BlurFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void размытиеПоГауссуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GaussFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GrayScaleFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void сепияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new SepiaFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void яркостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BrightnessFilter(50);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void фильтрСобеляToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new SobelFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void резкостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new SharpenFilter();
            backgroundWorker1.RunWorkerAsync(filter);

        }

        private void тиснениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new EmbossFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void переносToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new TranslationFilter(50, 30);
            backgroundWorker1.RunWorkerAsync(filter);

        }

        private void поворотToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new RotationFilter(45); 
            backgroundWorker1.RunWorkerAsync(filter);

        }

        private void серыйМирToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GrayWorldFilter(image);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void размытиеВДвиженииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new MotionBlurFilter();
            backgroundWorker1.RunWorkerAsync(filter); 
        }

        private void эффектСтеклаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GlassFilter();
            backgroundWorker1.RunWorkerAsync(filter);

        }

        private void dilationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[,] mask = {
        { true, true, true },
        { true, true, true },
        { true, true, true }
    };

            Filters filter = new Dilation(mask);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void erosionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[,] mask = {
        { true, true, true },
        { true, true, true },
        { true, true, true }
    };

            Filters filter = new Erosion(mask);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void openingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[,] mask = {
        { true, true, true },
        { true, true, true },
        { true, true, true }
    };

            Filters filter = new Opening(mask);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void closingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[,] mask = {
        { true, true, true },
        { true, true, true },
        { true, true, true }
    };

            Filters filter = new Closing(mask);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void topHatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[,] mask = {
        { true, true, true },
        { true, true, true },
        { true, true, true }
    };

            Filters filter = new TopHat(mask);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void медианныйФильтрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new MedianFilter(1); 
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void светящиесяКраяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GlowingEdgesFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }
        private void SaveImage(string filePath)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap imageToSave = new Bitmap(pictureBox1.Image);
                try
                {
                    imageToSave.Save(filePath, System.Drawing.Imaging.ImageFormat.Png); 
                    MessageBox.Show("Изображение успешно сохранено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении изображения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            
            saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";

    
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
              
                string filePath = saveFileDialog.FileName;

    
                SaveImage(filePath);
            }
        }

        private void линейноеРастяженеиГистограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new LinearHistogramStretchFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }
    }
}
