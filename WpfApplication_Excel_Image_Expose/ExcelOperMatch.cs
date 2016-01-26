using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection;
using System.Globalization;

namespace WpfApplication_Excel_Image_Expose
{
    public class ExcelOperMatch
    {
        /// <summary>
        /// 根据SheetName和文件路径转换实体对象
        /// 将Excel中的数据映射实体对象集合
        /// </summary>
        /// <param name="sheetName">操作SheetName</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>实体对象集合</returns>
        public List<TestPerson> ExcelToPersons(string sheetName, string filePath)
        {
            List<TestPerson> listTestPersons = new List<TestPerson>();
            try
            {
                using (SpreadsheetDocument spreadDocument = SpreadsheetDocument.Open(filePath, false))//Excel文档包
                {
                    WorkbookPart workBookPart = spreadDocument.WorkbookPart;
                    listTestPersons = ExtcelToObjects(workBookPart, sheetName);
                }
            }
            catch (Exception exp)
            {
                // throw new Exception("可能Excel正在打开中,请关闭重新操作！");
            }
            return listTestPersons;
        }

        /// <summary>
        /// 根据WorkbookPart和SheetName获取实体对象集合
        /// </summary>
        /// <param name="workBookPart">WorkbookPart对象</param>
        /// <param name="sheetName">sheetName</param>
        /// <returns>实体对象集合</returns>
        private List<TestPerson> ExtcelToObjects(WorkbookPart workBookPart, string sheetName)
        {
            List<TestPerson> listPersons = new List<TestPerson>();
            List<string> columnValues = new List<string>();//列头值集合
            List<string> rowCellValues = null;//行数据集合

            //获取WorkbookPart下名为sheetName的Sheet的所有行数据
            IEnumerable<Row> sheetRows = new ExcelOper().GetWorkBookPartRows(workBookPart, sheetName);
            if (sheetRows == null || sheetRows.Count() <= 0)
            {
                return null;
            }

            //将数据导入DataTable,假定第一行为列名,第二行以后为数据
            foreach (Row row in sheetRows)
            {
                rowCellValues = new List<string>();//新行数据
                foreach (Cell cell in row)
                {
                    //获取单元格的值
                    string cellValue = new ExcelOper().GetCellValue(cell, workBookPart);
                    if (row.RowIndex == 1)
                    {
                        columnValues.Add(cellValue);
                    }
                    else
                    {
                        rowCellValues.Add(cellValue);
                    }
                }
                if (row.RowIndex > 1)
                {
                    int rowIndex = Convert.ToInt32(row.RowIndex.ToString());

                    //使用强类型处理
                    TestPerson singlePerson = ConvertToTestPerson(rowIndex, columnValues, rowCellValues);

                    //使用泛型处理,可以转换任意实体
                    //TestPerson singlePerson = ConvertToObject<TestPerson>(rowIndex, columnValues, rowCellValues);

                    listPersons.Add(singlePerson);
                }
            }
            return listPersons;
        }

        /// <summary>
        /// 根据行号\列集合\行集合转换实体对象
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnValues">列头集合</param>
        /// <param name="rowCellValues">一行数据集合</param>
        /// <returns>映射的实体对象</returns>
        private TestPerson ConvertToTestPerson(int rowIndex, List<string> columnValues, List<string> rowCellValues)
        {
            TestPerson singlePerson = new TestPerson();//TestPerson对象
            foreach (PropertyInfo pi in singlePerson.GetType().GetProperties())
            {
                for (int index = 0; index < columnValues.Count; index++)
                {
                    try
                    {
                        if (pi.Name.Equals(columnValues[index], StringComparison.OrdinalIgnoreCase))
                        {
                            String propertyType = pi.PropertyType.Name;
                            switch (propertyType)
                            {
                                case "Int32":
                                    pi.SetValue(singlePerson, int.Parse(rowCellValues[index]), null);
                                    break;
                                case "DateTime":
                                    pi.SetValue(singlePerson, DateTime.Parse(rowCellValues[index]), null);
                                    break;
                                case "Decimal":
                                    pi.SetValue(singlePerson, Decimal.Parse(rowCellValues[index]), null);
                                    break;
                                case "Double":
                                    pi.SetValue(singlePerson, Double.Parse(rowCellValues[index]), null);
                                    break;
                                case "String":
                                    pi.SetValue(singlePerson, rowCellValues[index], null);
                                    break;
                                case "Boolean":
                                    pi.SetValue(singlePerson, Boolean.Parse(rowCellValues[index]), null);
                                    break;
                            }
                            break;
                        }
                    }
                    catch (Exception exp)
                    {
                        index = (index - 1) % 26;
                        string cellRef = Convert.ToChar(65 + index).ToString() + rowIndex;
                        string expMessage = string.Format("请确认Excel中{0}位置数据填写正确！", cellRef);
                        throw new Exception(expMessage);
                    }
                }
            }
            return singlePerson;
        }

        /// <summary>
        /// 使用泛型,这样可以针对任何实体对象进行映射参照
        /// </summary>
        /// <typeparam name="T">映射实体对象</typeparam>
        /// <param name="rowIndex">行号</param>
        /// <param name="columnValues">列集合</param>
        /// <param name="rowCellValues">行单元格集合</param>
        /// <returns>实体对象</returns>
        private T ConvertToObject<T>(int rowIndex, List<string> columnValues, List<string> rowCellValues)
        {
            T singleT = Activator.CreateInstance<T>();//创建实体对象T
            foreach (PropertyInfo pi in singleT.GetType().GetProperties())
            {
                for (int index = 0; index < columnValues.Count; index++)
                {
                    try
                    {
                        if (pi.Name.Equals(columnValues[index], StringComparison.OrdinalIgnoreCase))
                        {
                            String propertyType = pi.PropertyType.Name;
                            switch (propertyType)
                            {
                                case "Int32":
                                    pi.SetValue(singleT, int.Parse(rowCellValues[index]), null);
                                    break;
                                case "DateTime":
                                    pi.SetValue(singleT, DateTime.Parse(rowCellValues[index]), null);
                                    break;
                                case "Decimal":
                                    pi.SetValue(singleT, Decimal.Parse(rowCellValues[index]), null);
                                    break;
                                case "Double":
                                    pi.SetValue(singleT, Double.Parse(rowCellValues[index]), null);
                                    break;
                                case "String":
                                    pi.SetValue(singleT, rowCellValues[index], null);
                                    break;
                                case "Boolean":
                                    pi.SetValue(singleT, Boolean.Parse(rowCellValues[index]), null);
                                    break;
                            }
                            break;
                        }
                    }
                    catch (Exception exp)
                    {
                        index = (index - 1) % 26;
                        string cellRef = Convert.ToChar(65 + index).ToString() + rowIndex;
                        string expMessage = string.Format("请确认Excel中{0}位置数据填写正确！", cellRef);
                        throw new Exception(expMessage);

                        //singleT = default(T);
                    }
                }
            }
            return singleT;
        }

    }

    /// <summary>
    /// 辅助测试类
    /// </summary>
    public class TestPerson
    {
        #region 公共属性
        //int类型测试
        public int PersonID { get; set; }
        //bool类型测试
        public bool PersonSex { get; set; }
        //string类型测试
        public string PersonName { get; set; }
        //DateTime 日期测试
        public DateTime PersonBirth { get; set; }
        //DateTime 时间测试
        public DateTime PersonLogTime { get; set; }
        //decimal类型测试
        public decimal PersonAmountMoney { get; set; }
        #endregion
    }
}