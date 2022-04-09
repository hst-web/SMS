/*----------------------------------------------------------------

// 文件名：ExcelHelper.cs
// 功能描述：Excel操作类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.Reflection;
using System.Threading;
using System.ComponentModel;
using System.Text;

namespace HST.Utillity
{
    /// <summary>
    /// Excel操作类
    /// </summary>
    public class ExcelHelper
    {
        //自定义导出事件 
        public delegate void ExportPercent(int value);
        //public  event ExportPercent Export;
        public BackgroundWorker worker;
        
        public static byte[] ExportExcelByte<T>(List<T> data, List<ExcelHeaderColumn> excelColumns)
        {
            var sbHtml = new StringBuilder();
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");
          
            foreach (var item in excelColumns)
            {
                sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item.DisplayName);
            }
            sbHtml.Append("</tr>");

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            foreach (T item in data)
            {
                sbHtml.Append("<tr>");
                foreach(ExcelHeaderColumn col in excelColumns)
                {
                    PropertyInfo property= properties.FirstOrDefault(p => p.Name == col.Name);
                    
                    sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", property!=null? property.GetValue(item, null):"");
                }
                
                sbHtml.Append("</tr>");
            }
            sbHtml.Append("</table>");

            return  Encoding.Default.GetBytes(sbHtml.ToString());
        }

        /// <summary>
        /// 导出csv
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="excelColumns"></param>
        /// <returns></returns>
        public static byte[] ExportCsvByte<T>(List<T> data, List<ExcelHeaderColumn> excelColumns)
        {
            var sbHtml = new StringBuilder();
           
            foreach (var item in excelColumns)
            {
                sbHtml.Append(item.DisplayName+",");
            }
            sbHtml.Append("\n");

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            if(data!=null)
            {
                foreach (T item in data)
                {
                    foreach (ExcelHeaderColumn col in excelColumns)
                    {
                        PropertyInfo property = properties.FirstOrDefault(p => p.Name == col.Name);

                        sbHtml.Append(property != null ? property.GetValue(item, null) + "," : ",");
                    }
                    sbHtml.Append("\n");
                }
            }
            
            return Encoding.Default.GetBytes(sbHtml.ToString());
        }


        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="excelColumns">列头</param>
        /// <param name="titleName">标题</param>
        /// <returns></returns>
        public MemoryStream ExportExcel<T>(List<T> data, List<ExcelHeaderColumn> excelColumns, string titleName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            //标题
            IRow rowTitle = sheet.CreateRow(0);
            ICell celltitle = rowTitle.CreateCell(0, CellType.String);
            celltitle.SetCellValue(titleName);
            rowTitle.Cells.Add(celltitle);

            //列名
            IRow rowColum = sheet.CreateRow(1);
            for (int j = 0; j < excelColumns.Count; j++)
            {
                ICell cell = rowColum.CreateCell(j);
                cell.SetCellValue(excelColumns[j].DisplayName);
                rowColum.Cells.Add(cell);
            }

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            string value = null;
            for (int i = 0; i < data.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 2);
                for (int j = 0; j < excelColumns.Count; j++)
                {
                    value = null;
                    ExcelHeaderColumn col = excelColumns[j];
                    PropertyInfo property = properties.FirstOrDefault(p => p.Name == col.Name);
                    if (property != null)
                    {
                        Object tempValue = property.GetValue(data[i], null);
                        if (tempValue != null) { value = tempValue.ToString(); }
                    }
                    SetCellValue(row, j, value);
                }
                if (worker != null)
                {
                    worker.ReportProgress(i);
                    Thread.Sleep(10);
                }
            }
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            return stream;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="excelColumns">列头</param>
        /// <returns></returns>
        public  MemoryStream ExportExcel<T>(List<T> data, List<ExcelHeaderColumn> excelColumns)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            CreateHeader(sheet, excelColumns);
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            string value = null;
            for (int i = 0; i < data.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < excelColumns.Count; j++)
                {
                    value = null;
                    ExcelHeaderColumn col = excelColumns[j];
                    PropertyInfo property = properties.FirstOrDefault(p => p.Name == col.Name);
                    if (property != null)
                    {
                        Object tempValue = property.GetValue(data[i], null);
                        if (tempValue != null) { value = tempValue.ToString(); }
                    }
                    SetCellValue(row, j, value);
                }
                if (worker != null)
                {
                    worker.ReportProgress(i);
                    Thread.Sleep(10);
                }
            }
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            return stream;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dataTable">数据</param>
        /// <param name="excelColumns">列头</param>
        /// <param name="titleName">标题</param>
        /// <returns></returns>
        public MemoryStream ExportExcel(DataTable dataTable, List<ExcelHeaderColumn> excelColumns, string titleName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");

            //标题
            IRow rowTitle = sheet.CreateRow(0);
            ICell celltitle = rowTitle.CreateCell(0, CellType.String);
            celltitle.SetCellValue(titleName);
            rowTitle.Cells.Add(celltitle);

            //列名
            IRow column = sheet.CreateRow(1);
            for (int j = 0; j < excelColumns.Count; j++)
            {
                ICell cell = column.CreateCell(j);
                cell.SetCellValue(excelColumns[j].DisplayName);
                column.Cells.Add(cell);
            }

            string value = null;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 2);
                for (int j = 0; j < excelColumns.Count; j++)
                {
                    value = null;
                    ExcelHeaderColumn col = excelColumns[j];
                    if (dataTable.Columns.Contains(col.Name))
                    {
                        Object tempValue = dataTable.Rows[i][col.Name];
                        if (tempValue != null && tempValue != DBNull.Value) { value = tempValue.ToString(); }
                    }
                    SetCellValue(row, j, value);
                }

                if (worker != null)
                {
                    worker.ReportProgress(i);
                    Thread.Sleep(10);
                }
            }
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            return stream;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="excelColumns">sheetName</param>
        /// <param name="sheetName">sheet名</param>
        /// <returns></returns>
        public  void ExportExcel<T>(List<T> data, List<ExcelHeaderColumn> excelColumns, ref  ISheet sheet)
        {

            CreateHeader(sheet, excelColumns);
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            string value = null;
            for (int i = 0; data != null && i < data.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < excelColumns.Count; j++)
                {
                    value = null;
                    ExcelHeaderColumn col = excelColumns[j];
                    PropertyInfo property = properties.FirstOrDefault(p => p.Name == col.Name);
                    if (property != null)
                    {
                        Object tempValue = property.GetValue(data[i], null);
                        if (tempValue != null) { value = tempValue.ToString(); }
                    }
                    SetCellValue(row, j, value);
                }
            }

        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="fileStream">Excel文件流</param>
        /// <param name="sheetName">Sheet名称</param>
        /// <param name="action"></param>
        public  void ImporttExcel(Stream fileStream, string sheetName, Action<ExcelColumn> action)
        {
            if (action != null)
            {
                HSSFWorkbook workbook = new HSSFWorkbook(fileStream);
                ISheet sheet = workbook.GetSheet(sheetName);
                InnnerImportExcel(sheet, action);
            }
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="fileStream">Excel文件流</param>
        /// <param name="sheetName">Sheet名称</param>
        /// <param name="action">action为输入值为RowIndex,ColumnIndex,CellValue</param>
        public  void ImporttExcel(Stream fileStream, string sheetName, Action<ExcelRow> action)
        {
            if (action != null)
            {
                HSSFWorkbook workbook = new HSSFWorkbook(fileStream);
                ISheet sheet = workbook.GetSheet(sheetName);
                InnnerImportExcel(sheet, action);
            }
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="fileStream">Excel文件流</param>
        /// <param name="action">action为输入值为RowIndex,ColumnIndex,CellValue</param>
        public  void ImporttExcel(Stream fileStream, Action<ExcelColumn> action)
        {
            if (action != null)
            {
                HSSFWorkbook workbook = new HSSFWorkbook(fileStream);
                ISheet sheet = workbook.GetSheetAt(0);
                InnnerImportExcel(sheet, action);
            }
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="fileStream">Excel文件流</param>
        /// <param name="action">action为输入值为RowIndex,ColumnIndex,CellValue</param>
        public  void ImporttExcel(Stream fileStream, Action<ExcelRow> action)
        {
            if (action != null)
            {
                HSSFWorkbook workbook = new HSSFWorkbook(fileStream);
                ISheet sheet = workbook.GetSheetAt(0);
                InnnerImportExcel(sheet, action);
            }
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="file">Excel文件地址</param>
        /// <param name="sheetName">Sheet名称</param>
        /// <param name="action"></param>
        private  void InnnerImportExcel(ISheet sheet, Action<ExcelColumn> action)
        {
            int rowIndex = 0;
            int columnIndex = 0;
            int firstRow = sheet.FirstRowNum;
            int lastRow = sheet.LastRowNum;
            object value = null;
            for (rowIndex = firstRow; rowIndex <= lastRow; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);
                for (columnIndex = 0; columnIndex < row.LastCellNum; columnIndex++)
                {
                    ICell cell = row.GetCell(columnIndex);
                    value = GetCellValue(cell);
                    action(new ExcelColumn() { RowIndex = rowIndex, ColumnIndex = columnIndex, Value = value });
                }
            }
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="file">Excel文件地址</param>
        /// <param name="sheetName">Sheet名称</param>
        /// <param name="action"></param>
        private  void InnnerImportExcel(ISheet sheet, Action<ExcelRow> action)
        {
            int rowIndex = 0;
            int columnIndex = 0;
            int firstRow = sheet.FirstRowNum;
            int lastRow = sheet.LastRowNum;
            object value = null;
            for (rowIndex = firstRow; rowIndex <= lastRow; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);
                ExcelRow excelRow = new ExcelRow()
                {
                    RowIndex = rowIndex
                };
                for (columnIndex = 0; columnIndex < row.LastCellNum; columnIndex++)
                {
                    ICell cell = row.GetCell(columnIndex);
                    value = GetCellValue(cell);
                    excelRow.AddColumn(columnIndex, value);
                }
                action(excelRow);
            }
        }

        /// <summary>
        /// 取Cell的值
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private  object GetCellValue(ICell cell)
        {
            object value = null;
            if (cell == null) return value;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    break;
                case CellType.Boolean:
                    value = cell.BooleanCellValue;
                    break;
                case CellType.Error:
                    value = cell.ErrorCellValue;
                    break;
                case CellType.Formula:
                    value = cell.CellFormula;
                    break;
                case CellType.Numeric:
                    value = cell.NumericCellValue;
                    break;
                case CellType.String:
                    value = cell.StringCellValue;
                    break;
                case CellType.Unknown:
                    try
                    {
                        value = cell.DateCellValue;
                    }
                    catch
                    {
                        try
                        {
                            value = cell.RichStringCellValue;
                        }
                        catch
                        {

                        }
                    }
                    break;
            }

            return value;
        }


        /// <summary>
        /// 创建列头
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="excelColumns"></param>
        private  void CreateHeader(ISheet sheet, List<ExcelHeaderColumn> excelColumns)
        {
            IRow row = sheet.CreateRow(0);
            for (int j = 0; j < excelColumns.Count; j++)
            {
                ICell cell = row.CreateCell(j);
                cell.SetCellValue(excelColumns[j].DisplayName);
                row.Cells.Add(cell);
            }

        }

        /// <summary>
        /// 设置Cell的值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="j"></param>
        /// <param name="value"></param>
        private  void SetCellValue(IRow row, int j, string value)
        {
            ICell cell = row.CreateCell(j);
            cell.SetCellValue(value);
            row.Cells.Add(cell);
        }
    }

    /// <summary>
    ///  Excel的列描述B
    /// </summary>
    public class ExcelHeaderColumn
    {
        /// <summary>
        /// 实体的属性名称或者DataTable有列名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Excel显示的列头名称
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Excel行
    /// </summary>
    public class ExcelRow
    {
        private int _rowIndex = 0;

        /// <summary>
        /// 行索引号
        /// </summary>
        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        private List<ExcelColumn> _excelColumns = new List<ExcelColumn>();

        /// <summary>
        /// 列集合
        /// </summary>
        public List<ExcelColumn> ExcelColumns
        {
            get { return _excelColumns; }
            set { _excelColumns = value; }
        }

        /// <summary>
        /// 增加列
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="value"></param>
        public void AddColumn(int columnIndex, object value)
        {
            this._excelColumns.Add(new ExcelColumn() { ColumnIndex = columnIndex, Value = value, RowIndex = _rowIndex });
        }
    }

    /// <summary>
    /// Excel列
    /// </summary>
    public class ExcelColumn
    {
        private int _rowIndex = 0;

        /// <summary>
        /// 行索引号
        /// </summary>
        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        private int _columnIndex = 0;

        /// <summary>
        /// 列索引号
        /// </summary>
        public int ColumnIndex
        {
            get { return _columnIndex; }
            set { _columnIndex = value; }
        }

        private object _value;
        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
