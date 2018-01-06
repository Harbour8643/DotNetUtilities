using System;

namespace DotNet.Utilities
{
    /// <summary>
    /// 日期类型帮助类
    /// </summary>
    public class DateTimeHelper
    {
        private readonly static int _defaultYear = 1999;//默认年
        private readonly static int _defaultMonth = 1;//默认月
        private readonly static int _defaultDay = 1;//默认天
        private readonly static int _defaultHour = 0;//默认小时
        private readonly static int _defaultMinute = 0;//默认分钟
        private readonly static int _defaultSecond = 0;//默认秒

        /// <summary>
        /// 根据时间得到DateTime
        /// </summary>
        /// <param name="Time">时间，eg:8:30</param>
        /// <returns></returns>
        public static DateTime GetDateTime(string Time)
        {
            return GetDateTime(Time, ":");
        }

        /// <summary>
        /// 根据时间得到DateTime
        /// </summary>
        /// <param name="Time">时间，eg:8:30</param>
        /// <param name="splitStr">分割符，eg:“;”</param> 
        /// <returns></returns>
        public static DateTime GetDateTime(string Time, string splitStr)
        {
            string[] HourAndMinute = StringHelper.SplitString(Time, splitStr);
            if (HourAndMinute.Length < 2)
                return GetDateTime(_defaultSecond);
            int Hour = TypeHelper.StringToInt(HourAndMinute[0], _defaultHour);
            int Minute = TypeHelper.StringToInt(HourAndMinute[1], _defaultMinute);
            return GetDateTime(Hour, Minute, _defaultSecond);
        }

        /// <summary>
        /// 获得日期
        /// </summary>
        /// <param name="Second">秒</param>
        /// <returns></returns>
        public static DateTime GetDateTime(int Second)
        {
            return GetDateTime(_defaultHour, _defaultMinute, Second);
        }

        /// <summary>
        /// 获得日期
        /// </summary>
        /// <param name="Minute">分</param>
        /// <param name="Second">秒</param>
        /// <returns></returns>
        public static DateTime GetDateTime(int Minute, int Second)
        {
            return GetDateTime(_defaultHour, Minute, Second);
        }

        /// <summary>
        /// 获得日期
        /// </summary>
        /// <param name="Hour">时</param>
        /// <param name="Minute">分</param>
        /// <param name="Second">秒</param>
        /// <returns></returns>
        public static DateTime GetDateTime(int Hour, int Minute, int Second)
        {
            return new DateTime(_defaultYear, _defaultMonth, _defaultDay, Hour, Minute, Second);
        }

        /// <summary>
        /// 返回与CompareTime相比时间差最小的时间
        /// </summary>
        /// <param name="CompareTime">被比较</param>
        /// <param name="Time1">比较者</param>
        /// <param name="time2">比较者</param>
        /// <returns></returns>
        public static DateTime GetSmallTimeSpan(DateTime CompareTime, DateTime Time1, DateTime time2)
        {
            TimeSpan TimeSpan1 = Time1 - CompareTime;
            TimeSpan TimeSpan2 = time2 - CompareTime;
            return TimeSpan.Compare(TimeSpan1, TimeSpan2) < 0 ? Time1 : time2;
        }

        /// <summary>
        /// 得到时间的小时和分钟，eg:8:09
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetDecimalHourMinute(DateTime dateTime)
        {
            string Minute = null;
            string Hour = null;
            if (dateTime.Minute < 10)
                Minute = "0" + dateTime.Minute;
            else
                Minute = dateTime.Minute.ToString();

            if (dateTime.Hour < 10)
                Hour = "0" + dateTime.Hour;
            else
                Hour = dateTime.Hour.ToString();
            return Hour + ":" + Minute;
        }

        /// <summary>
        /// 得到随机日期
        /// </summary>
        /// <param name="StartTime">起始日期</param>
        /// <param name="EndTime">结束日期</param>
        /// <returns>间隔日期之间的 随机日期</returns>
        public static DateTime GetRandomTime(DateTime StartTime, DateTime EndTime)
        {
            Random random = new Random();
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();

            System.TimeSpan ts = new System.TimeSpan(StartTime.Ticks - EndTime.Ticks);

            // 获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;

            if (dTotalSecontds > System.Int32.MaxValue)
            {
                iTotalSecontds = System.Int32.MaxValue;
            }
            else if (dTotalSecontds < System.Int32.MinValue)
            {
                iTotalSecontds = System.Int32.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }


            if (iTotalSecontds > 0)
            {
                minTime = EndTime;
                maxTime = StartTime;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = StartTime;
                maxTime = EndTime;
            }
            else
            {
                return StartTime;
            }

            int maxValue = iTotalSecontds;

            if (iTotalSecontds <= System.Int32.MinValue)
                maxValue = System.Int32.MinValue + 1;

            int i = random.Next(System.Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }

        /// <summary>
        /// 格式化日期时间
        /// </summary>
        /// <param name="dateTime">日期时间</param>
        /// <param name="dateMode">显示模式</param>
        /// <returns>0-9种模式的日期</returns>
        public static string DateTimeFormat(DateTime dateTime, DateMode dateMode)
        {
            switch (dateMode)
            {
                case DateMode.yyyy_MM_dd:
                    return dateTime.ToString("yyyy-MM-dd");
                case DateMode.yyyy_MM_dd_HH_mm_ss:
                    return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                case DateMode.yyyyMMdd:
                    return dateTime.ToString("yyyy/MM/dd");
                case DateMode.yyyy_Year_MM_Month_dd_Day:
                    return dateTime.ToString("yyyy年MM月dd日");
                case DateMode.MM_dd:
                    return dateTime.ToString("MM-dd");
                case DateMode.MMdd:
                    return dateTime.ToString("MM/dd");
                case DateMode.MM_Month_dd_Day:
                    return dateTime.ToString("MM月dd日");
                case DateMode.yyyy_MM:
                    return dateTime.ToString("yyyy-MM");
                case DateMode.yyyyMM:
                    return dateTime.ToString("yyyy/MM");
                case DateMode.yyyy_Year_MM_Month:
                    return dateTime.ToString("yyyy年MM月");
                case DateMode.Default:
                    return dateTime.ToString();
                default: return dateTime.ToString();
            }
        }

        /// <summary>
        /// 是否为同一天
        /// </summary>
        /// <param name="DateTime1">DateTime1</param>
        /// <param name="DateTime2">DateTime2</param>
        /// <returns></returns>
        public static bool IsSameDay(DateTime DateTime1, DateTime DateTime2)
        {
            bool bol = DateTime1.Year == DateTime2.Year && DateTime1.Month == DateTime2.Month && DateTime1.Day == DateTime2.Day;
            if (bol)
                return true;
            else
                return false;
        }

    }

    /// <summary>
    /// 日期模式
    /// </summary>
    public enum DateMode
    {
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        yyyy_MM_dd = 0,

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        yyyy_MM_dd_HH_mm_ss = 1,

        /// <summary>
        /// yyyy/MM/dd
        /// </summary>
        yyyyMMdd = 2,

        /// <summary>
        /// yyyy年MM月dd日
        /// </summary>
        yyyy_Year_MM_Month_dd_Day = 3,

        /// <summary>
        /// MM-dd
        /// </summary>
        MM_dd = 4,

        /// <summary>
        /// MM/dd
        /// </summary>
        MMdd = 5,

        /// <summary>
        /// MM月dd日
        /// </summary>
        MM_Month_dd_Day = 6,

        /// <summary>
        /// yyyy-MM
        /// </summary>
        yyyy_MM = 7,

        /// <summary>
        /// yyyy/MM
        /// </summary>
        yyyyMM = 8,

        /// <summary>
        /// yyyy年MM月
        /// </summary>
        yyyy_Year_MM_Month = 9,

        /// <summary>
        /// 默认
        /// </summary>
        Default = 10

    }

}
