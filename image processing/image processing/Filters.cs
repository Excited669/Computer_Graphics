using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.ComponentModel;

namespace Filters_Peryashkin
{
    public abstract class Filters
    {
        protected abstract Color calculateNewPixelColor(Bitmap sourceImage, int x, int y);

        public virtual Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Bitmap sourceCopy = new Bitmap(sourceImage);

            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress(Math.Min(100, (int)((float)i / (sourceImage.Width - 1) * 100)));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceCopy, i, j));
                }
            }
            return resultImage;
        }

        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
        protected int GetIntensity(Color color)
        {
            return (int)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B);
        }
    }
    public class GrayWorldFilter : Filters
    {
        private float avgR, avgG, avgB;

        public GrayWorldFilter(Bitmap image)
        {
            float sumR = 0, sumG = 0, sumB = 0;
            int width = image.Width;
            int height = image.Height;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color color = image.GetPixel(x, y);
                    sumR += color.R;
                    sumG += color.G;
                    sumB += color.B;
                }
            }

            int pixelCount = width * height;
            avgR = sumR / pixelCount;
            avgG = sumG / pixelCount;
            avgB = sumB / pixelCount;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color = sourceImage.GetPixel(x, y);
            float avg = (avgR + avgG + avgB) / 3;

            int newR = Clamp((int)(color.R * (avg / avgR)), 0, 255);
            int newG = Clamp((int)(color.G * (avg / avgG)), 0, 255);
            int newB = Clamp((int)(color.B * (avg / avgB)), 0, 255);

            return Color.FromArgb(newR, newG, newB);
        }
    }
    public class LinearHistogramStretchFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
 
            return sourceImage.GetPixel(x, y);
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            int width = sourceImage.Width;
            int height = sourceImage.Height;

            int rMin = 255, rMax = 0;
            int gMin = 255, gMax = 0;
            int bMin = 255, bMax = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color c = sourceImage.GetPixel(x, y);

                    if (c.R < rMin) rMin = c.R;
                    if (c.R > rMax) rMax = c.R;

                    if (c.G < gMin) gMin = c.G;
                    if (c.G > gMax) gMax = c.G;

                    if (c.B < bMin) bMin = c.B;
                    if (c.B > bMax) bMax = c.B;
                }
            }

            Bitmap result = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                if (worker != null && worker.WorkerReportsProgress)
                    worker.ReportProgress((int)((float)y / height * 100));

                for (int x = 0; x < width; x++)
                {
                    Color c = sourceImage.GetPixel(x, y);

                    int r = (c.R - rMin) * 255 / Math.Max(1, rMax - rMin);
                    int g = (c.G - gMin) * 255 / Math.Max(1, gMax - gMin);
                    int b = (c.B - bMin) * 255 / Math.Max(1, bMax - bMin);

                    r = Clamp(r, 0, 255);
                    g = Clamp(g, 0, 255);
                    b = Clamp(b, 0, 255);

                    result.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return result;
        }
    }




    class InvertFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(255 - sourceColor.R, 255 - sourceColor.G, 255 - sourceColor.B);
            return resultColor;
        }
    }
    public class MatrixFilter : Filters
    {
        protected float[,] kernel = null;
        protected MatrixFilter() { }
        public MatrixFilter(float[,] kernel)
        {
            this.kernel = kernel;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            float resultR = 0;
            float resultG = 0;
            float resultB = 0;

            for (int l = -radiusY; l <= radiusY; l++)
            {
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    resultR += neighborColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += neighborColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += neighborColor.B * kernel[k + radiusX, l + radiusY];
                }
            }

            return Color.FromArgb(
                Clamp((int)resultR, 0, 255),
                Clamp((int)resultG, 0, 255),
                Clamp((int)resultB, 0, 255)
            );
        }
    }
    class BlurFilter : MatrixFilter
    {
        public BlurFilter() 
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            for(int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    kernel[i, j] = 1.0f / (float)(sizeX * sizeY);
                }
            }
        }
    }
    class GaussFilter : MatrixFilter
    {
        public void createGaussianKernel(int radius,float sigma)
        {
            int size = radius * 2 + 1;
            kernel = new float[size, size];
            float norm = 0;
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    kernel[i + radius, j + radius] = (float)(Math.Exp(-(i * i + j * j) / (2 * sigma * sigma)) / (2 * sigma * sigma));
                    norm += kernel[i + radius, j + radius];
                }
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    kernel[i, j] /= norm;
                }
            }
        }
        public GaussFilter()
        {
            createGaussianKernel(3,2);
        }
    }
    class GrayScaleFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int gray = GetIntensity(sourceImage.GetPixel(x, y));
            return Color.FromArgb(gray, gray, gray);
        }
    }
    class SepiaFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int intensity = GetIntensity(sourceColor);
            int k = 30;

            int r = Clamp(intensity + 2 * k, 0, 255);
            int g = Clamp(intensity + k, 0, 255);
            int b = Clamp(intensity, 0, 255);

            return Color.FromArgb(r, g, b);
        }
    }
    class BrightnessFilter : Filters
    {
        private int brightness; 

        public BrightnessFilter(int brightness)
        {
            this.brightness = brightness;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);

            int r = Clamp(sourceColor.R + brightness, 0, 255);
            int g = Clamp(sourceColor.G + brightness, 0, 255);
            int b = Clamp(sourceColor.B + brightness, 0, 255);

            return Color.FromArgb(r, g, b);
        }
    }
    class SobelFilter : MatrixFilter
    {
        public SobelFilter() : base(new float[,]
        {
        { -1, 0, 1 },
        { -2, 0, 2 },
        { -1, 0, 1 }
        })
        { }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {

            float[,] gx = new float[,]
            {
            { -1, 0, 1 },
            { -2, 0, 2 },
            { -1, 0, 1 }
            };

            float[,] gy = new float[,]
            {
            {  1,  2,  1 },
            {  0,  0,  0 },
            { -1, -2, -1 }
            };
            float gradientX = 0;
            float gradientY = 0;

            int radiusX = gx.GetLength(0) / 2;
            int radiusY = gx.GetLength(1) / 2;

            for (int i = -radiusY; i <= radiusY; i++)
            {
                for (int j = -radiusX; j <= radiusX; j++)
                {
                    int idX = Clamp(x + j, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + i, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);

                    gradientX += neighborColor.R * gx[j + radiusX, i + radiusY];
                    gradientY += neighborColor.R * gy[j + radiusX, i + radiusY];
                }
            }
            float gradient = (float)Math.Sqrt(gradientX * gradientX + gradientY * gradientY);
            int finalColor = Clamp((int)gradient, 0, 255);

            return Color.FromArgb(finalColor, finalColor, finalColor);
        }
    }
    class SharpenFilter : MatrixFilter
    {
        public SharpenFilter() : base(new float[,]
        {
        { -1, -1, -1 },
        { -1,  9, -1 },
        { -1, -1, -1 }
        })
        { }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            float resultR = 0, resultG = 0, resultB = 0;
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            for (int i = -radiusY; i <= radiusY; i++)
            {
                for (int j = -radiusX; j <= radiusX; j++)
                {
                    int idX = Clamp(x + j, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + i, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);

                    resultR += neighborColor.R * kernel[j + radiusX, i + radiusY];
                    resultG += neighborColor.G * kernel[j + radiusX, i + radiusY];
                    resultB += neighborColor.B * kernel[j + radiusX, i + radiusY];
                }
            }

            int finalR = Clamp((int)resultR, 0, 255);
            int finalG = Clamp((int)resultG, 0, 255);
            int finalB = Clamp((int)resultB, 0, 255);

            return Color.FromArgb(finalR, finalG, finalB);
        }
    }
    class EmbossFilter : MatrixFilter
    {
        public EmbossFilter() : base(new float[,]
        {
        {  1,  1,  1 },
        {  1, -7,  1 },
        {  1,  1,  1 }
        })
        { }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            float resultR = 0, resultG = 0, resultB = 0;
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            for (int i = -radiusY; i <= radiusY; i++)
            {
                for (int j = -radiusX; j <= radiusX; j++)
                {
                    int idX = Clamp(x + j, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + i, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);

                    resultR += neighborColor.R * kernel[j + radiusX, i + radiusY];
                    resultG += neighborColor.G * kernel[j + radiusX, i + radiusY];
                    resultB += neighborColor.B * kernel[j + radiusX, i + radiusY];
                }
            }

    
            int finalR = Clamp((int)resultR, 0, 255);
            int finalG = Clamp((int)resultG, 0, 255);
            int finalB = Clamp((int)resultB, 0, 255);

            return Color.FromArgb(finalR, finalG, finalB);
        }
    }
    class TranslationFilter : Filters
    {
        private int dx, dy;

        public TranslationFilter(int dx, int dy)
        {
            this.dx = dx;
            this.dy = dy;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int newX = Clamp(x + dx, 0, sourceImage.Width - 1);
            int newY = Clamp(y + dy, 0, sourceImage.Height - 1);

            return sourceImage.GetPixel(newX, newY);
        }
    }
    class RotationFilter : Filters
    {
        private float angle;

        public RotationFilter(float angle)
        {
            this.angle = angle;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
       
            float radians = angle * (float)Math.PI / 180.0f;

 
            int centerX = sourceImage.Width / 2;
            int centerY = sourceImage.Height / 2;


            int newX = (int)(Math.Cos(radians) * (x - centerX) - Math.Sin(radians) * (y - centerY) + centerX);
            int newY = (int)(Math.Sin(radians) * (x - centerX) + Math.Cos(radians) * (y - centerY) + centerY);


            newX = Clamp(newX, 0, sourceImage.Width - 1);
            newY = Clamp(newY, 0, sourceImage.Height - 1);

            return sourceImage.GetPixel(newX, newY);
        }
    }
    public class MotionBlurFilter : MatrixFilter
    {
        public MotionBlurFilter(int size = 5)
        {
            kernel = new float[size, size];

            for (int i = 0; i < size; i++)
            {
                kernel[i, i] = 1.0f / size;
            }
        }
    }
    public class GlassFilter : Filters
    {
        private Random rand = new Random();

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int k = 5; 

            int newX = Clamp(x + rand.Next(-k, k + 1), 0, sourceImage.Width - 1);
            int newY = Clamp(y + rand.Next(-k, k + 1), 0, sourceImage.Height - 1);

            return sourceImage.GetPixel(newX, newY);
        }
    }
    public abstract class MorphologyFilter : Filters
    {
        // Сделаем маску публичным свойством
        public bool[,] Mask { get; protected set; }
        protected int radiusX;
        protected int radiusY;

        public MorphologyFilter(bool[,] mask)
        {
            this.Mask = mask;
            radiusX = mask.GetLength(0) / 2;
            radiusY = mask.GetLength(1) / 2;
        }

        // Метод для изменения маски
        public void SetMask(bool[,] newMask)
        {
            Mask = newMask;
            radiusX = newMask.GetLength(0) / 2;
            radiusY = newMask.GetLength(1) / 2;
        }
    }

    public class Dilation : MorphologyFilter
    {
        public Dilation(bool[,] mask) : base(mask) { }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int maxR = 0, maxG = 0, maxB = 0;

            for (int i = -radiusY; i <= radiusY; i++)
            {
                for (int j = -radiusX; j <= radiusX; j++)
                {
                    int nx = Clamp(x + j, 0, sourceImage.Width - 1);
                    int ny = Clamp(y + i, 0, sourceImage.Height - 1);

                    if (Mask[j + radiusX, i + radiusY]) // Используем Mask
                    {
                        Color neighbor = sourceImage.GetPixel(nx, ny);
                        maxR = Math.Max(maxR, neighbor.R);
                        maxG = Math.Max(maxG, neighbor.G);
                        maxB = Math.Max(maxB, neighbor.B);
                    }
                }
            }

            return Color.FromArgb(maxR, maxG, maxB);
        }
    }

    public class Erosion : MorphologyFilter
    {
        public Erosion(bool[,] mask) : base(mask) { }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int minR = 255, minG = 255, minB = 255;

            for (int i = -radiusY; i <= radiusY; i++)
            {
                for (int j = -radiusX; j <= radiusX; j++)
                {
                    int nx = Clamp(x + j, 0, sourceImage.Width - 1);
                    int ny = Clamp(y + i, 0, sourceImage.Height - 1);

                    if (Mask[j + radiusX, i + radiusY]) // Используем Mask
                    {
                        Color neighbor = sourceImage.GetPixel(nx, ny);
                        minR = Math.Min(minR, neighbor.R);
                        minG = Math.Min(minG, neighbor.G);
                        minB = Math.Min(minB, neighbor.B);
                    }
                }
            }

            return Color.FromArgb(minR, minG, minB);
        }
    }

    public class Opening : Filters
    {
        private MorphologyFilter morphologyFilter;

        public Opening(bool[,] mask)
        {
            morphologyFilter = new Erosion(mask); // можно заменить на любой фильтр
        }

        public void ChangeMask(bool[,] newMask)
        {
            morphologyFilter.SetMask(newMask);
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return Color.Black;
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            var eroded = morphologyFilter.processImage(sourceImage, worker);
            var dilation = new Dilation(morphologyFilter.Mask); // Если маска изменилась
            return dilation.processImage(eroded, worker);
        }
    }

    public class Closing : Filters
    {
        private MorphologyFilter morphologyFilter;

        public Closing(bool[,] mask)
        {
            morphologyFilter = new Dilation(mask); // можно заменить на любой фильтр
        }

        public void ChangeMask(bool[,] newMask)
        {
            morphologyFilter.SetMask(newMask);
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return Color.Black;
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            var dilated = morphologyFilter.processImage(sourceImage, worker);
            var erosion = new Erosion(morphologyFilter.Mask); // Если маска изменилась
            return erosion.processImage(dilated, worker);
        }
    }

    public class TopHat : Filters
    {
        private bool[,] mask;

        public TopHat(bool[,] mask)
        {
            this.mask = mask;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return Color.Black;
        }

        public void ChangeMask(bool[,] newMask)
        {
            mask = newMask;
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Opening opening = new Opening(mask);
            Bitmap opened = opening.processImage(sourceImage, worker);

            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * 100));
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color orig = sourceImage.GetPixel(i, j);
                    Color op = opened.GetPixel(i, j);

                    int r = Clamp(orig.R - op.R, 0, 255);
                    int g = Clamp(orig.G - op.G, 0, 255);
                    int b = Clamp(orig.B - op.B, 0, 255);

                    result.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }

            return result;
        }
    }


    class MedianFilter : Filters
    {
        private int radius;

        public MedianFilter(int radius = 1)
        {
            this.radius = radius;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            List<int> rValues = new List<int>();
            List<int> gValues = new List<int>();
            List<int> bValues = new List<int>();

            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    int newX = Clamp(x + i, 0, sourceImage.Width - 1);
                    int newY = Clamp(y + j, 0, sourceImage.Height - 1);

                    Color neighborColor = sourceImage.GetPixel(newX, newY);

                    rValues.Add(neighborColor.R);
                    gValues.Add(neighborColor.G);
                    bValues.Add(neighborColor.B);
                }
            }

            rValues.Sort();
            gValues.Sort();
            bValues.Sort();

            int middle = rValues.Count / 2;
            return Color.FromArgb(rValues[middle], gValues[middle], bValues[middle]);
        }
    }
    class MaxFilter : Filters
    {
        private int size;

        public MaxFilter(int size)
        {
            this.size = size;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            List<int> rValues = new List<int>();
            List<int> gValues = new List<int>();
            List<int> bValues = new List<int>();

            int radius = size / 2;

            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    int newX = Clamp(x + i, 0, sourceImage.Width - 1);
                    int newY = Clamp(y + j, 0, sourceImage.Height - 1);

                    Color color = sourceImage.GetPixel(newX, newY);
                    rValues.Add(color.R);
                    gValues.Add(color.G);
                    bValues.Add(color.B);
                }
            }

            int rMax = rValues.Max();
            int gMax = gValues.Max();
            int bMax = bValues.Max();

            return Color.FromArgb(rMax, gMax, bMax);
        }
    }
    public class GlowingEdgesFilter : Filters
    {
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Filters medianFilter = new MedianFilter(3);
            Bitmap result = medianFilter.processImage(sourceImage, worker);

            Filters sobelFilter = new SobelFilter();
            result = sobelFilter.processImage(result, worker);

            Filters maxFilter = new MaxFilter(3);
            result = maxFilter.processImage(result, worker);

            return result;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return Color.Black;
        }
    }

}