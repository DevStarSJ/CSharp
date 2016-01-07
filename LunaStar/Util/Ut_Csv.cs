using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LunaStar.Util
{
    public class Ut_Csv
    {
        public static DataTable GetDataTable(string Ps_FileName)
        {
            DataTable L_Data = new DataTable();

            string Ls_FileData = FileIO.Instance.ReadFile(Ps_FileName);

            string[] Ls_FileLine = Ls_FileData.Replace("\r", "").Split('\n');

            int Li_LineCnt = Ls_FileLine.Length;

            string[] Ls_Columns = Ls_FileLine[0].Split(',');

            int Li_ColumnCnt = Ls_Columns.Length;

            foreach (string Ls_Column in Ls_Columns)
            {
                DataColumn L_Column = new DataColumn(Ls_Column.ToUpper().Trim());
                L_Data.Columns.Add(L_Column);
            }

            for (int Li_RowIndex = 1; Li_RowIndex < Li_LineCnt; Li_RowIndex++)
            {

                Ls_Columns = Ls_FileLine[Li_RowIndex].Split(',');
                DataRow L_dr = L_Data.NewRow();

                if (IsEmptyLine(Ls_Columns)) continue;

                int Li_CurrentColCnt = (Li_ColumnCnt <= Ls_Columns.Length) ? Li_ColumnCnt : Ls_Columns.Length;
                int Li_ColIndex = 0;
                for (Li_ColIndex = 0; Li_ColIndex < Li_CurrentColCnt; Li_ColIndex++)
                {
                    L_dr[Li_ColIndex] = Ls_Columns[Li_ColIndex].Trim().Replace("'", "");
                }

                for (int Li_ColIndex2 = Li_ColIndex; Li_ColIndex2 < Li_ColumnCnt; Li_ColIndex2++)
                {
                    L_dr[Li_ColIndex2] = "N/A";
                }
                L_Data.Rows.Add(L_dr);
            }

            return L_Data;
        }

        private static bool IsEmptyLine(string[] Ps_Line)
        {
            bool Lb_Result = true;

            for (int i = 0; i < Ps_Line.Length; i++)
            {
                if (Ps_Line[i].Trim().Replace("'", "") != string.Empty)
                {
                    Lb_Result = false;
                    break;
                }
            }
            return Lb_Result;
        }
    }
}
