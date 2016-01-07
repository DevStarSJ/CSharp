using System;

namespace LunaStar.Util
{
    class Ut_Date
    {

        /// <summary>  날짜사이의시간간격의숫자를반환합니다.
        /// </summary>
        /// <param name="Interval">y-m-d h:n:s:ms</param>
        /// <param name="Date1"></param>
        /// <param name="Date2"></param>
        /// <returns></returns>
        public static double DateDiff(string Interval, DateTime Date1, DateTime Date2)
        {

            double diff = 0;

            TimeSpan ts = Date2 - Date1;

            switch (Interval.ToLower())
            {
                case "y":
                    ts = DateTime.Parse(Date2.ToString("yyyy-01-01")) - DateTime.Parse(Date1.ToString("yyyy-01-01"));
                    diff = Convert.ToDouble(ts.TotalDays / 365);
                    break;
                case "m":
                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-01")) - DateTime.Parse(Date1.ToString("yyyy-MM-01"));
                    diff = Convert.ToDouble((ts.TotalDays / 365) * 12);
                    break;
                case "d":
                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-dd")) - DateTime.Parse(Date1.ToString("yyyy-MM-dd"));
                    diff = ts.Days;
                    break;
                case "h":
                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-dd HH:00:00")) - DateTime.Parse(Date1.ToString("yyyy-MM-dd HH:00:00"));
                    diff = ts.TotalHours;
                    break;
                case "n":
                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-dd HH:mm:00")) - DateTime.Parse(Date1.ToString("yyyy-MM-dd HH:mm:00"));
                    diff = ts.TotalMinutes;
                    break;
                case "s":
                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-dd HH:mm:ss")) - DateTime.Parse(Date1.ToString("yyyy-MM-dd HH:mm:ss"));
                    diff = ts.TotalSeconds;
                    break;
                case "ms":
                    diff = ts.TotalMilliseconds;
                    break;
            }

            return diff;
        }
    }
}
