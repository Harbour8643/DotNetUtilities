using System;

namespace DotNet.Utilities
{
    /// <summary>
    /// 类型帮助类
    /// </summary>
    public class TypeHelper
    {
        #region 转Int

        /// <summary>
        /// 将string类型转换成int类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int StringToInt(string s, int defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                int result;
                if (int.TryParse(s, out result))
                    return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// 将string类型转换成int类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <returns></returns>
        public static int StringToInt(string s)
        {
            return StringToInt(s, 0);
        }

        /// <summary>
        /// 将object类型转换成int类型
        /// </summary>
        /// <param name="Object">目标对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int ObjectToInt(object Object, int defaultValue)
        {
            if (Object != null)
                return StringToInt(Object.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 将object类型转换成int类型
        /// </summary>
        /// <param name="Object">目标对象</param>
        /// <returns></returns>
        public static int ObjectToInt(object Object)
        {
            return ObjectToInt(Object, 0);
        }

        /// <summary>
        ///Harbour 将指定的字符串数组转换成整型数组
        /// </summary>
        /// <param name="sourceStr">字符串数组</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int[] StringArrayToIntArray(string[] sourceStr, int defaultValue)
        {
            if (sourceStr == null)
                return new int[0] { };
            int[] retInt = new Int32[sourceStr.Length];
            for (int i = 0; i < sourceStr.Length; i++)
            {
                retInt[i] = StringToInt(sourceStr[i], defaultValue);
            }
            return retInt;
        }

        /// <summary>
        /// Harbour 将指定的字符串数组转换成整型数组
        /// </summary>
        /// <param name="sourceStr">字符串数组</param>
        /// <returns></returns>
        public static int[] StringArrayToIntArray(string[] sourceStr)
        {
            return StringArrayToIntArray(sourceStr, -1);
        }

        /// <summary>
        /// int?转成int
        /// </summary>
        /// <param name="Int"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int IntObjectToInt(int? Int, int defaultValue)
        {
            if (Int != null)
                return StringToInt(Int.ToString(), defaultValue);
            else
                return defaultValue;
        }

        /// <summary>
        /// int?转成int
        /// </summary>
        /// <param name="Int"></param>
        /// <returns></returns>
        public static int IntObjectToInt(int? Int)
        {
            return IntObjectToInt(Int, 0);
        }

        #endregion

        #region 转Bool

        /// <summary>
        /// 将string类型转换成bool类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool StringToBool(string s, bool defaultValue)
        {
            s = s.ToLower().Trim();
            if (s == "false")
                return false;
            else if (s == "true")
                return true;

            return defaultValue;
        }

        /// <summary>
        /// 将string类型转换成bool类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <returns></returns>
        public static bool ToBool(string s)
        {
            return StringToBool(s, false);
        }

        /// <summary>
        /// 将object类型转换成bool类型
        /// </summary>
        /// <param name="Object">目标对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool ObjectToBool(object Object, bool defaultValue)
        {
            if (Object != null)
                return StringToBool(Object.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 将object类型转换成bool类型
        /// </summary>
        /// <param name="Object">目标对象</param>
        /// <returns></returns>
        public static bool ObjectToBool(object Object)
        {
            return ObjectToBool(Object, false);
        }

        #endregion

        #region 转DateTime

        /// <summary>
        /// 将string类型转换成datetime类型
        /// </summary>
        /// <param name="String">目标字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static DateTime StringToDateTime(string String, DateTime defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(String))
            {
                DateTime result;
                if (DateTime.TryParse(String, out result))
                    return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// 将string类型转换成datetime类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <returns></returns>
        public static DateTime StringToDateTime(string s)
        {
            return StringToDateTime(s, DateTime.Now);
        }

        /// <summary>
        /// 将object类型转换成datetime类型
        /// </summary>
        /// <param name="Object">目标对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static DateTime ObjectToDateTime(object Object, DateTime defaultValue)
        {
            if (Object != null)
                return StringToDateTime(Object.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 将object类型转换成datetime类型
        /// </summary>
        /// <param name="Object">目标对象</param>
        /// <returns></returns>
        public static DateTime ObjectToDateTime(object Object)
        {
            return ObjectToDateTime(Object, DateTime.Now);
        }

        #endregion

        #region 转Decimal

        /// <summary>
        /// 将string类型转换成decimal类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static decimal StringToDecimal(string s, decimal defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                decimal result;
                if (decimal.TryParse(s, out result))
                    return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// 将string类型转换成decimal类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <returns></returns>
        public static decimal StringToDecimal(string s)
        {
            return StringToDecimal(s, 0m);
        }

        /// <summary>
        /// 将object类型转换成decimal类型
        /// </summary>
        /// <param name="Object">目标对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static decimal ObjectToDecimal(object Object, decimal defaultValue)
        {
            if (Object != null)
                return StringToDecimal(Object.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 将object类型转换成decimal类型
        /// </summary>
        /// <param name="Object">目标对象</param>
        /// <returns></returns>
        public static decimal ObjectToDecimal(object Object)
        {
            return ObjectToDecimal(Object, 0m);
        }

        #endregion

        #region 转String

        /// <summary>
        /// 将int数组按指定分隔符转换成字符串
        /// </summary>
        /// <param name="ints">int s数组</param>
        /// <param name="sign">分隔符</param>
        /// <returns></returns>
        public static string IntToStringBySign(int[] ints, string sign)
        {
            return IntToStringBySign(ints, sign, null);
        }

        /// <summary>
        /// 将int数组按指定分隔符转换成字符串
        /// </summary>
        /// <param name="ints">int s数组</param>
        /// <param name="sign">分隔符</param>
        /// <param name="defaultStr">默认字符</param>
        /// <returns></returns>
        public static string IntToStringBySign(int[] ints, string sign, string defaultStr)
        {
            string ret = defaultStr;
            if (ints == null || string.IsNullOrEmpty(sign))
                return ret;
            foreach (int t in ints)
            {
                if (ret == defaultStr)
                    ret = t.ToString() + sign;
                else
                    ret = ret + t.ToString() + sign;
            }
            return ret == defaultStr ? ret : ret.Substring(0, ret.Length - sign.Length);
        }


        /// <summary>
        /// 将string数组按指定分隔符转换成字符串
        /// </summary>
        /// <param name="ints">string s数组</param>
        /// <param name="sign">分隔符</param>
        /// <returns></returns>
        public static string StringArryToStringBySign(string[] ints, string sign)
        {
            string ret = null;
            if (ints == null || string.IsNullOrEmpty(sign))
                return ret;
            foreach (string t in ints)
            {
                if (ret == null)
                    ret = t.ToString() + sign;
                else
                    ret = ret + t.ToString() + sign;
            }
            return ret == null ? ret : ret.Substring(0, ret.Length - sign.Length);
        }

        /// <summary>
        /// Object转成String
        /// </summary>
        /// <param name="ob">Object 对象</param>
        /// <returns>空对象返回Empty</returns>
        public static string ObjectToString(object ob)
        {
            return null == ob ? String.Empty : ob.ToString();
        }


        #endregion


    }
}
