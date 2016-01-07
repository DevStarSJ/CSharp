using System;
using System.Data;
using System.Data.SqlClient;
using Globals;

namespace LunaStar.Util
{
    public class Ut_String
    {
        public Ut_String()
        {
        }


        /// <summary>
        /// 프로시져 Sql문 변환
        /// </summary>
        /// <param name="Ps_ProcName"></param>
        /// <param name="Psp_Param"></param>
        /// <returns></returns>
        public string GetProcString(string Ps_ProcName, SqlParameter[] Psp_Param)
        {
            string Ls_Ret = "";

            Ls_Ret += " EXEC " + Ps_ProcName + "\n";
            for (int Li_Cnt = 0; Li_Cnt < Psp_Param.Length; Li_Cnt++)
            {
                Ls_Ret += Psp_Param[Li_Cnt].ParameterName.ToString() + " = ";

                if (Psp_Param[Li_Cnt].SqlDbType == SqlDbType.Int
                    || Psp_Param[Li_Cnt].SqlDbType == SqlDbType.Real
                    || Psp_Param[Li_Cnt].SqlDbType == SqlDbType.Decimal
                    || Psp_Param[Li_Cnt].SqlDbType == SqlDbType.Float
                    || Psp_Param[Li_Cnt].SqlDbType == SqlDbType.SmallInt
                    || Psp_Param[Li_Cnt].SqlDbType == SqlDbType.TinyInt
                    || Psp_Param[Li_Cnt].SqlDbType == SqlDbType.Money
                    || Psp_Param[Li_Cnt].SqlDbType == SqlDbType.SmallMoney)
                {
                    Ls_Ret += Psp_Param[Li_Cnt].Value + ",\n";
                }
                else
                {
                    Ls_Ret += "'" + Psp_Param[Li_Cnt].Value.ToString().Trim() + "',\n";
                }
            }

            if (Ls_Ret.Length > 1)
            {
                Ls_Ret = Ls_Ret.Substring(0, Ls_Ret.Length - 2);
            }
            return Ls_Ret;
        }


        /// <summary>
        /// String Split
        /// </summary>
        /// <param name="Ps_Stirng"></param>
        /// <param name="Pc_Separator"></param>
        /// <returns></returns>
        public string[] Split(string Ps_Stirng, char Pc_Separator)
        {
            string[] Ls_Ret;
            char[] Lc_Separator = { Pc_Separator };
            Ls_Ret = Ps_Stirng.Split(Lc_Separator);
            return Ls_Ret;
        }

        /// <summary>
        /// Is ? Digit
        /// </summary>
        /// <param name="Ps_Stirng">Determine the string</param>
        /// <returns>bool variable</returns>
        public static bool IsDigit(string Ps_Stirng)
        {
            if (string.IsNullOrEmpty(Ps_Stirng)) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(Ps_Stirng, "^\\d+$");
        }

        /// <summary>
        /// Is ? Int
        /// </summary>
        /// <param name="Ps_Stirng">Determine the string</param>
        /// <returns>bool variable</returns>
        public static bool IsInt(string Ps_Stirng)
        {
            if (string.IsNullOrEmpty(Ps_Stirng)) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(Ps_Stirng, @"^[+-]?\d*$");
        }

        /// <summary>
        /// Is ? Double
        /// </summary>
        /// <param name="Ps_Stirng">Determine the string</param>
        /// <returns>bool variable</returns>
        public static bool IsDouble(string Ps_Stirng)
        {
            if (string.IsNullOrEmpty(Ps_Stirng)) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(Ps_Stirng, @"^[+-]?\d*(\.?\d*)$");
        }

        /// <summary>
        /// Is ? Length(8) DateType
        /// </summary>
        /// <param name="Ps_Stirng">Determine the string</param>
        /// <returns>bool variable</returns>
        public static bool IsDate(string Ps_Stirng)
        {
            if (string.IsNullOrEmpty(Ps_Stirng)) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(Ps_Stirng, @"^([\d]{8})$");
        }

        public static bool IsInToDate(string Ps_StartDate, string Ps_EndDate, string Ps_ChkDate)
        {
            if (string.IsNullOrEmpty(Ps_EndDate))
                return false;
            if (string.IsNullOrEmpty(Ps_StartDate))
                return false;
            if (string.IsNullOrEmpty(Ps_ChkDate))
                return false;
            // 2012-02-20 : 20120220 : KGC : ADD201202201213 :: 자동게더링 주기 변경으로 인한 삭제
            //Ps_FromDate = Ps_FromDate.Replace("-", string.Empty);
            //Ps_ToDate = Ps_ToDate.Replace("-", string.Empty);

            // 2012-02-20 : 20120220 : KGC :: 코드 변경
            //if (int.Parse(Ps_ChkDate) >= int.Parse(Ps_FromDate) && int.Parse(Ps_ChkDate) <= int.Parse(Ps_ToDate)) return true;
            //return false;
            return (int.Parse(Ps_ChkDate) >= int.Parse(Ps_StartDate) && int.Parse(Ps_ChkDate) <= int.Parse(Ps_EndDate)) ? true : false;
        }

        public static EM_StringCompare StringCompare(string Ps_Left, string Ps_Right)
        {
            EM_StringCompare Li_Result = EM_StringCompare.EQUAL;
            bool Lb_IsFinish = false;

            int Li_MinLength = Math.Min(Ps_Left.Length, Ps_Right.Length);

            for (int i = 0; i < Li_MinLength; i++)
            {
                if (Ps_Left[i] > Ps_Right[i])
                {
                    Li_Result = EM_StringCompare.BIG_LEFT;
                    Lb_IsFinish = true;
                    break;
                }
                else if (Ps_Left[i] < Ps_Right[i])
                {
                    Li_Result = EM_StringCompare.BIG_RIGHT;
                    Lb_IsFinish = true;
                    break;
                }
            }

            if (!Lb_IsFinish)
            {
                if (Ps_Left.Length > Ps_Right.Length)
                {
                    Li_Result = EM_StringCompare.BIG_LEFT;
                }
                else if (Ps_Left.Length < Ps_Right.Length)
                {
                    Li_Result = EM_StringCompare.BIG_RIGHT;
                }
                else
                {
                    Li_Result = EM_StringCompare.EQUAL;
                }
            }

            return Li_Result;
        }

        public static int GetContainsCount(string Ps_Str, char Pc_Char)
        {
            int Li_RET = 0;

            for (int i = 0; i < Ps_Str.Length; i++)
            {
                if (Ps_Str[i] == Pc_Char) Li_RET++;
            }

            return Li_RET;
        }

        public static EM_StringCompare VersionCompare(string Ps_Left, string Ps_Right)
        {
            EM_StringCompare Li_Result = EM_StringCompare.EQUAL;
            bool Lb_IsFinish = false;

            char Lc_Seperator = '.';
            string[] Ls_Left = Ps_Left.Split(Lc_Seperator);
            string[] Ls_Right = Ps_Right.Split(Lc_Seperator);


            int Li_MinLength = Math.Min(Ls_Left.Length, Ls_Right.Length);

            for (int i = 0; i < Li_MinLength; i++)
            {
                if (IsDigit(Ls_Left[i]) && IsDigit(Ls_Right[i]))
                {
                    int Li_Left = Convert.ToInt32(Ls_Left[i]);
                    int Li_Right = Convert.ToInt32(Ls_Right[i]);
                    if (Li_Left > Li_Right)
                    {
                        Li_Result = EM_StringCompare.BIG_LEFT;
                        Lb_IsFinish = true;
                        break;
                    }
                    else if (Li_Left < Li_Right)
                    {
                        Li_Result = EM_StringCompare.BIG_RIGHT;
                        Lb_IsFinish = true;
                        break;
                    }
                    else continue;
                }
                else
                {
                    Li_Result = EM_StringCompare.ERROR;
                    Lb_IsFinish = true;
                    break;
                }
            }

            if (!Lb_IsFinish)
            {
                if (Ls_Left.Length > Ls_Right.Length)
                {
                    Li_Result = EM_StringCompare.BIG_LEFT;
                }
                else if (Ls_Left.Length < Ls_Right.Length)
                {
                    Li_Result = EM_StringCompare.BIG_RIGHT;
                }
                else
                {
                    Li_Result = EM_StringCompare.EQUAL;
                }
            }

            return Li_Result;
        }
    }
}
