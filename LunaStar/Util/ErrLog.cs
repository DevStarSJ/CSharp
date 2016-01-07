using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LunaStar.Util
{
    public class ErrLog
    {
        private static byte[] WriteStringBytes(string _string, FileStream _fs)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(_string);
            _fs.Write(info, 0, info.Length);

            return info;
        }

        public static void SetLog(string _Point, string _ErrMsg)
        {
            SaveLog(_Point, _ErrMsg);
        }

        public static void SetLog(string _Point)
        {
            SaveLog(_Point, String.Empty);
        }


        private static void SaveLog(string _Point, string _Errmsg)
        {
            string dir = Application.StartupPath;
            string filename = string.Format(@"\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");

            CheckDirectory(dir);
            InsertLog(dir + filename, _Point, _Errmsg);
        }

        private static void CheckDirectory(string _dir)
        {
            if(!Directory.Exists(_dir))
            {
                Directory.CreateDirectory(_dir);
            }
        }

        private static void InsertLog(string _filepath, string _Point, string _Errmsg)
        {
            if (File.Exists(_filepath))
            {
                FileStream fs = new FileStream(_filepath, FileMode.Append);

                fs.Seek(0, SeekOrigin.End);
                ErrLog.WriteStringBytes("======================================================================================================================\r\n", fs);
                ErrLog.WriteStringBytes("> Time    : " + DateTime.Now.ToString("yyyy-MM-dd tt HH:mm:ss.ffff") + "\r\n", fs);
                ErrLog.WriteStringBytes("> Point   : " + _Point                                          + "\r\n", fs);
                ErrLog.WriteStringBytes("> Message : " + _Errmsg                                         + "\r\n", fs);
                //ErrLog.WriteStringBytes("======================================================================================================================\r\n", fs);

                fs.Flush();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(_filepath, FileMode.Create);

                fs.Seek(0, SeekOrigin.End);
                ErrLog.WriteStringBytes("======================================================================================================================\r\n", fs);
                ErrLog.WriteStringBytes("> Time    : " + DateTime.Now.ToString("yyyy-MM-dd tt HH:mm:ss.ffff") + "\r\n", fs);
                ErrLog.WriteStringBytes("> Point   : " + _Point                                          + "\r\n", fs);
                ErrLog.WriteStringBytes("> Message : " + _Errmsg                                         + "\r\n", fs);
                //ErrLog.WriteStringBytes("======================================================================================================================\r\n", fs);

                fs.Flush();
                fs.Close();
            }
        }
    }
}
