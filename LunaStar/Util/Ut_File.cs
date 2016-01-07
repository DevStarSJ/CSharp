using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace LunaStar.Util
{
    public class Ut_File
    {
        public static void MakeDir(string Ps_Path)
        {
            try
            {
                if (!Directory.Exists(Ps_Path))   // 폴더가 없으면
                {
                    Directory.CreateDirectory(Ps_Path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static string VersionInfo(string Ps_FileName)
        {
            string Ls_Versioninfo = Convert.ToString(FileVersionInfo.GetVersionInfo(Ps_FileName).FileVersion);

            if (!string.IsNullOrEmpty(Ls_Versioninfo))
            {
                Ls_Versioninfo = Ls_Versioninfo.Replace(",", ".").Replace(" ", "");
            }
            else
            {
                Ls_Versioninfo = string.Empty;
            }
            return Ls_Versioninfo;
        }

        /// <summary>
        /// true 면 File 버전이 커서 Upload 해야함. False면 DB 버전이 크거나 같아서 안해도 됨
        /// </summary>
        /// <param name="Ps_DBVersion"></param>
        /// <param name="Ps_FileVersion"></param>
        /// <returns></returns>
        public static bool VersionCompare(string Ps_DBVersion, string Ps_FileVersion)
        {
            string[] Ls_DbV = Ps_DBVersion.Split('.');
            string[] Ls_FileV = Ps_FileVersion.Split('.');

            for (int i = 0; i < Ls_DbV.Length; i++)
            {
                if (Ls_FileV.Length < i) return false;

                long Ld_Db = Convert.ToInt64(Ls_DbV[i]);
                long Ld_File = Convert.ToInt64(Ls_FileV[i]);

                if (Ld_Db > Ld_File) return false;
                else if (Ld_Db < Ld_File) return true;
                else continue;
            }
            return false;
        }

    }
}
