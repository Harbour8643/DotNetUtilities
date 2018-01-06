using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HPSF;

namespace DotNet.Utilities
{
    /// <summary>
    /// 使用NPOI 第三方导出 Excel 
    /// </summary>
    public class ExcelNPOIHelper
    {
        #region ==========导出===========

        /// <summary>
        ///使用NPOI 导出 Excel 
        /// </summary>
        /// <param name="dtSource">DataTable数据</param>
        /// <param name="strFileName">文件名</param>
        /// <remarks>NPOI认为Excel的第一个单元格是：(0，0)</remarks>
        public static void ExportExcel(DataTable dtSource, string strFileName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            //填充表头
            IRow dataRow = sheet.CreateRow(0);
            foreach (DataColumn column in dtSource.Columns)
            {
                dataRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }


            //填充内容
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                dataRow = sheet.CreateRow(i + 1);
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    dataRow.CreateCell(j).SetCellValue(dtSource.Rows[i][j].ToString());
                }
            }


            //保存
            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        /// DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置</param>
        public static void ExportExcel(DataTable dtSource, string strHeaderText, string strFileName)
        {
            using (MemoryStream ms = ExportMemoryStream(dtSource, strHeaderText))
            {
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        private static MemoryStream ExportMemoryStream(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            #region 右击文件 属性信息

            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "http://www.Harbour.com/";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "Harbour"; //填加xls文件作者信息
                si.ApplicationName = "NPOI Export Excel"; //填加xls文件创建程序信息
                si.LastAuthor = "Harbour"; //填加xls文件最后保存者信息
                si.Comments = "Harbour Create"; //填加xls文件作者信息
                si.Title = "NPOI"; //填加xls文件标题信息
                si.Subject = "NPOI Theme";//填加文件主题信息
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }

            #endregion

            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }

            int rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;
                        CellRangeAddress cellRange = new CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1);
                        sheet.AddMergedRegion(cellRange);

                    }
                    #endregion


                    #region 列头及样式
                    {
                        IRow headerRow = sheet.CreateRow(1);


                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);


                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);

                        }
                    }
                    #endregion

                    rowIndex = 2;
                }
                #endregion

                #region 填充内容
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }
                #endregion

                rowIndex++;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

               return ms;
            }
        }


        ///// <summary>
        /////使用NPOI 导出 Excel 
        ///// </summary>
        ///// <param name="dtSource">DataTable数据</param>
        ///// <param name="strFileName">文件名</param>
        ///// <remarks>NPOI认为Excel的第一个单元格是：(0，0)</remarks>
        //public static void Export(System.Windows.Forms.ListView dtSource, string strFileName)
        //{
        //    HSSFWorkbook workbook = new HSSFWorkbook();
        //    ISheet sheet = workbook.CreateSheet();

        //    //填充表头
        //    IRow dataRow = sheet.CreateRow(0);
        //    for (int i = 0; i < dtSource.Columns.Count; i++)
        //    {
        //        dataRow.CreateCell(dtSource.Columns[i].Index).SetCellValue(dtSource.Columns[i].Text);
        //    }



        //    //填充内容
        //    for (int i = 0; i < dtSource.Items.Count; i++)
        //    {
        //        dataRow = sheet.CreateRow(i + 1);
        //        for (int j = 0; j < dtSource.Columns.Count; j++)
        //        {
        //            dataRow.CreateCell(j).SetCellValue(dtSource.Items[i].SubItems[j].Text.ToString());
        //        }
        //    }


        //    //保存
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
        //        {
        //            workbook.Write(ms);
        //            ms.Flush();
        //            ms.Position = 0;
        //            byte[] data = ms.ToArray();
        //            fs.Write(data, 0, data.Length);
        //            fs.Flush();
        //        }
        //    }
        //}

        #endregion

        #region  ========读取excel=======

        /// <summary>
        /// 读取excel,默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable ImportExcel(string strFileName)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                //出错
                if (dt.Columns.Contains(cell.ToString()))
                {
                    dt.Columns.Add(cell.ToString() + "1");
                }
                else
                {
                    dt.Columns.Add(cell.ToString());
                }
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="ExcelFileStream">Excel文件Stream</param>
        /// <param name="SheetName">Sheet名</param>
        /// <param name="HeaderRowIndex">表头行号</param>
        /// <returns>DataTable</returns>
        public static DataTable ImportExcel(Stream ExcelFileStream, string SheetName, int HeaderRowIndex)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            ISheet sheet = workbook.GetSheet(SheetName);

            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;

            for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                    dataRow[j] = row.GetCell(j).ToString();
            }

            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="ExcelFileStream">Excel文件Stream</param>
        /// <param name="SheetIndex">Sheet索引号</param>
        /// <param name="HeaderRowIndex">表头行号</param>
        /// <returns></returns>
        public static DataTable ImportExcel(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            ISheet sheet = workbook.GetSheetAt(SheetIndex);

            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;

            for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                table.Rows.Add(dataRow);
            }

            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }


        /// <summary>
        /// 由Excel 2003 导入DataSet，如果有多个工作表，则导入多个DataTable
        /// </summary>
        /// <param name="ExcelFilePath">Excel文件流</param>
        /// <param name="HeaderRowIndex">Excel表头行索引</param>
        /// <returns>DataSet</returns>
        public static DataSet ImportDataSetFromExcel2003(string ExcelFilePath, int HeaderRowIndex)
        {
            DataSet ds = new DataSet();
            using (FileStream stream = System.IO.File.OpenRead(ExcelFilePath))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(stream);
                try
                {
                    for (int a = 0, b = workbook.NumberOfSheets; a < b; a++)
                    {
                        DataTable table = new DataTable();
                        try
                        {
                            #region 标题
                            string sheetName = string.Empty;
                            ISheet sheet = workbook.GetSheetAt(a);

                            table.TableName = workbook.GetSheetName(a);
                            IRow headerRow = sheet.GetRow(HeaderRowIndex);
                            int cellCount = headerRow.LastCellNum;


                            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                            {
                                string strCellValue = string.Empty;
                                if (headerRow.GetCell(i) == null)
                                {
                                    // 如果遇到第一个空列，则不再继续向后读取
                                    cellCount = i + 1;
                                    break;
                                }
                                if (headerRow.GetCell(i).CellType == CellType.String)
                                {
                                    strCellValue = headerRow.GetCell(i).StringCellValue;

                                }
                                else if (headerRow.GetCell(i).CellType == CellType.Boolean)
                                {
                                    strCellValue = headerRow.GetCell(i).BooleanCellValue.ToString();
                                }
                                else if (headerRow.GetCell(i).CellType == CellType.Error)
                                {
                                    strCellValue = headerRow.GetCell(i).ErrorCellValue.ToString();
                                }
                                else if (headerRow.GetCell(i).CellType == CellType.Numeric)
                                {
                                    strCellValue = headerRow.GetCell(i).NumericCellValue.ToString();
                                }
                                else
                                {
                                    // 如果遇到第一个空列，则不再继续向后读取
                                    cellCount = i + 1;
                                    break;
                                }
                                if (strCellValue.Trim().Length < 1)
                                {
                                    // 如果遇到第一个空列，则不再继续向后读取
                                    cellCount = i + 1;
                                    break;
                                }
                                DataColumn column = null;
                                //string columnName = headerRow.GetCell(i).StringCellValue;
                                if (table.Columns.Contains(strCellValue))
                                {
                                    column = new DataColumn(strCellValue + i.ToString());
                                }
                                else
                                {
                                    column = new DataColumn(strCellValue);
                                }
                                // DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                                table.Columns.Add(column);
                            }
                            #endregion
                            #region 内容
                            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                            {
                                IRow row = sheet.GetRow(i);
                                if (row == null || row.GetCell(0) == null || row.GetCell(0).ToString().Trim() == "")
                                {
                                    // 如果遇到第一个空行，则不再继续向后读取
                                    break;
                                }
                                DataRow dataRow = table.NewRow();
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    if (row.GetCell(j) != null)
                                    {
                                        dataRow[j] = row.GetCell(j).ToString();
                                    }
                                }
                                table.Rows.Add(dataRow);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {

                        }
                        ds.Tables.Add(table);
                    }
                }
                catch (Exception ex)
                {

                }
                workbook = null;
                stream.Close();
            }
            return ds;

        }

        /// <summary>
        /// 由Excel导入DataSet，如果有多个工作表，则导入多个DataTable
        /// </summary>
        /// <param name="ExcelFilePath">Excel文件流</param>
        /// <param name="HeaderRowIndex">Excel表头行索引</param>
        /// <returns>DataSet</returns>
        public static DataSet ImportDataSetFromExcel2007(string ExcelFilePath, int HeaderRowIndex)
        {
            DataSet ds = new DataSet();
            using (FileStream stream = System.IO.File.OpenRead(ExcelFilePath))
            {
                XSSFWorkbook workbook = new XSSFWorkbook(stream);
                try
                {
                    for (int a = 0, b = workbook.NumberOfSheets; a < b; a++)
                    {
                        DataTable table = new DataTable();
                        try
                        {
                            #region 标题
                            string sheetName = string.Empty;
                            ISheet sheet = workbook.GetSheetAt(a);

                            table.TableName = workbook.GetSheetName(a);
                            IRow headerRow = sheet.GetRow(HeaderRowIndex);
                            if (headerRow == null)
                            {
                                break;
                            }
                            int cellCount = headerRow.LastCellNum;


                            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                            {
                                string strCellValue = string.Empty;
                                if (headerRow.GetCell(i) == null)
                                {
                                    // 如果遇到第一个空列，则不再继续向后读取
                                    cellCount = i + 1;
                                    break;
                                }
                                if (headerRow.GetCell(i).CellType == CellType.String)
                                {
                                    strCellValue = headerRow.GetCell(i).StringCellValue;

                                }
                                else if (headerRow.GetCell(i).CellType == CellType.Boolean)
                                {
                                    strCellValue = headerRow.GetCell(i).BooleanCellValue.ToString();
                                }
                                else if (headerRow.GetCell(i).CellType == CellType.Error)
                                {
                                    strCellValue = headerRow.GetCell(i).ErrorCellValue.ToString();
                                }
                                else if (headerRow.GetCell(i).CellType == CellType.Numeric)
                                {
                                    strCellValue = headerRow.GetCell(i).NumericCellValue.ToString();
                                }
                                else
                                {
                                    // 如果遇到第一个空列，则不再继续向后读取
                                    cellCount = i + 1;
                                    break;
                                }
                                if (strCellValue.Trim().Length < 1)
                                {
                                    // 如果遇到第一个空列，则不再继续向后读取
                                    cellCount = i + 1;
                                    break;
                                }
                                DataColumn column = null;
                                //string columnName = headerRow.GetCell(i).StringCellValue;
                                if (table.Columns.Contains(strCellValue))
                                {
                                    column = new DataColumn(strCellValue + i.ToString());
                                }
                                else
                                {
                                    column = new DataColumn(strCellValue);
                                }
                                // DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                                table.Columns.Add(column);
                            }
                            #endregion
                            #region 内容
                            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                            {
                                IRow row = sheet.GetRow(i);
                                if (row == null || row.GetCell(0) == null || row.GetCell(0).ToString().Trim() == "")
                                {
                                    // 如果遇到第一个空行，则不再继续向后读取
                                    break;
                                }
                                DataRow dataRow = table.NewRow();
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    if (row.GetCell(j) != null)
                                    {
                                        dataRow[j] = row.GetCell(j).ToString();
                                    }
                                }
                                table.Rows.Add(dataRow);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {

                        }
                        ds.Tables.Add(table);
                    }
                }
                catch (Exception ex)
                {
                }
                workbook = null;
                stream.Close();
            }
            return ds;

        }
        
        #endregion

        #region  导出数据

        private static void DownLoad(String fileName)
        {
            try
            {
                //FileName--要下载的文件名 
                FileInfo DownloadFile = new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/temp/" + fileName));
                if (DownloadFile.Exists)
                {
                    //System.Web.HttpContext.Current.Response.Clear();
                    //System.Web.HttpContext.Current.Response.ClearHeaders();
                    //System.Web.HttpContext.Current.Response.Buffer = false;
                    //System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    //System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
                    //System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                    //System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
                    //System.Web.HttpContext.Current.Response.Flush();
                    //System.Web.HttpContext.Current.Response.End();

                    System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                    System.Web.HttpContext.Current.Response.Clear();
                    System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
                    System.Web.HttpContext.Current.Response.End();
                }
                else
                {
                    //文件不存在

                }
            }
            catch (Exception e)
            {
                //打开时异常了
                // Alert.Show(e.ToString());
            }
        }

        #endregion

    }
}
