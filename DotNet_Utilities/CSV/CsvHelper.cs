using System.Data;
using System.IO;

namespace DotNet.Utilities
{
    /// <summary>
    /// CSV文件转换类
    /// </summary>
    public static class CSVHelper
    {
        /// <summary>
        /// 导出报表为CSV
        /// </summary>
        /// <param name="dtSource">DataTable</param>
        /// <param name="strFilePath">物理路径</param>
        /// <param name="tableHeader">表头</param>
        /// <param name="columName">字段标题,逗号分隔</param>
        public static bool ExportCSV(DataTable dtSource, string strFilePath, string tableHeader, string columName)
        {
            try
            {
                string strBufferLine = "";
                StreamWriter strmWriterObj = new StreamWriter(strFilePath, false, System.Text.Encoding.UTF8);
                strmWriterObj.WriteLine(tableHeader);
                strmWriterObj.WriteLine(columName);
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    strBufferLine = "";
                    for (int j = 0; j < dtSource.Columns.Count; j++)
                    {
                        if (j > 0)
                            strBufferLine += ",";
                        strBufferLine += dtSource.Rows[i][j].ToString();
                    }
                    strmWriterObj.WriteLine(strBufferLine);
                }
                strmWriterObj.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将Csv读入DataTable
        /// </summary>
        /// <param name="filePath">csv文件路径</param>
        /// <param name="Index">表示第n行是字段title,第n+1行是记录开始</param>
        /// <param name="dtTemp">DataTable模板</param>
        public static DataTable ImportCSV(string filePath, int Index, DataTable dtTemp)
        {
            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.UTF8, false);
            int i = 0, m = 0;
            reader.Peek();
            while (reader.Peek() > 0)
            {
                m = m + 1;
                string str = reader.ReadLine();
                if (m >= Index + 1)
                {
                    string[] split = str.Split(',');

                    System.Data.DataRow dr = dtTemp.NewRow();
                    for (i = 0; i < split.Length; i++)
                    {
                        dr[i] = split[i];
                    }
                    dtTemp.Rows.Add(dr);
                }
            }
            return dtTemp;
        }
    }
}
