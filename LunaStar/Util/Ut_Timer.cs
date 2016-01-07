using System;
using System.Threading;

namespace LunaStar.Util
{
    public class Ut_Timer
    {
        /// <SUMMARY>
        /// Delay 함수 MS 
        /// </SUMMARY>
        /// <PARAM name="MS">(단위 : MS)</PARAM>
        public static DateTime Delay(int MS)
        {
            //DateTime ThisMoment = DateTime.Now;
            //TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            //DateTime AfterWards = ThisMoment.Add(duration);
            //while (AfterWards >= ThisMoment)
            //{
            //    System.Windows.Forms.Application.DoEvents();
            //    ThisMoment = DateTime.Now;
            //}

            Thread.Sleep(MS);

            return DateTime.Now;
        }
    }
}
