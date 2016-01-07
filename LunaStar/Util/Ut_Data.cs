using System;
using System.Collections.Generic;
using System.Data;

namespace LunaStar.Util
{
    public class Ut_Data
    {
        #region Singleton : Property 활용 없이 Method만 사용할 경우는 Singleton을 이용하여 Static 처럼 활용

        private static Ut_Data _instance = null;

        public static Ut_Data Singleton
        {
            get
            {
                if (_instance == null) _instance = new Ut_Data();
                return _instance;
            }
        }

        #endregion

        public void DateToString(DataTable Pdt_Grid)
        {
            List<string> L_Column = new List<string>();
            for (int i = 0; i < Pdt_Grid.Columns.Count; i++)
            {
                Type L_Type = Pdt_Grid.Columns[i].DataType;
                if (L_Type == typeof(DateTime))
                {
                    string Ls_ColumnName = Pdt_Grid.Columns[i].ColumnName;
                    Pdt_Grid.Columns[i].ColumnName = Ls_ColumnName + "_TEMPDATE";
                    Pdt_Grid.Columns.Add(Ls_ColumnName, typeof(string));
                    L_Column.Add(Ls_ColumnName);
                }
            }

            for (int i = 0; i < Pdt_Grid.Columns.Count; i++)
            {
                if (L_Column.Count < 1) break;
                string Ls_ColumnName = Pdt_Grid.Columns[i].ColumnName;
                for (int j = 0; j < L_Column.Count; j++)
                {
                    string Ls_TempDate = Ls_ColumnName + "_TEMPDATE";
                    if (Ls_ColumnName == L_Column[j])
                    {
                        for (int k = 0; k < Pdt_Grid.Rows.Count; k++)
                        {
                            DateTime L_Date = Convert.ToDateTime(Pdt_Grid.Rows[k][Ls_TempDate]);
                            string Ls_Date = L_Date.ToString("yyyy-MM-dd HH:mm:ss");
                            Pdt_Grid.Rows[k][Ls_ColumnName] = Ls_Date;
                        }

                        L_Column.Remove(Ls_ColumnName);
                        Pdt_Grid.Columns.Remove(Ls_TempDate);
                        break;
                    }
                }
            }
        }

        public void NullToEmpty(DataTable Pdt_Data)
        {
            for (int i = 0; i < Pdt_Data.Rows.Count; i++)
            {
                for (int j = 0; j < Pdt_Data.Columns.Count; j++)
                {
                    object L_Obj = Pdt_Data.Rows[i][j];
                    Type L_Type = L_Obj.GetType();
                    if (L_Type == typeof(DBNull)) Pdt_Data.Rows[i][j] = string.Empty;
                }
            }
        }

        /// <summary>
        /// 주어진 조건에 맞는 결과만을 DataTable로 만듭니다.
        /// </summary>
        /// <param name="Pdt_Data"></param>
        /// <param name="Ps_FilterExpression"></param>
        /// <returns></returns>
        public DataTable Select(DataTable Pdt_Data, string Ps_FilterExpression)
        {
            DataTable Ldt_RET = GetEmptyTable(Pdt_Data);

            DataRow[] Ldr_Result = Pdt_Data.Select(Ps_FilterExpression);

            if (Ldr_Result.Length > 0)
            {
                foreach (DataRow Ldr_Row in Ldr_Result)
                {
                    Ldt_RET.ImportRow(Ldr_Row);
                }
            }
            return Ldt_RET;
        }

        /// <summary>
        /// Column 구조가 똑같은 빈 DataTable을 만듭니다.
        /// </summary>
        /// <param name="Pdt_Data"></param>
        /// <returns></returns>
        public DataTable GetEmptyTable(DataTable Pdt_Data)
        {
            DataTable Ldt_RET = null;

            if (Pdt_Data.Columns.Count > 0)
            {
                Ldt_RET = new DataTable();
                foreach (DataColumn Ldc_Column in Pdt_Data.Columns)
                {
                    Ldt_RET.Columns.Add(Ldc_Column.ColumnName, Ldc_Column.DataType);
                }
            }
            return Ldt_RET;
        }

        /// <summary>
        /// DataTable에서 1개의 Value를 가지고 온다.
        /// </summary>
        /// <param name="Pdt_Data"DataTable></param>
        /// <param name="Key_Column">Key Column Name</param>
        /// <param name="Value_Column">Value Column Name</param>
        /// <param name="Key">Key 값</param>
        /// <returns>Value 값</returns>
        public Object getValue(DataTable Pdt_Data, string Key_Column, string Value_Column, string Key)
        {
            Object Value = null;

            DataRow[] dr = Pdt_Data.Select(String.Format("{0} LIKE '{1}'", Key_Column, Key));
            Value = dr[0][Value_Column];
            
            return Value;
        }

        /// <summary>
        /// DataTable안에 해당 ColumnName 이 있는지 확인한다. - 2012-10-22 - 윤석준
        /// </summary>
        /// <param name="P_Data"></param>
        /// <param name="Ps_ColumnName"></param>
        /// <returns></returns>
        public bool ContainsColumn(DataTable P_Data, string Ps_ColumnName)
        {
            bool IsExist = false;
            foreach (DataColumn L_Col in P_Data.Columns)
            {
                if (L_Col.ColumnName.Trim() == Ps_ColumnName)
                {
                    IsExist = true;
                    break;
                }
            }

            return IsExist;
        }

        /// <summary>
        /// DataTable안에 해당 ColumnName 이 있는지 확인한다. - 2012-10-22 - 윤석준
        /// </summary>
        /// <param name="P_Data"></param>
        /// <param name="Ps_ColumnName"></param>
        /// <returns></returns>
        public bool ContainsColumn(DataTable P_Data, string[] Ps_ColumnNames)
        {
            bool IsExist = true;
            foreach (string L_Name in Ps_ColumnNames)
            {
                if (!ContainsColumn(P_Data,L_Name))
                {
                    IsExist = false;
                    break;
                }
            }

            return IsExist;
        }


    }
}
