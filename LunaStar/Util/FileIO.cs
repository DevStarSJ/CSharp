using System;
using System.IO;
using System.Text;

namespace LunaStar.Util
{
    /* Class : FileIO
     * Content :    File로 부터 값을 읽어서 String으로 반환한다.
     *
     *              Singleton으로 구현되어서 한 Project에 1개의 개체밖에 생성되지 않는다.
     *
     * Author : 윤석준
     * Date : 2010.12.13
     */
    public class FileIO
    {

        #region Singleton

        private static FileIO _instance = new FileIO();

        private FileIO() { }

        /// <summary>
        /// Singleton 개체를 가져옵니다. 일반 class의 new 랑 같은 기능입니다.
        /// </summary>
        public static FileIO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FileIO();
                }
                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// fileName 에서 내용을 읽어서 String으로 return
        /// </summary>
        /// <param name="fileName">읽어올 File 이름</param>
        /// <returns>File의 내용들</returns>
        public string ReadFile(string fileName, Encoding encode = null)
        {
            if (encode == null)
                encode = Encoding.Default;

            string strRet = null;

            Encoding fileEnc = DetectEncoding(fileName);

            using (Stream S = File.OpenRead(fileName))
            {
                using (StreamReader sr = new StreamReader(S, fileEnc))
                {
                    if (encode == fileEnc)
                        strRet = sr.ReadToEnd();
                    else
                    {
                        string strFile = sr.ReadToEnd();
                        byte[] bytesFile = fileEnc.GetBytes(strFile);
                        byte[] bytesRet = Encoding.Convert(fileEnc, encode, bytesFile);
                        strRet = encode.GetString(bytesRet);
                    }
                }
            }
            return strRet;
        }

        public Encoding DetectEncoding(string filename)
        {
            using (Stream S = File.OpenRead(filename))
            {
                using (StreamReader SR = new StreamReader(S))
                {
                    char[] buffer = new char[10];
                    SR.Read(buffer, 0, 10);

                    if (IsEqualBytes(buffer, new byte[] { 0xFF, 0xFE }, 2))
                    {
                        return Encoding.Unicode; // Little Endian with BOM
                    }
                    else if (IsEqualBytes(buffer, new byte[] { 0xFE, 0xFF }, 2))
                    {
                        return Encoding.BigEndianUnicode; // with BOM
                    }
                    else if (IsEqualBytes(buffer, new byte[] { 0xEF, 0xBB, 0xBF }, 3))
                    {
                        return Encoding.UTF8; // with BOM
                    }
                    else if (IsEqualBytes(buffer, new byte[] { 0x2B, 0x2F, 0x76, 0x38, 0x2D }, 5))
                    {
                        return Encoding.UTF7; // with BOM
                    }
                    else if (IsZeroBytes(buffer, 10, true))
                    {
                        return Encoding.Unicode; // Little Endian without BOM
                    }
                    else if (IsZeroBytes(buffer, 10, false))
                    {
                        return Encoding.BigEndianUnicode; // without BOM
                    }
                }
            }
            return Encoding.Default; // Ansi : MBCS (Multibyte Character Set)
        }

        public bool IsEqualBytes(char[] src, byte[] pattern, int num)
        {
            if (src.Length < num || pattern.Length < num)
                throw new ArgumentOutOfRangeException();

            bool bRet = true;
            for (int i = 0; i < num; i++)
            {
                if (src[i] != pattern[i])
                {
                    bRet = false;
                    break;
                }
            }
            return bRet;
        }

        public bool IsZeroBytes(char[] src, int num, bool checkOdd = true)
        {
            if (src.Length < num)
                throw new ArgumentOutOfRangeException();

            bool bRet = true;
            int rest = checkOdd ? 1 : 0;

            for (int i = 0; i < num; i++)
            {
                if (i % 2 == rest)
                {
                    if (src[i] != 0x00)
                    {
                        bRet = false;
                        break;
                    }
                }
            }
            return bRet;
        }

        /// <summary>
        /// fileName에 data 내용을 저장
        /// </summary>
        /// <param name="fileName">저장할 File 이름</param>
        /// <param name="data">File에 저장될 내용</param>
        public void WriteFile(String fileName, String data)
        {
            using (Stream s = File.OpenWrite(fileName))
            {
                using (StreamWriter sw = new StreamWriter(s, Encoding.Default))
                {
                    sw.Write(data);
                }
            }
        }

        public byte [] GetBytes(string Ps_FileName)
        {
            byte [] LB_FileData = null;

            try
            {
                using (FileStream L_FileStream = new FileStream(Ps_FileName, System.IO.FileMode.Open))
                {
                    using (BinaryReader L_BinaryReader = new BinaryReader(L_FileStream))
                    {
                        int Li_FileLength = Convert.ToInt32(L_FileStream.Length);
                        LB_FileData = L_BinaryReader.ReadBytes(Li_FileLength);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return LB_FileData;
        }

        public bool BytesToFile(string Ps_FileName, byte[] PB_FileData)
        {
            try
            {
                using (BinaryWriter Writer = new BinaryWriter(File.OpenWrite(Ps_FileName)))
                {
                    Writer.Write(PB_FileData);
                    Writer.Flush();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

    }
}
