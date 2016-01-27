using DocumentFormat.OpenXml;
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
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace WpfApplication_Excel_Image_Expose
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        List<PictureInfo> pictures = new List<PictureInfo>();
        ObservableCollection<Jewelry> oje = new ObservableCollection<Jewelry>();


        public MainWindow()
        {
            InitializeComponent();


            Expose();

            table();

            foreach (var item in oje)
            {
                var picture = pictures.FirstOrDefault(a => a.FromRow+1 == int.Parse(item.Row));
                if (null != picture)
                {
                    item.Image = picture.Image;
                }
            }

            foreach (var item in oje)
            {
                insertInToDB(item);
            }

            it1.ItemsSource = oje;
        }

        private void insertInToDB(Jewelry je)
        {
            string connectionFormat = @"Data Source=C:\xhz\{0};";
        //更新数据，不管是编辑，还是新增
            string connectString = string.Format(connectionFormat, "first");
            //把图片转换成base64编码
            string imageString = helper.ImageToBase64(je.Image);
            string sqlAll, sql;
            if (true)
            {
                sqlAll = "insert into Data (guid,image,buytime,buyprice,buywho,goldprice,type,color,mark,buySource,ownwho,state,borrowtime,borrowwho,borrowprice,borrowreturntime,saletime,salewho,saleprice,salestate,createtime,updatetime) Values('{0}','{1}','{2}',{3},'{4}',{5},'{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}',{14},'{15}','{16}','{17}',{18},'{19}','{20}','{21}')";
                sql = string.Format(sqlAll, je.Guid, imageString, Convert.ToDateTime(je.BuyTime).ToString("s"), je.BuyPrice, "", je.GoldPrice, je.Type, je.Color, je.Mark, je.BuySource, "", je.State, Convert.ToDateTime(null).ToString("s"), "", 0, Convert.ToDateTime(null).ToString("s"), Convert.ToDateTime(je.SaleTime).ToString("s"), je.SaleWho, je.SalePirce, "", System.DateTime.Now.ToString("s"), System.DateTime.Now.ToString("s"));
            }
            //else
            //{
            //    sqlAll = "update Data set image = '{0}',buytime = '{1}',buyprice = '{2}',buywho = '{3}',goldprice = {4},type = '{5}',color = '{6}',mark = '{7}',buySource = '{8}',ownwho = '{9}',state = '{10}',borrowtime = '{11}',borrowwho = '{12}',borrowprice = {13},borrowreturntime = '{14}',saletime = '{15}',salewho = '{16}',saleprice = {17},salestate = '{18}',updatetime = '{19}' where guid='{20}'";
            //    sql = string.Format(sqlAll, imageString, Convert.ToDateTime(je.BuyTime).ToString("s"), je.BuyPrice, je.BuyWho, je.GoldPrice, je.Type, je.Color, je.Mark, je.BuySource, je.OwnWho, je.State, Convert.ToDateTime(je.BorrowTime).ToString("s"), je.BorrowWho, je.BorrowPirce, Convert.ToDateTime(je.BorrowReturnTime).ToString("s"), Convert.ToDateTime(je.SaleTime).ToString("s"), je.SaleWho, je.SalePirce, je.SaleState, System.DateTime.Now.ToString("s"), je.Guid);
            //}
            int temp ;
            using (SQLiteConnection sqlcon = new SQLiteConnection(connectString))
            {
                SQLiteCommand cmd = new SQLiteCommand(sql, sqlcon);
                sqlcon.Open();
                temp = cmd.ExecuteNonQuery();
                sqlcon.Close();
            }
        }

        public void Expose()
        {
            //string path = @"C:\xhz\201401.xlsx";
            string path = @"C:\xhz\综合.xlsx";

            //List<PictureInfo> pictures = null;
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(path, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                var sheets = wbPart.Workbook.Sheets.Take(1);
                foreach (Sheet sheet in sheets)
                {
                    WorksheetPart wsPart = (WorksheetPart)wbPart.GetPartById(sheet.Id);
                    DrawingsPart drawingPart = wsPart.GetPartsOfType<DrawingsPart>().ToList().FirstOrDefault();
                    //pictures = new List<PictureInfo>();
                    if (drawingPart != null)
                    {
                        foreach (var part in drawingPart.Parts)
                        {
                            PictureInfo pic = new PictureInfo();
                            ImagePart imgPart = (ImagePart)part.OpenXmlPart;
                            //Image img1 = Image.FromStream(imgPart.GetStream());
                            Image img1 = new Image();
                            try
                            {
                                var bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.StreamSource = imgPart.GetStream();
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.EndInit();
                                bitmap.Freeze();
                                img1.Source = bitmap;
                                pic.Image = img1;
                            }
                            catch (Exception)
                            {

                                throw;
                            }

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
            //dataGridView1.AutoGenerateColumns = true;
            it1.ItemsSource = pic1;
            //dataGridView1.ItemsSource = pic1;
        }

        public void ex()
        {
            //string path = @"C:\xhz\综合.xlsx";
            //using (SpreadsheetDocument document = SpreadsheetDocument.Open(path, false))
            //{
            //    string version = string.Empty;
            //    WorkbookPart wbPart = document.WorkbookPart;
            //    List<Sheet> sheets = wbPart.Workbook.Descendants<Sheet>().ToList();
            //    var versionSheet = wbPart.Workbook.Descendants<Sheet>().FirstOrDefault(c => c.Name == "Version");
            //    WorksheetPart worksheetPart = (WorksheetPart)wbPart.GetPartById(versionSheet.Id);

            //    if (versionSheet == null)
            //        //throw new Exceptions.ValidationException("There must be Version sheet !");

            //    Cell theCell = worksheetPart.Worksheet.Descendants<Cell>().Where(c => c.CellReference.Value == "A2").FirstOrDefault();
            //    string type = string.Empty;
            //    if (theCell != null)
            //    {
            //        version = ExcelHelper.GetCellValue(wbPart, theCell);
            //    }
            //    else
            //    {
            //        //throw new Exceptions.ValidationException("Uploading file does not have version number!");
            //    }
            //}
        }

        public void table()
        {
            try
            {
                string filePath = @"C:\xhz\综合.xlsx";
                //根据Excel流转换为spreadDocument对象
                using (SpreadsheetDocument spreadDocument = SpreadsheetDocument.Open(filePath, false))//Excel文档包
                {
                    //Workbook workBook = spreadDocument.WorkbookPart.Workbook;//主文档部件的根元素
                    //Sheets sheeets = workBook.Sheets;//块级结构（如工作表、文件版本等）的容器
                    WorkbookPart workBookPart = spreadDocument.WorkbookPart;
                    //获取Excel中SheetName集合
                    List<string> sheetNames = GetSheetNames(workBookPart);

                    //遍历每个sheet页
                    foreach (string sheetName in sheetNames)
                    {
                        if (!sheetName.Equals("2013"))
                        {
                            break;
                        }
                        IEnumerable<Row> sheetRows = GetWorkBookPartRows(workBookPart, sheetName);
                        if (sheetRows == null || sheetRows.Count() <= 0)
                        {
                            return;
                        }

                        //将数据导入DataTable,假定第一行为列名,第二行以后为数据
                        foreach (Row row in sheetRows)
                        {
                            //获取Excel中的列头
                            if (row.RowIndex == 1)
                            {

                            }
                            else
                            {
                                //Excel第二行同时为DataTable的第一行数据
                                //DataRow dataRow = GetDataRow(row, dataTable, workBookPart);
                                //if (dataRow != null)
                                //{
                                //    dataTable.Rows.Add(dataRow);
                                //}

                                IEnumerable<Cell> cells = row.Elements<Cell>();
                                Jewelry je = new Jewelry(true);
                                string year = string.Empty;
                                string month = string.Empty;
                                foreach (Cell cell in cells)
                                {
                                    je.Row = cell.CellReference.Value.Substring(1);
                                    je.Col = cell.CellReference.Value.Substring(0, 1);
                                    string cellVlue = GetCellValue(cell, workBookPart);
                                    switch (je.Col)
                                    {
                                        case "A":
                                            year = cellVlue;
                                            break;
                                        case "B":
                                            month = cellVlue;
                                            if (string.IsNullOrEmpty(month)||string.IsNullOrEmpty(year))
                                            {
                                                
                                            }
                                            else
                                            {
                                                DateTime dt = new DateTime(int.Parse(year), int.Parse(month), 1);
                                                je.BuyTime = dt.ToString("s");
                                            }
                                            break;
                                        case "C":
                                            break;
                                        case "D":
                                            break;
                                        case "E":
                                            if (string.IsNullOrEmpty(cellVlue) ||string.IsNullOrWhiteSpace(cellVlue))
                                            {
                                                
                                            }
                                            else
                                            {
                                                int temp = int.Parse(cellVlue.Split('.')[0]);
                                                je.GoldPrice = Double.Parse(temp.ToString());
                                            }
                                            break;
                                        case "F":
                                            je.Type = cellVlue;
                                            break;
                                        case "G":
                                            je.Color = cellVlue;
                                            break;
                                        case "H":
                                            if (cellVlue.Equals("是"))
                                            {
                                                je.State = "卖出";
                                            }
                                            else
                                            {
                                                je.State = "未卖";
                                            }
                                            break;
                                        case "I":
                                            je.SaleWho = cellVlue;
                                            break;
                                        case "J":
                                            if (cellVlue.Length == 8)
                                            {
                                                DateTime dt1 = new DateTime(int.Parse(cellVlue.Substring(0, 4)), int.Parse(cellVlue.Substring(4, 2)), int.Parse(cellVlue.Substring(6, 2)));
                                                je.SaleTime = dt1.ToString("s");
                                            }
                                            else if (cellVlue.Length == 4)
                                            {
                                                int temp = 0;
                                                if (int.TryParse(cellVlue,out temp))
                                                {
                                                    DateTime dt2 = new DateTime(int.Parse(cellVlue.Substring(0, 4)), 1, 1);
                                                    je.SaleTime = dt2.ToString("s");
                                                }
                                            }
                                            break;
                                        case "K":
                                            if (string.IsNullOrEmpty(cellVlue)||string.IsNullOrWhiteSpace(cellVlue))
                                            {
                                                
                                            }
                                            else
                                            {
                                                je.Mark = cellVlue;
                                            }
                                            break;
                                        case "L":
                                            if (string.IsNullOrEmpty(cellVlue) || string.IsNullOrWhiteSpace(cellVlue))
                                            {

                                            }
                                            else
                                            {
                                                je.BuySource = cellVlue;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                oje.Add(je);
                            }
                        }
                        string s = "";
                    }
                }
            }
            catch (Exception exp)
            {
                //throw new Exception("可能Excel正在打开中,请关闭重新操作！");
            }
        }

        private List<string> GetSheetNames(WorkbookPart workBookPart)
        {
            List<string> sheetNames = new List<string>();
            Sheets sheets = workBookPart.Workbook.Sheets;
            foreach (Sheet sheet in sheets)
            {
                string sheetName = sheet.Name;
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheetNames.Add(sheetName);
                }
            }
            return sheetNames;
        }

        /// <summary>
        /// 根据WorkbookPart和sheetName获取该Sheet下所有Row数据
        /// </summary>
        /// <param name="workBookPart">WorkbookPart对象</param>
        /// <param name="sheetName">SheetName</param>
        /// <returns>该SheetName下的所有Row数据</returns>
        public IEnumerable<Row> GetWorkBookPartRows(WorkbookPart workBookPart, string sheetName)
        {
            IEnumerable<Row> sheetRows = null;
            //根据表名在WorkbookPart中获取Sheet集合
            IEnumerable<Sheet> sheets = workBookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName);
            if (sheets.Count() == 0)
            {
                return null;//没有数据
            }

            WorksheetPart workSheetPart = workBookPart.GetPartById(sheets.First().Id) as WorksheetPart;
            //获取Excel中得到的行
            sheetRows = workSheetPart.Worksheet.Descendants<Row>();

            return sheetRows;
        }

        public string GetCellValue(Cell cell, WorkbookPart workBookPart)
        {
            string cellValue = string.Empty;
            if (cell.ChildElements.Count == 0)//Cell节点下没有子节点
            {
                return cellValue;
            }
            string cellRefId = cell.CellReference.InnerText;//获取引用相对位置
            string cellInnerText = cell.CellValue.InnerText;//获取Cell的InnerText
            cellValue = cellInnerText;//指定默认值(其实用来处理Excel中的数字)

            //获取WorkbookPart中NumberingFormats样式集合
            List<string> dicStyles = GetNumberFormatsStyle(workBookPart);
            //获取WorkbookPart中共享String数据
            SharedStringTable sharedTable = workBookPart.SharedStringTablePart.SharedStringTable;

            try
            {
                EnumValue<CellValues> cellType = cell.DataType;//获取Cell数据类型
                if (cellType != null)//Excel对象数据
                {
                    switch (cellType.Value)
                    {
                        case CellValues.SharedString://字符串
                                                     //获取该Cell的所在的索引
                            int cellIndex = int.Parse(cellInnerText);
                            cellValue = sharedTable.ChildElements[cellIndex].InnerText;
                            break;
                        case CellValues.Boolean://布尔
                            cellValue = (cellInnerText == "1") ? "TRUE" : "FALSE";
                            break;
                        case CellValues.Date://日期
                            cellValue = Convert.ToDateTime(cellInnerText).ToString();
                            break;
                        case CellValues.Number://数字
                            cellValue = Convert.ToDecimal(cellInnerText).ToString();
                            break;
                        default: cellValue = cellInnerText; break;
                    }
                }
                else//格式化数据
                {
                    if (dicStyles.Count > 0 && cell.StyleIndex != null)//对于数字,cell.StyleIndex==null
                    {
                        int styleIndex = Convert.ToInt32(cell.StyleIndex.Value);
                        string cellStyle = string.Empty;
                        if (dicStyles.Count >= styleIndex)
                        {
                            cellStyle = dicStyles[styleIndex - 1];//获取该索引的样式
                        }
                        if (cellStyle.Contains("yyyy") || cellStyle.Contains("h")
                            || cellStyle.Contains("dd") || cellStyle.Contains("ss"))
                        {
                            //如果为日期或时间进行格式处理,去掉“;@”
                            cellStyle = cellStyle.Replace(";@", "");
                            while (cellStyle.Contains("[") && cellStyle.Contains("]"))
                            {
                                int otherStart = cellStyle.IndexOf('[');
                                int otherEnd = cellStyle.IndexOf("]");

                                cellStyle = cellStyle.Remove(otherStart, otherEnd - otherStart + 1);
                            }
                            double doubleDateTime = double.Parse(cellInnerText);
                            DateTime dateTime = DateTime.FromOADate(doubleDateTime);//将Double日期数字转为日期格式
                            if (cellStyle.Contains("m")) { cellStyle = cellStyle.Replace("m", "M"); }
                            if (cellStyle.Contains("AM/PM")) { cellStyle = cellStyle.Replace("AM/PM", ""); }
                            cellValue = dateTime.ToString(cellStyle);//不知道为什么Excel 2007中格式日期为yyyy/m/d
                        }
                        else//其他的货币、数值
                        {
                            //cellStyle = cellStyle.Substring(cellStyle.LastIndexOf('.')== -1?0: cellStyle.LastIndexOf('.') - 1).Replace("\\", "");
                            //decimal decimalNum = decimal.Parse(cellInnerText);
                            //cellValue = decimal.Parse(decimalNum.ToString(cellStyle)).ToString();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                //string expMessage = string.Format("Excel中{0}位置数据有误,请确认填写正确！", cellRefId);
                //throw new Exception(expMessage);
                cellValue = "N/A";
            }
            return cellValue;
        }

        private List<string> GetNumberFormatsStyle(WorkbookPart workBookPart)
        {
            List<string> dicStyle = new List<string>();
            Stylesheet styleSheet = workBookPart.WorkbookStylesPart.Stylesheet;
            OpenXmlElementList list = styleSheet.NumberingFormats.ChildElements;//获取NumberingFormats样式集合

            foreach (var element in list)//格式化节点
            {
                if (element.HasAttributes)
                {
                    using (OpenXmlReader reader = OpenXmlReader.Create(element))
                    {
                        if (reader.Read())
                        {
                            if (reader.Attributes.Count > 0)
                            {
                                string numFmtId = reader.Attributes[0].Value;//格式化ID
                                string formatCode = reader.Attributes[1].Value;//格式化Code
                                dicStyle.Add(formatCode);//将格式化Code写入List集合
                            }
                        }
                    }
                }
            }
            return dicStyle;
        }

    }
}
