using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LunaStar.Util
{
    public class MakeDT
    {
        #region Singleton
        private static MakeDT _instance = null;

        public static MakeDT Get
        {
            get {
                if (_instance == null)
                    _instance = new MakeDT();
                return _instance;
            }
        }

        private MakeDT() { }
        #endregion

        public DataTable Worst5(DataTable Pdt_Data, String Ps_Code, String Ps_Name, String Ps_Qty, String Ps_Percent, ref Int32 Pi_Sum)
        {
            DataTable Ldt_Worst = new DataTable();

            Ldt_Worst = new DataTable();
            Ldt_Worst.Columns.Add(Ps_Code);
            Ldt_Worst.Columns.Add(Ps_Name);
            Ldt_Worst.Columns.Add(Ps_Qty, typeof(Int32));
            Ldt_Worst.Columns.Add(Ps_Percent, typeof(Double));

            //grid_List.DataSource = Ldt_Worst;

            Pi_Sum = 0;
            if (Pdt_Data.Rows.Count > 0)
            {
                //gridBand_Sum.Visible = true;
                //gridBand_Sum1.Visible = true;
                //gridBand_Sum2.Visible = true;

                for (int i = 0; i < Pdt_Data.Rows.Count; i++)
                {
                    Pi_Sum = Pi_Sum + Convert.ToInt32(Pdt_Data.Rows[i][2]);
                }

                if (Pdt_Data.Rows.Count <= 5)
                {
                    for (int i = 0; i < Pdt_Data.Rows.Count; i++)
                    {
                        Ldt_Worst.Rows.Add(Pdt_Data.Rows[i][0], Pdt_Data.Rows[i][1], Pdt_Data.Rows[i][2], Math.Round(Convert.ToDouble(Pdt_Data.Rows[i][2]) / Pi_Sum * 100, 2));
                    }
                }
                else
                {
                    int L_Sum = 0;

                    for (int i = 0; i < 5; i++)
                    {
                        Ldt_Worst.Rows.Add(Pdt_Data.Rows[i][0], Pdt_Data.Rows[i][1], Pdt_Data.Rows[i][2], Math.Round(Convert.ToDouble(Pdt_Data.Rows[i][2]) / Pi_Sum * 100, 2));
                        L_Sum = L_Sum + Convert.ToInt32(Pdt_Data.Rows[i][2]);

                    }
                    Ldt_Worst.Rows.Add(" ", "기타", Pi_Sum - L_Sum, Math.Round(Convert.ToDouble(Pi_Sum - L_Sum) / Pi_Sum * 100, 2));
                }
            }

            return Ldt_Worst;
        }

        public void AppendDataTable(DataTable Pdt_Target, DataTable Pdt_Source)
        {
            for (int i = 0; i < Pdt_Source.Rows.Count; i++)
            {
                Pdt_Target.Rows.Add(Pdt_Source.Rows[i]["Code"], Pdt_Source.Rows[i]["Name"]);
            }
        }

    }
}
