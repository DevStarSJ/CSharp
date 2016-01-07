using System;
using System.Collections;

namespace LunaStar.Util
{
    /* Class : FileProperties
     * Content :    File을 읽어서 각 Properties들을 분류하여 Hashtable에 저장한다.
     *              사용자가 Key값을 주면 그 Key값에 해당하는 Property의 Value를 넘겨준다.
     *              
     * Author : 윤석준 (JS-System)
     * Date : 2010.12.13
     */
    public class FileProperties
    {
        // Properties의 (Key,Value)가 저장될 Hashtable
        private Hashtable ht = new Hashtable();

        /// <summary>
        /// 기본 생성자
        /// </summary>
        public FileProperties() { }

        /// <summary>
        /// 생성하면서 property들을 File에서 읽어옵니다.
        /// </summary>
        /// <param name="fileName">property들이 저장된 File 이름</param>
        public FileProperties(string fileName)
        {
            Load(fileName);
        }

        /// <summary>
        ///  File에서 내용을 읽어서 Hashtable에 저장한다.
        /// </summary>
        /// <param name="fileName">property들이 저장된 File 이름</param>
        public void Load(string fileName)
        {
            // File에서 읽어와서
            string readData = FileIO.Instance.ReadFile(fileName).Trim();

            // Line 단위로 끊은뒤
            string[] strLine = readData.Split('\n');

            foreach (string str in strLine)
            {

                // 빈칸이거나 주석이면 Pass
                if (str.Trim().Equals(String.Empty)) { continue; }
                else if (str.Trim()[0] == '#')/*.Substring(0, 1).Equals("#"))*/ { continue; }

                // 내용 있으면 '=' 앞은 key, 뒤는 value
                else
                {
                    string[] token = str.Trim().Split('=');
                    ht.Add(token[0].Trim(), token[1].Trim());
                }
            }
        }

        /// <summary>
        /// Key값에 해당하는 Value를 반환한다.
        /// </summary>
        /// <param name="key">찾아야 할 Key값</param>
        /// <returns></returns>
        public string getProperty(string key)
        {
            return (string)ht[key];
        }
    }
}
