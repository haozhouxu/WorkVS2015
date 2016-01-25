using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApplication_Excel_Image_Expose
{
    public class Helper
    {

    }

    public class PictureInfo
    {
        public int FromRow { get; set; }
        public int FromCol { get; set; }
        public Image Image { get; set; }
        public string RefId { get; set; }
    }
}
