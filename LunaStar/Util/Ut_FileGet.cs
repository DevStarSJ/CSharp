using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utils
{
    public class Ut_FileGet : IDisposable
    {
        private string[] Ls_FileList;
        private const string NET_PATH = @"\\ZERO\temp\";
        //private Thread th = null;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region File의 내용을 조회하여 반환함
        public string Gf_ReadFile(string Ps_Path, string Ps_Delimeter, int Pi_Length)
        {
            return Gf_ReadFile(Ps_Path, new string[] { Ps_Delimeter }, Pi_Length);
        }

        public string Gf_ReadFile(string Ps_Path, string[] Ps_Delimeter, int Pi_Length)
        {
            string[] Ls_Data;

            Ls_Data = Gf_ReadFile(Ps_Path, Ps_Delimeter);

            return Ls_Data[Pi_Length];
        }

        public string[] Gf_ReadFile(string Ps_Path, string Ps_Delimeter)
        {
            return Gf_ReadFile(Ps_Path, new string[] { Ps_Delimeter });
        }

        public string[] Gf_ReadFile(string Ps_Path, string[] Ps_Delimeter)
        {
            FileStream Lobj_Fs = null;
            StreamReader Lobj_Sr = null;

            try
            {
                string[] Ls_RetData;
                string Ls_Data;

                Lobj_Fs = new FileStream(Ps_Path, FileMode.Open);
                Lobj_Sr = new StreamReader(Lobj_Fs);

                Lobj_Sr.BaseStream.Seek(0, SeekOrigin.Begin);
                Ls_Data = Lobj_Sr.ReadToEnd();
                Ls_RetData = Ls_Data.Split(Ps_Delimeter, StringSplitOptions.RemoveEmptyEntries);

                return Ls_RetData;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (Lobj_Sr != null)
                {
                    Lobj_Sr.Close();
                    Lobj_Sr.Dispose();
                    Lobj_Sr = null;
                }

                if (Lobj_Fs != null)
                {
                    Lobj_Fs.Close();
                    Lobj_Fs.Dispose();
                    Lobj_Fs = null;
                }
            }
        }
        #endregion

        #region 폴더 유무 검사
        public Boolean Gf_ExistFolder(string Ps_Path)
        {
            return Directory.Exists(Ps_Path);
        }
        #endregion

        #region 폴더 생성
        public void Gf_CreateFolder(string Ps_Path)
        {
            Gf_CreateFolder(Ps_Path, 0);
        }

        private void Gf_CreateFolder(string Ps_Path, int Pi_idx)
        {
            try
            {
                string[] Ls_Keys = Ps_Path.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);

                string Ls_Key = "";

                for (int Li_Cnt = 0; Li_Cnt <= Pi_idx; Li_Cnt++)
                {
                    Ls_Key += Ls_Keys[Li_Cnt] + '\\';
                }

                Ls_Key = Ls_Key.Substring(0, Ls_Key.Length - 1);

                if (Gf_ExistFolder(Ls_Key) == true)
                {
                    Gf_CreateFolder(Ps_Path, Pi_idx + 1);
                }
                else
                {
                    Directory.CreateDirectory(Ls_Key);

                    if (Ls_Keys.Length > Pi_idx + 1)
                    {
                        Gf_CreateFolder(Ps_Path, Pi_idx + 1);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 파일 생성
        public void Gs_CreateFile(string Ps_File)
        {
            FileStream Lobj_Fs = null;

            try
            {
                Lobj_Fs = new FileStream(Ps_File, FileMode.Create, FileAccess.Write);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (Lobj_Fs != null)
                {
                    Lobj_Fs.Close();
                    Lobj_Fs.Dispose();
                    Lobj_Fs = null;
                }
            }
        }
        #endregion

        #region 파일 Write
        public void Gs_WriteFile(string Ps_Path, string Ps_Value)
        {
            FileStream Lobj_Fs = null;
            StreamWriter Lobj_Sw = null;

            try
            {
                Lobj_Fs = new FileStream(Ps_Path, FileMode.Append, FileAccess.Write);

                Lobj_Sw = new StreamWriter(Lobj_Fs);

                Lobj_Sw.Write(Ps_Value);


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (Lobj_Sw != null)
                {
                    Lobj_Sw.Close();
                    Lobj_Sw.Dispose();
                    Lobj_Sw = null;
                }

                if (Lobj_Fs != null)
                {
                    Lobj_Fs.Close();
                    Lobj_Fs.Dispose();
                    Lobj_Fs = null;
                }
            }
        }
        #endregion

        #region 파일 존재 여부 검사
        public Boolean Gf_ExistFile(string Ps_File)
        {
            return File.Exists(Ps_File);
        }
        #endregion

        #region 파일 삭제
        public void Gs_DeleteFile(string Ps_File)
        {
            if (Gf_ExistFile(Ps_File) == true)
            {
                File.Delete(Ps_File);
            }
        }
        #endregion

        private void Work()
        {
            FileStream fs;
            Byte[] by;
            //Oracle Lda_Oracle= null;
            string[] Ls_Result;
            string[] Ls_Result2;
            string Ls_Temp;

            Ls_FileList = new string[11];

            Ls_FileList[0] = "eq1.csv";
            Ls_FileList[1] = "eq2.csv";
            Ls_FileList[2] = "eq3.csv";
            Ls_FileList[3] = "eq4.csv";
            Ls_FileList[4] = "eq5.csv";
            Ls_FileList[5] = "eq6.csv";
            Ls_FileList[6] = "eq7.csv";
            Ls_FileList[7] = "eq8.csv";
            Ls_FileList[8] = "eq9.csv";
            Ls_FileList[9] = "eq10.csv";
            Ls_FileList[10] = "eq11.csv";

            for (int Li_Cnt = 0; Ls_FileList.Length > Li_Cnt; Li_Cnt++)
            {
                if (File.Exists(NET_PATH + Ls_FileList[Li_Cnt]))
                {
                    fs = new FileStream(NET_PATH + Ls_FileList[Li_Cnt], FileMode.Open, FileAccess.Read);
                    by = new Byte[fs.Length];
                    fs.Read(by, 0, by.Length);
                    Ls_Temp = Encoding.ASCII.GetString(by);
                    Ls_Result = Ls_Temp.Split(new Char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int Li_ResultCnt = 0; Ls_Result.Length > Li_ResultCnt; Li_ResultCnt++)
                    {
                        Ls_Result2 = Ls_Result[Li_ResultCnt].Split(',');

                        //Lda_Oracle= new Oracle();
                        //Lda_Oracle.ExecuteNonQuery("INSERT INTO TB_TEST VALUES (" +
                        //    "'" + Ls_Result2[0] + "'," +
                        //    "'" + Ls_Result2[1] + "'," +
                        //    "'" + Ls_Result2[2] + "'," +
                        //    "'" + Ls_Result2[3] + "'," +
                        //    "'" + Ls_Result2[4] + "'," +
                        //    "'" + Ls_Result2[5] + "'," +
                        //    "'" + Ls_Result2[6] + "'," +
                        //    "'" + Ls_Result2[7] + "'," +
                        //    "'" + Ls_Result2[8] + "'," +
                        //    "'" + Ls_Result2[9] + "'," +
                        //    "'" + Ls_Result2[10] + "'," +
                        //    "'" + Ls_Result2[11] + "'," +
                        //    "'" + Ls_Result2[12] + "'," +
                        //    "'" + Ls_Result2[13] + "'," +
                        //    "'" + Ls_Result2[14] + "'," +
                        //    "'" + Ls_Result2[15] + "'," +
                        //    "'" + Ls_Result2[16] + "'," +
                        //    "'" + Ls_Result2[17] + "'," +
                        //    "'" + Ls_Result2[18] + "'," +
                        //    "'" + Ls_Result2[19] + "'," +
                        //    "'" + Ls_Result2[20] + "'," +
                        //    "SYSDATE," +
                        //    "'" + Ls_Result2[22] + "')"
                        //    );
                    }
                }
            }

            //MessageBox.Show("완료");
        }

    }
}