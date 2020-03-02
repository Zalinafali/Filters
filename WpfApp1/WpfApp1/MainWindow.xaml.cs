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

namespace WpfApp1
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool isImageLoaded = false;
        Bitmap bmp = null;
        Bitmap copyBmp = null;
        int pixelWidth = 0;
        int pixelHeight = 0;

        // fixed parameters for filters
        const double brightnessParameter = 40.0;
        const double contrastParameter = 20.0;
        const double gammaParameter = 1.5;

        const int kernelSize = 3;
        int[,] kernel3x3 = new int[kernelSize,kernelSize];

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

        private void Blur()
        {
            if (!isImageLoaded) return;

            kernel3x3[0, 0] = 1;   kernel3x3[0, 1] = 1;   kernel3x3[0, 2] = 1;
            kernel3x3[1, 0] = 1;   kernel3x3[1, 1] = 1;   kernel3x3[1, 2] = 1;
            kernel3x3[2, 0] = 1;   kernel3x3[2, 1] = 1;   kernel3x3[2, 2] = 1;


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

        private void BlurCheckbox_Click(object sender, RoutedEventArgs e)
        {
            Blur();
        }
    }

    //public struct PixelColor
    //{
    //    public byte Blue;
    //    public byte Green;
    //    public byte Red;
    //    public byte Alpha;
    //}

    //private PixelColor[,] GetPixels(BitmapSource source)
    //{
    //    if (source.Format != PixelFormats.Bgra32)
    //        source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);

    //    pixelWidth = source.PixelWidth;
    //    pixelHeight = source.PixelHeight;
    //    PixelColor[,] result = new PixelColor[pixelWidth, pixelHeight];

    //    source.CopyPixels(result, pixelWidth * 4, 0);
    //    return result;
    //}

}
