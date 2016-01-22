using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;

namespace WpfApplication_Excel_Image_Expose
{
    public class Helper
    {
        public static void Expose()
        {
            List<PictureInfo> pictures = null;
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(@txtFile.Text, true))
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
                            Image img1 = Image.FromStream(imgPart.GetStream());
                            pic.Image = img1;
                            pictures.Add(pic);
                        }

                        var worksheetDrawings = drawingPart.WorksheetDrawing.Where(c => c.ChildElements.Any
                            (a => a.GetType().FullName == "DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture")).ToList();
                        foreach (var worksheetDrawing in worksheetDrawings)
                        {
                            if (worksheetDrawing.GetType().FullName ==
                                "DocumentFormat.OpenXml.Drawing.Spreadsheet.TwoCellAnchor")
                            {
                                TwoCellAnchor anchor = (TwoCellAnchor)worksheetDrawing;
                                DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture picDef =
                                    (DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture)
                                    anchor.ChildElements.FirstOrDefault(c => c.GetType().FullName ==
                                    "DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture");
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

            dataGridView1.DataSource = pic1;
        }
    }

    public class PictureInfo
    {
        public int FromRow { get; set; }
        public int FromCol { get; set; }
        public Image Image { get; set; }
    }
}
