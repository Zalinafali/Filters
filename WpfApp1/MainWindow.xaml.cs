using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Point = System.Windows.Point;

namespace WpfApp1
{

    public class Filter
    {
        public string Name { get; set; }

        public int Size { get; set; }

        public int[,] ConvolutionKernel { get; set; }

        public int ConvolutionFactor { get; set; }

        public Filter(string name, int size, int convolutionFactor)
        {
            Name = name;
            Size = size;
            ConvolutionFactor = convolutionFactor;
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeConvolutionFilters();
            InitializeCanvas();
        }

        bool isImageLoaded = false;
        Bitmap bmp = null;
        Bitmap copyBmp = null;
        int pixelWidth = 0;
        int pixelHeight = 0;


        Dictionary<string, Filter> filters;

        // ---------- OPTIONS ----------

        // fixed parameters for function filters
        const double brightnessParameter = 40.0;
        const double contrastParameter = 20.0;
        const double gammaParameter = 1.5;

        // ---------- ------- ----------

        private void Load_Image(object sender, RoutedEventArgs e)
        {
            ImageInfo imageInfo = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.bmp;*.jpg)|*.png;*.bmp;*.jpg|All files (*.*)|*.*";
            DialogResult result = openFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo file = new FileInfo(openFileDialog.FileName);
                if (file.Extension == ".PNG" || file.Extension == ".JPG" || file.Extension == ".jpg" || file.Extension == ".png" || file.Extension == ".bmp" || file.Extension == ".BMP")
                {
                    Bitmap loadedBmp = new Bitmap(file.FullName);
                    imageInfo = new ImageInfo(file.Name, loadedBmp.Width, loadedBmp.Height, file.Length, file.FullName);

                    pixelWidth = loadedBmp.Width;
                    pixelHeight = loadedBmp.Height;

                    BitmapImage bmImage = Bitmap2BitmapImage(loadedBmp);

                    originalImage.Source = bmImage;
                    filteredImage.Source = bmImage;
                    bmp = loadedBmp;
                    copyBmp = new Bitmap(loadedBmp);

                    ImageLoaded();
                }
            }
        }

        private void ImageLoaded()
        {
            isImageLoaded = true;

            saveImage.IsEnabled = true;
            reverseChanges.IsEnabled = true;

            inverseCheckbox.IsEnabled = true;
            inverseCheckbox.IsChecked = false;
            brightnessCheckbox.IsEnabled = true;
            brightnessCheckbox.IsChecked = false;
            contrastCheckbox.IsEnabled = true;
            contrastCheckbox.IsChecked = false;
            gammaCheckbox.IsEnabled = true;
            gammaCheckbox.IsChecked = false;

            blurCheckbox.IsEnabled = true;
            blurCheckbox.IsChecked = false;
            gaussianCheckbox.IsEnabled = true;
            gaussianCheckbox.IsChecked = false;
            sharpenCheckbox.IsEnabled = true;
            sharpenCheckbox.IsChecked = false;
            edgeCheckbox.IsEnabled = true;
            edgeCheckbox.IsChecked = false;
            embossCheckbox.IsEnabled = true;
            embossCheckbox.IsChecked = false;
        }

        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            if (!isImageLoaded) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image files (*.png;*.bmp;*.jpg|*.png;*.bmp;*.jpg";
            ImageFormat format = ImageFormat.Png;
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                }
                bmp.Save(saveFileDialog.FileName, format);
            }
        }

        private void ReverseChanges_Click(object sender, RoutedEventArgs e)
        {
            bmp = new Bitmap(copyBmp);
            BitmapImage bmpImage = Bitmap2BitmapImage(bmp);
            filteredImage.Source = bmpImage;

            inverseCheckbox.IsChecked = false;
            brightnessCheckbox.IsChecked = false;
            contrastCheckbox.IsChecked = false;
            gammaCheckbox.IsChecked = false;

            blurCheckbox.IsChecked = false;
            gaussianCheckbox.IsChecked = false;
            sharpenCheckbox.IsChecked = false;
            edgeCheckbox.IsChecked = false;
            embossCheckbox.IsChecked = false;

            System.Windows.Forms.MessageBox.Show("Reverse changes");
        }

        private BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        // ----------- Function filters ----------

        private void InverseImage()
        {
            if (!isImageLoaded) return;

            BitmapData bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();

                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                double pixel = 0;

                while ((int)ptr != stopAddress)
                {
                    pixel = 255 - ptr[0];
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[0] = (byte)pixel;

                    pixel = 255 - ptr[1];
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[1] = (byte)pixel;

                    pixel = 255 - ptr[2];
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[2] = (byte)pixel;

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);

            BitmapImage bmpImage = Bitmap2BitmapImage(bmp);
            filteredImage.Source = bmpImage;
        }

        private void InverseCheckbox_Click(object sender, RoutedEventArgs e)
        {
            InverseImage();
        }

        private void BrightnessCorrection()
        {
            if (!isImageLoaded) return;

            double parameter = 0;
            if (brightnessCheckbox.IsChecked == false)
                parameter -= brightnessParameter;
            else
                parameter += brightnessParameter;

            BitmapData bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();

                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                double pixel = 0;

                while ((int)ptr != stopAddress)
                {
                    pixel = ptr[0] + parameter;
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[0] = (byte)pixel;

                    pixel = ptr[1] + parameter;
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[1] = (byte)pixel;

                    pixel = ptr[2] + parameter;
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[2] = (byte)pixel;

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);

            BitmapImage bmpImage = Bitmap2BitmapImage(bmp);
            filteredImage.Source = bmpImage;
        }

        private void BrightnessCheckbox_Click(object sender, RoutedEventArgs e)
        {
            BrightnessCorrection();
        }

        private void ContrastEnhancement()
        {
            if (!isImageLoaded) return;
            if (contrastParameter < -100 || contrastParameter > 100) return;

            BitmapData bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();

                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                double pixel = 0, contrast = (100.0 + contrastParameter) / 100.0;

                contrast *= contrast;

                while ((int)ptr != stopAddress)
                {
                    pixel = ptr[0] / 255.0;
                    pixel -= 0.5;
                    pixel *= contrast;
                    pixel += 0.5;
                    pixel *= 255;
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[0] = (byte)pixel;

                    pixel = ptr[1] / 255.0;
                    pixel -= 0.5;
                    pixel *= contrast;
                    pixel += 0.5;
                    pixel *= 255;
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[1] = (byte)pixel;

                    pixel = ptr[2] / 255.0;
                    pixel -= 0.5;
                    pixel *= contrast;
                    pixel += 0.5;
                    pixel *= 255;
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[2] = (byte)pixel;

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);

            BitmapImage bmpImage = Bitmap2BitmapImage(bmp);
            filteredImage.Source = bmpImage;
        }

        private void ContrastCheckbox_Click(object sender, RoutedEventArgs e)
        {
            ContrastEnhancement();
        }

        private void GammaCorrection()
        {
            if (!isImageLoaded) return;

            BitmapData bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();

                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                double pixel = 0;

                while ((int)ptr != stopAddress)
                {
                    pixel = ptr[0] / 255.0;
                    pixel = Math.Pow(pixel, gammaParameter);
                    pixel *= 255.0;
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[0] = (byte)pixel;

                    pixel = ptr[1] / 255.0;
                    pixel = Math.Pow(pixel, gammaParameter);
                    pixel *= 255.0;
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[1] = (byte)pixel;

                    pixel = ptr[2] / 255.0;
                    pixel = Math.Pow(pixel, gammaParameter);
                    pixel *= 255.0;
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[2] = (byte)pixel;

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);

            BitmapImage bmpImage = Bitmap2BitmapImage(bmp);
            filteredImage.Source = bmpImage;
        }

        private void GammaCheckbox_Click(object sender, RoutedEventArgs e)
        {
            GammaCorrection();
        }

        // ----------- Convolution filters ----------

        private void InitializeConvolutionFilters()
        {
            filters = new Dictionary<string, Filter>();

            Filter blur = new Filter("Blur", 3, 9);
            blur.ConvolutionKernel = new int[,] { { 1, 1, 1 },
                                                  { 1, 1, 1 },
                                                  { 1, 1, 1 } };
            filters.Add(blur.Name, blur);

            Filter gaussian = new Filter("Gaussian", 3, 8);
            gaussian.ConvolutionKernel = new int[,] { { 0, 1, 0 },
                                                  { 1, 4, 1 },
                                                  { 0, 1, 0 } };
            filters.Add(gaussian.Name, gaussian);

            int sharpenA = 1;
            int sharpenB = 5;
            int sharpenS = sharpenB - 4 * sharpenA;
            Filter sharpen = new Filter("Sharpen", 3, 1);
            sharpen.ConvolutionKernel = new int[,] { {                  0,     -sharpenA/sharpenS,                      0 },
                                                  { -sharpenA/sharpenS,     sharpenB/sharpenS,      -sharpenA/sharpenS },
                                                  {                  0,     -sharpenA/sharpenS,                      0 } };
            filters.Add(sharpen.Name, sharpen);

            Filter edgeDetection = new Filter("EdgeDetection", 3, 1);
            edgeDetection.ConvolutionKernel = new int[,] { {  0, -1,  0 },
                                                  { -1,  4, -1 },
                                                  {  0, -1,  0 } };
            filters.Add(edgeDetection.Name, edgeDetection);

            Filter emboss = new Filter("Emboss", 3, 1);
            emboss.ConvolutionKernel = new int[,] { { -1, -1,  0 },
                                                  { -1,  1,  1 },
                                                  {  0,  1,  1 } };
            filters.Add(emboss.Name, emboss);
        }

        private void BlurCheckbox_Click(object sender, RoutedEventArgs e)
        {
            ConvolutionFilter(filters["Blur"]);
        }

        private void GaussianCheckbox_Click(object sender, RoutedEventArgs e)
        {
            ConvolutionFilter(filters["Gaussian"]);
        }

        private void SharpenCheckbox_Click(object sender, RoutedEventArgs e)
        {
            ConvolutionFilter(filters["Sharpen"]);
        }

        private void EdgeCheckbox_Click(object sender, RoutedEventArgs e)
        {
            ConvolutionFilter(filters["EdgeDetection"]);
        }

        private void EmbossCheckbox_Click(object sender, RoutedEventArgs e)
        {
            ConvolutionFilter(filters["Emboss"]);
        }

        private void ConvolutionFilter(Filter filter)
        {
            if (!isImageLoaded) return;

            BitmapData bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            byte[] pixelBuffer = new byte[bmpData.Stride * bmpData.Height];
            byte[] resultBuffer = new byte[bmpData.Stride * bmpData.Height];

            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            bmp.UnlockBits(bmpData);

            double[] colors = { 0, 0, 0 };

            int filterOffset = (filter.Size - 1) / 2;
            int byteOffset = 0;
            int currentOffset = 0;

            for (int offsetY = filterOffset; offsetY < bmpData.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < bmpData.Width - filterOffset; offsetX++)
                {
                    colors[0] = 0;
                    colors[1] = 0;
                    colors[2] = 0;

                    byteOffset = (offsetY * bmpData.Stride) + (offsetX * 3);

                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            currentOffset = byteOffset + (filterY * bmpData.Stride) + (filterX * 3);

                            colors[0] += (double)(pixelBuffer[currentOffset]) * filter.ConvolutionKernel[filterY + filterOffset, filterX + filterOffset] / filter.ConvolutionFactor;
                            colors[1] += (double)(pixelBuffer[currentOffset + 1]) * filter.ConvolutionKernel[filterY + filterOffset, filterX + filterOffset] / filter.ConvolutionFactor;
                            colors[2] += (double)(pixelBuffer[currentOffset + 2]) * filter.ConvolutionKernel[filterY + filterOffset, filterX + filterOffset] / filter.ConvolutionFactor;
                        }
                    }

                    if (colors[0] > 255)
                        colors[0] = 255;
                    else if (colors[0] < 0)
                        colors[0] = 0;

                    if (colors[1] > 255)
                        colors[1] = 255;
                    else if (colors[1] < 0)
                        colors[1] = 0;

                    if (colors[2] > 255)
                        colors[2] = 255;
                    else if (colors[2] < 0)
                        colors[2] = 0;

                    resultBuffer[byteOffset] = (byte)colors[0];
                    resultBuffer[byteOffset + 1] = (byte)colors[1];
                    resultBuffer[byteOffset + 2] = (byte)colors[2];
                }
            }

            // upper and lower edge
            int nextEdge = bmpData.Stride * (bmpData.Height - 1);
            for (int offsetX = 0; offsetX < bmpData.Width; offsetX++)
            {
                currentOffset = offsetX * 3;
                resultBuffer[currentOffset] = pixelBuffer[currentOffset];
                resultBuffer[currentOffset + 1] = pixelBuffer[currentOffset + 1];
                resultBuffer[currentOffset + 2] = pixelBuffer[currentOffset + 2];

                currentOffset += nextEdge;
                resultBuffer[currentOffset] = pixelBuffer[currentOffset];
                resultBuffer[currentOffset + 1] = pixelBuffer[currentOffset + 1];
                resultBuffer[currentOffset + 2] = pixelBuffer[currentOffset + 2];
            }

            // left and right edge
            nextEdge = bmpData.Width - 1;
            for (int offsetY = 1; offsetY < bmpData.Height; offsetY++)
            {
                currentOffset = offsetY * bmpData.Stride;
                resultBuffer[currentOffset] = pixelBuffer[currentOffset];
                resultBuffer[currentOffset + 1] = pixelBuffer[currentOffset + 1];
                resultBuffer[currentOffset + 2] = pixelBuffer[currentOffset + 2];

                currentOffset += nextEdge * 3;
                resultBuffer[currentOffset] = pixelBuffer[currentOffset];
                resultBuffer[currentOffset + 1] = pixelBuffer[currentOffset + 1];
                resultBuffer[currentOffset + 2] = pixelBuffer[currentOffset + 2];
            }

            Bitmap bmpResult = new Bitmap(bmp.Width, bmp.Height);
            BitmapData resultData = bmpResult.LockBits(new System.Drawing.Rectangle(0, 0, bmpResult.Width, bmpResult.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);

            bmpResult.UnlockBits(resultData);

            bmp = bmpResult;
            BitmapImage bmpImage = Bitmap2BitmapImage(bmpResult);
            filteredImage.Source = bmpImage;
        }

        // ---------- TASK 1 ----------

        Polyline polyline;
        PointCollection points;
        bool move = false;

        private void InitializeCanvas()
        {
            polyline = new Polyline();
            points = new PointCollection
            {
                new Point(0, 255),
                new Point(100,200),
                new Point(255, 0)
            };

            polyline.Stroke = System.Windows.Media.Brushes.Red;
            polyline.StrokeThickness = 2;

            polyline.Points = points;
            canvas.Children.Add(polyline);
        }

        private void CreatePoint()
        {

        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(canvas);
            p.Y = 255 - p.Y;

            if (move)
            {
                moveFromX.Text = ((int)p.X).ToString();
                moveFromY.Text = ((int)p.Y).ToString();
            }
            else
            {
                pointX.Text = ((int)p.X).ToString();
                pointY.Text = ((int)p.Y).ToString();
            }
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (move)
            {
                Point p = e.GetPosition(canvas);
                p.Y = 255 - p.Y;

                moveToX.Text = ((int)p.X).ToString();
                moveToY.Text = ((int)p.Y).ToString();
            }
        }

        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (move)
            {
                move = false;
                moveButton.Content = "Move";
                moveMenu.Visibility = Visibility.Hidden;

                pointX.IsEnabled = true;
                pointY.IsEnabled = true;
            }
            else
            {
                move = true;
                moveButton.Content = "Cancel";
                moveMenu.Visibility = Visibility.Visible;

                pointX.IsEnabled = false;
                pointY.IsEnabled = false;
            }
        }

    }

}
