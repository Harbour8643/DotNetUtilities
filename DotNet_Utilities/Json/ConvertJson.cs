using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Data.Common;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Web.Script.Serialization;

namespace DotNet.Utilities
{
    /// <summary>
    /// JSON转换类
    /// </summary>
    public class ConvertJson
    {
        #region 私有方法

        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 格式化字符型、日期型、布尔型
        /// </summary>
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (type != typeof(string) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            return str;
        }

        #endregion

        #region Json反序列化

        /// <summary>
        /// 将json转为model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static T JsonToMOdel<T>(string Json)
        {
            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
            return jsonSerialize.Deserialize<T>(Json);
        }

        #endregion


        #region Model和ModelList转成Json

        /// <summary>
        /// 将model序列化为json
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ModelToJson(object model)
        {
            JavaScriptSerializer JS = new JavaScriptSerializer();
            return JS.Serialize(model);
        }

        /// <summary>
        /// 将model序列化为jsonArry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ModelToJsonArry(object model)
        {
            JavaScriptSerializer JS = new JavaScriptSerializer();
            string json = JS.Serialize(model);
            json = StringHelper.TrimStart(json, "{");
            json = StringHelper.TrimEnd(json, "}");
            return "[{" + json + "}]";
        }

        /// <summary>
        /// 将model序列化为json(只有属性)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ModelToJsonAttribute(object model)
        {
            JavaScriptSerializer JS = new JavaScriptSerializer();
            string json = JS.Serialize(model);
            json = StringHelper.TrimStart(json, "{");
            json = StringHelper.TrimEnd(json, "}");
            return json;
        }



        /// <summary>
        /// Model转化成Json数组
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="ModelList">ModelList</param>
        /// <returns></returns>
        public static string ModelListToJsonArry<T>(List<T> ModelList) where T : class
        {
            //if (ModelList == null)
            //    return null;
            //else if (ModelList.Count <= 0)
            //    return null;
            JavaScriptSerializer JS = new JavaScriptSerializer();
            return JS.Serialize(ModelList);
        }

        /// <summary>
        /// ModelList转换成Json
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="ModelList">ModelList</param>
        /// <returns></returns>
        public static string ModelListToJson<T>(List<T> ModelList) where T : class
        {
            return ModelListToJson(ModelList, "List");
        }

        /// <summary>
        /// ModelList转换成Json
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="ModelList">ModelList</param>
        /// <param name="jsonName">Json名</param>
        /// <returns></returns>
        public static string ModelListToJson<T>(List<T> ModelList, string jsonName) where T : class
        {
            string jsString = ModelListToJsonArry(ModelList);
            if (!string.IsNullOrEmpty(jsString))
            {
                jsString = "{\"" + jsonName + "\":" + jsString + "}";
            }
            return jsString;
        }

        #endregion


        #region List转换成Json

        /// <summary>
        /// List转换成Json
        /// </summary>
        public static string ListToJson<T>(IList<T> list)
        {
            object obj = list[0];
            return ListToJson<T>(list, obj.GetType().Name);
        }

        /// <summary>
        /// List转换成Json 
        /// </summary>
        public static string ListToJson<T>(IList<T> list, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = list[0].GetType().Name;

            Json.Append("{\"" + jsonName + "\":[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        Type type = pi[j].GetValue(list[i], null).GetType();
                        Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));

                        if (j < pi.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        /// <summary>
        /// List转换成JsonArry 
        /// </summary>
        public static string ListToJsonArry<T>(IList<T> list)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        Type type = pi[j].GetValue(list[i], null).GetType();
                        Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));

                        if (j < pi.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]");
            return Json.ToString();
        }

        #endregion

        #region 对象转换为Json
        /// <summary> 
        /// 对象转换为Json 
        /// </summary> 
        /// <param name="jsonObject">对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(object jsonObject)
        {
            string jsonString = "{";
            PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();//返回所有公共属性
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
                string value = string.Empty;
                if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
                {
                    value = "'" + objectValue.ToString() + "'";
                }
                else if (objectValue is string)
                {
                    value = "'" + ToJson(objectValue.ToString()) + "'";
                }
                else if (objectValue is IEnumerable)
                {
                    value = ToJson((IEnumerable)objectValue);
                }
                else
                {
                    value = ToJson(objectValue.ToString());
                }
                jsonString += "\"" + ToJson(propertyInfo[i].Name) + "\":" + value + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "}";
        }
        #endregion

        #region 对象集合转换Json
        /// <summary> 
        /// 对象集合转换Json 
        /// </summary> 
        /// <param name="array">集合对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString += ToJson(item) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }
        #endregion

        #region 普通集合转换Json
        /// <summary> 
        /// 普通集合转换Json 
        /// </summary> 
        /// <param name="array">集合对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToArrayString(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString = ToJson(item.ToString()) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }
        #endregion

        #region  DataSet转换为Json
        /// <summary> 
        /// DataSet转换为Json 
        /// </summary> 
        /// <param name="dataSet">DataSet对象</param> 
        /// <returns>Json字符串</returns> 
        public static string DataSetToJson(DataSet dataSet)
        {
            string jsonString = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += "\"" + table.TableName + "\":" + DataTableToJsonArry(table) + ",";
            }
            jsonString = jsonString.TrimEnd(',');
            return jsonString + "}";
        }
        #endregion

        #region Datatable转换为Json

        /// <summary>
        /// DataTable转换为Json 
        /// </summary>
        public static string DataTableToJson(DataTable dt)
        {
            return DataTableToJson(dt, "Table" + dt.TableName);
        }

        /// <summary>
        /// DataTable转换为Json 
        /// </summary>
        public static string DataTableToJson(DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName)) jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":");
            Json.Append(DataTableToJsonArry(dt));
            Json.Append("}");
            //if (dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        Json.Append("{");
            //        for (int j = 0; j < dt.Columns.Count; j++)
            //        {
            //            Type type = dt.Rows[i][j].GetType();
            //            Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
            //            if (j < dt.Columns.Count - 1)
            //            {
            //                Json.Append(",");
            //            }
            //        }
            //        Json.Append("}");
            //        if (i < dt.Rows.Count - 1)
            //        {
            //            Json.Append(",");
            //        }
            //    }
            //}
            //Json.Append("]}");
            return Json.ToString();
        }

        /// <summary> 
        /// Datatable转换为Json数组 
        /// </summary> 
        /// <param name="dt">Datatable对象</param> 
        /// <returns>Json字符串</returns> 
        public static string DataTableToJsonArry(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }


        #endregion

        #region DataReader转换为Json

        /// <summary> 
        /// DataReader转换为Json数组 
        /// </summary> 
        /// <param name="dataReader">DataReader对象</param> 
        /// <returns>Json字符串</returns> 
        public static string DataReaderToJsonArry(DbDataReader dataReader)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            while (dataReader.Read())
            {
                jsonString.Append("{");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Type type = dataReader.GetFieldType(i);
                    string strKey = dataReader.GetName(i);
                    string strValue = dataReader[i].ToString();
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (i < dataReader.FieldCount - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            dataReader.Close();
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }

        #endregion
    }
}