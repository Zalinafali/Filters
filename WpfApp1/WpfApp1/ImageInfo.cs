using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    class ImageInfo
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Size { get; set; }
        public string FullPath { get; set; }

        public ImageInfo(string name, double width, double height, double size, string path)
        {
            Name = name;
            Width = (int)width;
            Height = (int)height;
            FullPath = path;
        }
    }
}
