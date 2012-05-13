using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Data;
using System.Data.OleDb;

namespace TQuery.Net
{
    public class ExcelHelper
    {
        /// <summary>
        /// 创建CSV格式的Excel
        /// </summary>
        /// <param name="dt">数据</param>
        /// <param name="fileName">生成的Excel文件名</param>
        /// <param name="titleColumns">Excel标题名称组</param>
        /// <param name="columnNames">数据字段名称组</param>
        /// <param name="columnTypes">数据类型名称组</param>
        public static void CreateCSVExcel(DataTable dt, string fileName, string titleColumns, string columnNames, string columnTypes)
        {
            string[] titleColumnCollection = titleColumns.Split(',');
            string[] columnNameCollection = columnNames.Split(',');
            string[] columnTypeCollection = columnTypes.Split(',');

            if (!
                ((columnNameCollection.Length == columnTypeCollection.Length)
                && (columnNameCollection.Length == titleColumnCollection.Length))
                )
                throw new Exception("Excel设置的数据参数有误");

            HttpResponse resp;
            resp = HttpContext.Current.Response;
            resp.Buffer = true;
            resp.ContentType = "application/vnd.ms-excel";
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));

            StringWriter sw = new StringWriter();

            sw.WriteLine(titleColumns);

            foreach (DataRow eachRow in dt.Rows)
            {
                sw.WriteLine(GetEachRowExcelData(eachRow, columnNameCollection, columnTypeCollection));
            }
            sw.Close();
            resp.Write(sw);
            resp.End();
        }

        /// <summary>
        /// 创建CSV格式的Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        public static void CreateCSVExcel(DataTable dt, string fileName)
        {
            string titleColumns = String.Empty;
            string columnNames = String.Empty;
            string columnTypes = String.Empty;
            foreach (DataColumn col in dt.Columns)
            {
                if (titleColumns != String.Empty)
                    titleColumns += ",";
                titleColumns += col.ColumnName;
                if (columnTypes != String.Empty)
                    columnTypes += ",";
                columnTypes += "String";
            }
            columnNames = titleColumns;

            string[] columnNameCollection = columnNames.Split(',');
            string[] columnTypeCollection = columnTypes.Split(',');

            HttpResponse resp;
            resp = HttpContext.Current.Response;
            resp.Buffer = true;
            resp.ContentType = "application/vnd.ms-excel";
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));

            StringWriter sw = new StringWriter();

            sw.WriteLine(titleColumns);

            foreach (DataRow eachRow in dt.Rows)
            {
                sw.WriteLine(GetEachRowExcelData(eachRow, columnNameCollection, columnTypeCollection));
            }
            sw.Close();
            resp.Write(sw);
            resp.End();
        }

        private static string GetEachRowExcelData(DataRow row, string[] columnNameCollection, string[] columnTypeCollection)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < columnNameCollection.Length; i++)
            {
                if (columnTypeCollection[i] == "Data")
                    result.Append(row[columnNameCollection[i]].ToString());
                else if (columnTypeCollection[i] == "String")
                    result.Append(ToText(row[columnNameCollection[i]].ToString()));

                if (i < columnNameCollection.Length - 1)
                    result.Append(",");
            }

            return result.ToString();
        }

        private static string ToText(string number)
        {
            return "=\"" + number.Replace(',', '，') + "\"";
        }

        /// <summary>
        /// 将两个DataTable创建CSV格式的Excel
        /// </summary>
        /// <param name="dt1">数据1</param>
        /// <param name="fileName1">生成的Excel文件名</param>
        /// <param name="titleColumns1">Excel标题名称组1</param>
        /// <param name="columnNames1">数据字段名称组1</param>
        /// <param name="columnTypes1">数据类型名称组1</param>
        /// <param name="dt2">数据2</param>
        /// <param name="titleColumns2">Excel标题名称组2</param>
        /// <param name="columnNames2">数据字段名称组2</param>
        /// <param name="columnTypes2">数据类型名称组2</param>
        public static void CreateCSVExcelFromTwoDT(DataTable dt1, string fileName, string titleColumns1, string columnNames1, string columnTypes1,
            DataTable dt2, string titleColumns2, string columnNames2, string columnTypes2)
        {
            string[] titleColumnCollection1 = titleColumns1.Split(',');
            string[] columnNameCollection1 = columnNames1.Split(',');
            string[] columnTypeCollection1 = columnTypes1.Split(',');

            string[] titleColumnCollection2 = titleColumns2.Split(',');
            string[] columnNameCollection2 = columnNames2.Split(',');
            string[] columnTypeCollection2 = columnTypes2.Split(',');

            if (!
                ((columnNameCollection1.Length == columnTypeCollection1.Length)
                && (columnNameCollection1.Length == titleColumnCollection1.Length) &&
                (columnNameCollection2.Length == columnTypeCollection2.Length)
                && (columnNameCollection2.Length == titleColumnCollection2.Length))
                )
                throw new Exception("Excel设置的数据参数有误");

            HttpResponse resp;
            resp = HttpContext.Current.Response;
            resp.Buffer = true;
            resp.ContentType = "application/vnd.ms-excel";
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));

            StringWriter sw = new StringWriter();

            sw.WriteLine(titleColumns1);

            foreach (DataRow eachRow in dt1.Rows)
            {
                sw.WriteLine(GetEachRowExcelData(eachRow, columnNameCollection1, columnTypeCollection1));
            }
            sw.WriteLine();
            sw.WriteLine(titleColumns2);

            foreach (DataRow eachRow in dt2.Rows)
            {
                sw.WriteLine(GetEachRowExcelData(eachRow, columnNameCollection2, columnTypeCollection2));
            }

            sw.Close();
            resp.Write(sw);
            resp.End();
        }

        /// <summary>
        /// 读取Excel内容到DataSet
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataSet GetExcelDataToDataSet(string fileName)
        {
            DataSet ds = new DataSet();
            //For Excel2003
            //string connString = String.Format(
            //    @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'", fileName);
            //For Excel2007
            string connString = String.Format(
                @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'", fileName);
            try
            {
                OleDbConnection conn = new OleDbConnection(connString);
                conn.Open();
                DataTable sheetNames = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                //读取第一个Sheet页数据
                string sheetName = sheetNames.Rows[0]["Table_Name"].ToString();
                OleDbDataAdapter oada = new OleDbDataAdapter(String.Format("select * from [{0}]", sheetName), conn);
                oada.Fill(ds);
                conn.Close();
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("读取文件{0}失败：{1}", fileName, e.Message));
            }

            return ds;
        }

        /// <summary>
        /// 读取CSV文件内容到DataSet
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataSet GetCsvDataToDataSet(string fileName)
        {
            DataSet ds = new DataSet();
            if (File.Exists(fileName))
            {
                try
                {
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
                    //表头
                    string strHeaderLine = sr.ReadLine();
                    DataTable dtDAta = CreateDataTable(strHeaderLine);
                    //获取数据
                    for (string strLine = sr.ReadLine(); !string.IsNullOrEmpty(strLine); strLine = sr.ReadLine())
                    {
                        strLine = strLine.Replace("=", "");
                        strLine = strLine.Replace("\"", "");
                        AddRowData(strLine, ref dtDAta);
                    }
                    //添加数据行
                    ds.Tables.Add(dtDAta);
                    //关闭文件流
                    sr.Close();
                    fs.Close();
                }
                catch (Exception e)
                {
                    throw new Exception(String.Format("读取文件{0}失败：{1}", fileName, e.Message));
                }
            }
            else
            {
                throw new Exception(String.Format("文件{0}不存在", fileName));
            }
            return ds;
        }

        /// <summary>
        /// 根据表头生成DataTable
        /// </summary>
        /// <param name="FieldName">列名，例如"创建人,创建时间"</param>
        /// <returns></returns>
        public static DataTable CreateDataTable(string FieldName)
        {
            DataTable dt = new DataTable();
            string[] Fields = FieldName.Split(',');
            foreach (string field in Fields)
            {
                dt.Columns.Add(field);
            }

            return dt;
        }
        /// <summary>
        /// 给DataTable添加数据行
        /// </summary>
        /// <param name="FieldValue">数据，例如"0101,济南,1,2,3,test,2011/4/8 10:50:15"</param>
        /// <param name="psDataTable"></param>
        public static void AddRowData(string FieldValue, ref DataTable psDataTable)
        {
            string[] Fields = FieldValue.Split(',');
            DataRow row = psDataTable.NewRow();
            int colIndex = 0;
            foreach (DataColumn col in psDataTable.Columns)
            {
                row[col.ColumnName] = Fields[colIndex];
                colIndex++;
            }
            //添加行
            psDataTable.Rows.Add(row);
        }

        public static void FileDownload(string path)
        {
            string fileName = Path.GetFileName(path);
            if (HttpContext.Current.Request.UserAgent.Contains("MSIE") || HttpContext.Current.Request.UserAgent.Contains("msie"))
            {
                fileName = HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
            }
            HttpContext.Current.Response.ContentType = "application/ms-download";
            FileInfo file = new FileInfo(path);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Type", "application/ms-excel");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
            HttpContext.Current.Response.WriteFile(file.FullName);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.End();
        }
    }
}