using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication_Excel_Image_Expose
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Expose();
        }

        public void Expose()
        {
            string path = @"C:\xhz\201401.xlsx";

            List<PictureInfo> pictures = null;
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(path, true))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                var sheets = wbPart.Workbook.Sheets.Take(1);
                foreach (Sheet sheet in sheets)
                {
                    WorksheetPart wsPart = (WorksheetPart)wbPart.GetPartById(sheet.Id);
                    DrawingsPart drawingPart = wsPart.GetPartsOfType<DrawingsPart>().ToList().FirstOrDefault();
                    pictures = new List<PictureInfo>();
                    if (drawingPart != null)
                    {
                        foreach (var part in drawingPart.Parts)
                        {
                            PictureInfo pic = new PictureInfo();
                            ImagePart imgPart = (ImagePart)part.OpenXmlPart;
                            //Image img1 = Image.FromStream(imgPart.GetStream());
                            Image img1 = new Image();
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = imgPart.GetStream();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            bitmap.Freeze();
                            img1.Source = bitmap;
                            pic.Image = img1;

                            //增加编码
                            string ss = "rId";
                            string[] tt = imgPart.Uri.OriginalString.Split('/');
                            string tt1 = tt[tt.Count() - 1].ToString().Split('.')[0];
                            string[] tt2 = tt1.Split('e');
                            string num = tt2[tt2.Count() - 1].ToString();
                            pic.RefId = ss + num;

                            pictures.Add(pic);
                        }

                        var worksheetDrawings = drawingPart.WorksheetDrawing.Where(c => c.ChildElements.Any(a => a.GetType().FullName == "DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture")).ToList();
                        foreach (var worksheetDrawing in worksheetDrawings)
                        {
                            if (worksheetDrawing.GetType().FullName == "DocumentFormat.OpenXml.Drawing.Spreadsheet.TwoCellAnchor")
                            {
                                TwoCellAnchor anchor = (TwoCellAnchor)worksheetDrawing;
                                DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture picDef = (DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture)anchor.ChildElements.FirstOrDefault(c => c.GetType().FullName == "DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture");
                                if (picDef != null)
                                {
                                    var embed = picDef.BlipFill.Blip.Embed;
                                    if (embed != null)
                                    {
                                        var picMapping = pictures.FirstOrDefault(c => c.RefId == embed.InnerText);
                                        picMapping.FromCol = int.Parse(anchor.FromMarker.ColumnId.InnerText);
                                        picMapping.FromRow = int.Parse(anchor.FromMarker.RowId.InnerText);
                                    }
                                }
                                // anchor.FromMarker.RowId + anchor.FromMarker.ColumnId 
                            }
                        }
                    }

                }
            }
            //把图片信息显示在DataGridView中
            var pic1 = pictures.OrderBy(c => c.FromCol).OrderBy(c => c.FromRow).ToList();
            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.ItemsSource = pic1;
        }

    }
}
