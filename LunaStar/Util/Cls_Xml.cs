using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Globals;

namespace LunaStar.Util
{
    public class cls_Xml : IDisposable
    {
        private string Ls_FilePath = null;
        private XmlDocument xml = null;

        public void Dispose()
        {
            if (xml != null)
            {
                xml = null;
            }

            GC.SuppressFinalize(this);
        }

        #region Constructor

        //public cls_Xml(string Ps_File)
        //{
        //    try
        //    {
        //        Ls_FilePath = Statics.APP_PATH + Ps_File + ".xml";
        //        Ls_FilePath = Ls_FilePath.Replace("\\\\", "\\");
        //        if (File.Exists(Ls_FilePath) == true)
        //        {
        //            xml = new XmlDocument();
        //            xml.Load(Ls_FilePath);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public cls_Xml(string Ps_File, string Ps_Ext)
        //{
        //    try
        //    {
        //        Ls_FilePath = Statics.APP_PATH + Ps_File + "." + Ps_Ext;
        //        Ls_FilePath = Ls_FilePath.Replace("\\\\", "\\");
        //        if (File.Exists(Ls_FilePath) == true)
        //        {
        //            xml = new XmlDocument();
        //            xml.Load(Ls_FilePath);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public cls_Xml(string Ps_File, string Ps_IniPath, string Ps_Ext)
        //{
        //    try
        //    {
        //        Ls_FilePath = Statics.APP_PATH + "\\" + Ps_IniPath + Ps_File + "." + Ps_Ext;
        //        Ls_FilePath = Ls_FilePath.Replace("\\\\", "\\");
        //        if (File.Exists(Ls_FilePath) == true)
        //        {
        //            xml = new XmlDocument();
        //            xml.Load(Ls_FilePath);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        #endregion

        #region Xml Data 조회
        public string GetXmlValue(string Ps_Section, string Ps_Key)
        {
            string Ls_Key;

            try
            {
                Ls_Key = Ps_Section + "/" + Ps_Key;

                return GetXmlValue(Ls_Key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetXmlValue(string Ps_Head, string Ps_Section, string Ps_Key)
        {
            string Ls_Key;

            try
            {
                Ls_Key = Ps_Head + "/" + Ps_Section + "/" + Ps_Key;

                return GetXmlValue(Ls_Key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetXmlValue(string Ps_Key)
        {
            try
            {
                XPathNavigator navi = xml.CreateNavigator().SelectSingleNode(Ps_Key);

                if (navi != null)
                {
                    return navi.Value;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion

        public Boolean SetXmlValue(string Ps_Section, string Ps_Key, string Ps_Value)
        {
            Boolean Lb_Ret = false;
            string Ls_Key;

            try
            {
                Ls_Key = Ps_Section + "/" + Ps_Key;

                Lb_Ret = SetXmlValue(Ls_Key, Ps_Value);

            }
            catch (Exception)
            {
                throw;
            }

            return Lb_Ret;
        }

        public Boolean SetXmlValue(string Ps_Head, string Ps_Section, string Ps_Key, string Ps_Value)
        {
            Boolean Lb_Ret = false;
            string Ls_Key;

            try
            {
                Ls_Key = Ps_Head + "/" + Ps_Section + "/" + Ps_Key;

                Lb_Ret = SetXmlValue(Ls_Key, Ps_Value);
            }
            catch (Exception)
            {
                throw;
            }

            return Lb_Ret;
        }
        /// <summary>
        /// 해당 xml에 데이터를 입력한다.
        /// </summary>
        /// <param name="Ps_Key"> xml 구조 형식(대소문자 구분함) ex : root/login/gs_empid </param>
        /// <param name="Ps_Value">xml에 입력할 값</param>
        /// <returns></returns>
        public Boolean SetXmlValue(string Ps_Key, string Ps_Value)
        {
            Boolean Lb_Ret = false;
            XPathNavigator navi;

            try
            {
                if (Ps_Key.Substring(Ps_Key.Length - 1, 1) == "/")
                {
                    Ps_Key = Ps_Key.Substring(0, Ps_Key.Length - 1);
                }

                navi = xml.CreateNavigator().SelectSingleNode(Ps_Key);

                if (navi != null)
                {
                    //키가 존재할경우 데이터 입력
                    navi.SetValue(Ps_Value);
                    xml.Save(Ls_FilePath);
                    Lb_Ret = true;
                }
                else
                {
                    //노드가 존재하지 않을 경우 노드생성 재귀호출 후 데이터 입력
                    Wf_CreateNode(Ps_Key, 0);

                    navi = xml.CreateNavigator().SelectSingleNode(Ps_Key);

                    if (navi != null)
                    {
                        navi.SetValue(Ps_Value);
                        xml.Save(Ls_FilePath);
                        Lb_Ret = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Lb_Ret;
        }


        private void Wf_CreateNode(string Ps_Node, int Pi_Idx)
        {
            string[] Ls_Keys = Ps_Node.Split('/');
            string Ls_Key;
            XmlNode Parent;
            XmlNode elem;
            //Boolean Lb_Find = false;

            //키변환
            Ls_Key = "";
            try
            {

                for (int Li_Cnt = 0; Li_Cnt <= Pi_Idx; Li_Cnt++)
                {
                    Ls_Key += Ls_Keys[Li_Cnt] + "/";
                }
                Ls_Key = Ls_Key.Replace("//", "/");
                Ls_Key = Ls_Key.Substring(0, Ls_Key.Length - 1);

                //노드 선택
                XPathNavigator nav = ((IXPathNavigable) xml).CreateNavigator();
                XPathNodeIterator iter = nav.Select(Ls_Key);

                if (iter.Count > 0)
                {
                    if (Pi_Idx + 1 <= Ls_Keys.Length)
                    {
                        Wf_CreateNode(Ps_Node, Pi_Idx + 1);
                    }
                }
                else
                {
                    //키변환
                    Ls_Key = "";
                    for (int Li_Cnt = 0; Li_Cnt <= Pi_Idx - 1; Li_Cnt++)
                    {
                        Ls_Key += Ls_Keys[Li_Cnt] + "/";
                    }
                    Ls_Key = Ls_Key.Replace("//", "/");
                    ;
                    Ls_Key = Ls_Key.Substring(0, Ls_Key.Length - 1);

                    //노드 생성
                    Parent = xml.SelectSingleNode(Ls_Key);
                    elem = xml.CreateElement(Ls_Keys[Pi_Idx]);
                    Parent.AppendChild(elem);
                    xml.Save(Ls_FilePath);

                    if (Ls_Keys.Length > Pi_Idx + 1)
                    {
                        Wf_CreateNode(Ps_Node, Pi_Idx + 1);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
