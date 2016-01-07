using System;
using System.Data;
using Globals;
using System.Text;

namespace LunaStar.Util
{
    public class Ut_DataSet
    {
        public Ut_DataSet()
        {
        }

        public EM_RET CheckDataSet(DataSet Pds_Data)
        {
            EM_RET Le_Ret = EM_RET.EOF;

            try
            {
                if (Pds_Data == null) return Le_Ret;
                if (Pds_Data.Tables == null) return Le_Ret;
                if (Pds_Data.Tables.Count < 1) return Le_Ret;

                Le_Ret = CheckDataTable(Pds_Data.Tables[0]);

                return Le_Ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EM_RET CheckDataTable(DataTable Pdt_Table)
        {
            EM_RET Le_Ret = EM_RET.EOF;

            try
            {
                if (Pdt_Table == null) return Le_Ret;
                if (Pdt_Table.Rows == null) return Le_Ret;
                if (Pdt_Table.Rows.Count < 1) return Le_Ret;

                return EM_RET.OK;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
