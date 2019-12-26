using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Common
{
    /// <summary>
    /// 日期时间帮助类
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 1970年1月1日 （可用于解决系统默认日期0001/1/1插入数据库错误的问题）
        /// </summary>
        public static readonly DateTime DefaultDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 时间更新计时器
        /// </summary>
        private static readonly Timer _timer = null;
        /// <summary>
        /// 时间更新委托句柄
        /// </summary>
        /// <param name="now"></param>
        public delegate void NowUpdateHandler(DateTime now);
        /// <summary>
        /// 当前时间通过秒更新事件
        /// </summary>
        public static event NowUpdateHandler NowUpdateBySec;

        private static DateTime _now = DefaultDateTime;

        static DateTimeHelper()
        {
            _timer = new Timer(state =>
            {
                if (_now == DefaultDateTime)
                    _now = DateTime.Now;
                else
                    _now = _now.AddSeconds(1); 
                new Task(() => NowUpdateBySec?.Invoke(_now)).Start();
            }, null, 1000, 1000);
        }

        /// <summary>
        /// 当前时间
        /// </summary>
        public static DateTime Now
        {
            get { return DateTimeNow; }
            set
            {
                _now = value;
                new Task(() => NowUpdateBySec?.Invoke(_now)).Start();
            }
        }

        /// <summary>
        /// 日期时间格式化字符串
        /// </summary>
        public static string FormatDateTimeStr
        {
            get { return "yyyy-MM-dd HH:mm:ss"; }
        }

        /// <summary>
        /// 日期格式化字符串
        /// </summary>
        public static string FormatDateStr
        { get { return "yyyy-MM-dd"; } }

        /// <summary>
        /// 时间格式化字符串
        /// </summary>
        public static string FormatTimeStr
        {
            get { return "HH:mm:ss"; }
        }

        private static string _formatDayBeginStr = "yyyy-MM-dd 00:00:00";
        /// <summary>
        /// 当天开始日期格式化字符串
        /// </summary>
        public static string FormatDayBeginStr
        {
            get { return _formatDayBeginStr; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;
                _formatDayBeginStr = value;
            }
        }

        private static string _formatDayEndStr = "yyyy-MM-dd 23:59:59";
        /// <summary>
        /// 当天结束日期格式化字符串
        /// </summary>
        public static string FormatDayEndStr
        {
            get { return _formatDayEndStr; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;
                _formatDayEndStr = value;
            }
        }

        /// <summary>
        /// 现在日期时间
        /// </summary>
        public static DateTime DateTimeNow
        {
            get
            {
                if (_now != DefaultDateTime)
                    return _now;
                else
                    return DateTime.Now;
            }
        }

        /// <summary>
        /// 现在日期时间字符串格式
        /// </summary>
        public static string DateTimeNowStr
        {
            get { return DateTimeNow.ToString(FormatDateTimeStr); }
        }

        /// <summary>
        /// 当天日期
        /// </summary>
        public static DateTime TodayDate
        {
            get { return DateTime.Today; }
        }

        /// <summary>
        /// 当天日期字符串
        /// </summary>
        public static string TodayDateStr
        {
            get { return TodayDate.ToString(FormatDateStr); }
        }

        /// <summary>
        /// 现在时间
        /// </summary>
        public static TimeSpan TimeNow
        {
            get { return DateTimeNow.TimeOfDay; }
        }

        /// <summary>
        /// 现在时间字符串
        /// </summary>
        public static string TimeNowStr
        { get { return DateTimeNow.ToString(FormatTimeStr); } }

        /// <summary>
        /// 获取当天起始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetDayBegin(DateTime dt)
        {
            return StringToDateTime(dt.ToString(FormatDayBeginStr));
        }
        /// <summary>
        /// 获取当天起始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToDayBegin(this DateTime dt)
        {
            return GetDayBegin(dt);
        }

        /// <summary>
        /// 获取当天起始时间字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetDayBeginStr(DateTime dt)
        {
            return dt.ToString(FormatDayBeginStr);
        }

        /// <summary>
        /// 今天起始时间
        /// </summary>
        public static DateTime TodayBegin
        {
            get { return GetDayBegin(DateTimeNow); }
        }

        /// <summary>
        /// 今天起始时间字符串
        /// </summary>
        public static string TodayBeginStr
        {
            get { return GetDayBeginStr(DateTimeNow); }
        }

        /// <summary>
        /// 获取当天结束时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetDayEnd(DateTime dt)
        {
            return StringToDateTime(dt.ToString(FormatDayEndStr), "yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 获取当天结束时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToDayEnd(this DateTime dt)
        {
            return GetDayEnd(dt);
        }

        /// <summary>
        /// 获取当天结束时间字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetDayEndStr(DateTime dt)
        {
            return dt.ToString(FormatDayEndStr);
        }

        /// <summary>
        /// 今天结束时间
        /// </summary>
        public static DateTime TodayEnd
        {
            get { return GetDayEnd(DateTimeNow); }
        }

        /// <summary>
        /// 今天结束时间字符串
        /// </summary>
        public static string TodayEndStr
        {
            get
            {
                return GetDayEndStr(DateTimeNow);
            }
        }
        /// <summary>
        /// 本月最后时间
        /// </summary>
        public static DateTime MonthEnd
        {
            get { return GetDayEnd(new DateTime(DateTimeNow.Year, DateTimeNow.Month, DateTime.DaysInMonth(DateTimeNow.Year, DateTimeNow.Month))); }
        }

        /// <summary>
        /// 获取时间日期字符串格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetDateTimeStr(DateTime dt)
        {
            return dt.ToString(FormatDateTimeStr);
        }

        /// <summary>
        /// 将时间格式化成指定格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToFormatDateTimeStr(this DateTime time)
        {
            if (time.IsValid())
                return time.ToString(FormatDateTimeStr);
            return "";
        }

        /// <summary>
        /// 将时间格式化成指定格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToFormatDateStr(this DateTime time)
        {
            return time.ToString(FormatDateStr);
        }

        /// <summary>
        /// 将时间格式化成指定格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToFormatDayBeginStr(this DateTime time)
        {
            return time.ToString(FormatDayBeginStr);
        }

        /// <summary>
        /// 将时间格式化成指定格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToFormatDayEndStr(this DateTime time)
        {
            return time.ToString(FormatDayEndStr);
        }

        /// <summary>
        /// 将时间格式化成指定格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToFormatTimeStr(this DateTime time)
        {
            return time.ToString(FormatTimeStr);
        }

        /// <summary>
        /// datetime转换为时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static ulong DateTimeToTimestamp(DateTime time, bool isMilsec = false)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = time.ToUniversalTime() - startTime;
            if (isMilsec)
                return (ulong)span.TotalMilliseconds;
            else
                return (ulong)span.TotalSeconds;
        }
        /// <summary>
        /// DateTime时间戳转换
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static ulong ToTimestamp(this DateTime time, bool isMilsec = false)
        {
            return DateTimeToTimestamp(time, isMilsec);
        }
        /// <summary>
        /// 时间戳转换为datetime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime TimestampToDateTime(ulong timestamp, bool isMilsec = false)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan toNow = TimeSpan.Zero;
            if (isMilsec)
                toNow = TimeSpan.FromMilliseconds(timestamp);
            else
                toNow = TimeSpan.FromSeconds(timestamp);
            return startTime.Add(toNow).ToLocalTime();
        }
        /// <summary>
        /// 当前时间的时间戳
        /// </summary>
        public static ulong NowTimestamp
        {
            get { return DateTime.UtcNow.ToTimestamp(); }
        }

        /// <summary>
        /// 把字符串转换为时间
        /// </summary>
        /// <param name="timestr">时间字符串</param>
        /// <param name="formatstr">格式化字符串</param>
        /// <returns></returns>
        public static DateTime StringToDateTime(string timestr, string formatstr = "yyyy-MM-dd HH:mm:ss")
        {
            DateTime result = default(DateTime);
            try
            {
                result = DateTime.ParseExact(timestr, formatstr, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return result;
        }

        /// <summary>
        /// 把字符串转换为时间
        /// </summary>
        /// <param name="timestr">时间字符串</param>
        /// <param name="formatstr">格式化字符串</param>
        /// <returns></returns>
        public static DateTime FromString(string timestr, string formatstr = "yyyy-MM-dd HH:mm:ss")
        {
            return StringToDateTime(timestr, formatstr);
        }

        /// <summary>
        /// 转换成当月第一天日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetBeginMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        /// <summary>
        /// 转换成当月第一天日期
        /// </summary>
        public static DateTime ToBeginMonth(this DateTime date)
        {
            return GetBeginMonth(date);
        }

        /// <summary>
        /// 转换成当月第一天日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetEndMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59, 999);
        }
        /// <summary>
        /// 转换成当月第一天日期
        /// </summary>
        public static DateTime ToEndMonth(this DateTime date)
        {
            return GetEndMonth(date);
        }

        /// <summary>
        /// 验证时间是否有效
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static bool Valid(DateTime datetime)
        {
            if (datetime == default(DateTime) || datetime == DateTime.MinValue || datetime == DateTime.MaxValue || datetime == DefaultDateTime)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证时间是否有效
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static bool IsValid(this DateTime datetime)
        {
            return Valid(datetime);
        }

        /// <summary>
        /// TimeSpan转换位字符串形式
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static string TimeSpanToString(long ticks)
        {
            TimeSpan ts = TimeSpan.FromTicks(ticks);
            return TimeSpanToString(ts);
        }

        /// <summary>
        /// TimeSpan转换位字符串形式
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public static string TimeSpanToString(TimeSpan ts)
        {
            if (ts.Ticks == 0)
            {
                return "0秒";
            }

            string r = "";
            if (ts.Days > 0)
                r = ts.Days + "天";
            if (r.Length > 0 || ts.Hours > 0)
                r += ts.Hours + "小时";
            if (r.Length > 0 || ts.Minutes > 0)
                r += ts.Minutes + "分";
            if (ts.Seconds > 0)
                r += ts.Seconds + "秒";

            return r;
        }
        /// <summary>
        /// 输出格式化字符串
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string ToFormatString(this TimeSpan ts)
        {
            return TimeSpanToString(ts);
        }
    }

}