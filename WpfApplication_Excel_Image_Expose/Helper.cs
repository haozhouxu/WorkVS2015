using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;

namespace WpfApplication_Excel_Image_Expose
{
    public class helper
    {
        public static string ImageToBase64(Image im)
        {
            if (im.Source == null)
                return "";
            BitmapImage bi = im.Source as BitmapImage;
            MemoryStream ms = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bi));
            encoder.Save(ms);
            byte[] by = null;
            by = ms.ToArray();
            return Convert.ToBase64String(by);
        }

        public static string GetCellValue(WorkbookPart wbPart, Cell theCell)
        {
            string value = theCell.InnerText;
            if (theCell.DataType != null)
            {
                switch (theCell.DataType.Value)
                {
                    case CellValues.SharedString:
                        var stringTable = wbPart.
                          GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                        if (stringTable != null)
                        {
                            value = stringTable.SharedStringTable.
                              ElementAt(int.Parse(value)).InnerText;
                        }
                        break;

                    case CellValues.Boolean:
                        switch (value)
                        {
                            case "0":
                                value = "FALSE";
                                break;
                            default:
                                value = "TRUE";
                                break;
                        }
                        break;
                }
            }
            return value;
        }
    }

    public class PictureInfo
    {
        public int FromRow { get; set; }
        public int FromCol { get; set; }
        public Image Image { get; set; }
        public string RefId { get; set; }
    }

    public class Jewelry
    {
        #region 字段
        private string _guid;
        private Image _image;
        private string _buyTime;
        private Double _buyPrice;
        private Double _goldPrice;
        private string _type;
        private string _color;
        private string _mark;
        private string _buySource;

        private string _state;

        private string _saleTime;
        private string _saleWho;
        private Double _salePirce;

        private string _row;
        private string _col;

        #endregion

        #region 属性

        public string Guid
        {
            get
            {
                return _guid;
            }

            set
            {
                if (_guid != value)
                {
                    _guid = value;
                }
            }
        }

        public Image Image
        {
            get
            {
                return _image;
            }

            set
            {
                if (_image != value)
                {
                    _image = value;
                }
            }
        }

        public string BuyTime
        {
            get
            {
                return _buyTime;
            }

            set
            {
                if (_buyTime != value)
                {
                    _buyTime = value;
                }
            }
        }

        public Double BuyPrice
        {
            get
            {
                return _buyPrice;
            }

            set
            {
                if (_buyPrice != value)
                {
                    _buyPrice = value;
                }
            }
        }

        public double GoldPrice
        {
            get
            {
                return _goldPrice;
            }

            set
            {
                if (_goldPrice != value)
                {
                    _goldPrice = value;
                }
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }

            set
            {
                if (_type != value)
                {
                    _type = value;
                }
            }
        }

        public string Color
        {
            get
            {
                return _color;
            }

            set
            {
                if (_color != value)
                {
                    _color = value;
                }
            }
        }

        public string Mark
        {
            get
            {
                return _mark;
            }

            set
            {
                if (_mark != value)
                {
                    _mark = value;
                }
            }
        }

        public string BuySource
        {
            get
            {
                return _buySource;
            }

            set
            {
                if (_buySource != value)
                {
                    _buySource = value;
                }
            }
        }
        
        public string SaleTime
        {
            get
            {
                return _saleTime;
            }

            set
            {
                if (_saleTime != value)
                {
                    _saleTime = value;
                }
            }
        }

        public string SaleWho
        {
            get
            {
                return _saleWho;
            }

            set
            {
                if (_saleWho != value)
                {
                    _saleWho = value;
                }
            }
        }

        public Double SalePirce
        {
            get
            {
                return _salePirce;
            }

            set
            {
                if (_salePirce != value)
                {
                    _salePirce = value;
                }
            }
        }
        

        public string State
        {
            get
            {
                return _state;
            }

            set
            {
                if (_state != value)
                {
                    _state = value;
                }
            }
        }

        public string Row
        {
            get
            {
                return _row;
            }

            set
            {
                if (_row != value)
                {
                    _row = value;
                }
            }
        }

        public string Col
        {
            get
            {
                return _col;
            }

            set
            {
                if (_col != value)
                {
                    _col = value;
                }
            }
        }

        #endregion

        #region 方法
        public Jewelry()
        {

        }

        public Jewelry(bool isNew)
        {
            if (isNew)
            {
                _guid = System.Guid.NewGuid().ToString();
                Image = new Image();
            }
        }
        #endregion

    }
}
