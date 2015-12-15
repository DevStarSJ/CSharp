using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;


namespace Globals.DB.Connection
{
    public class Ole :IDisposable
    {
        private string ConnStr;

        private OleDbConnection  Conn;
        private OleDbTransaction Trans;


        public Ole()
        {
            ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @"MDB\SPC_DB.mdb";

            try
            {
                if (ConnStr != null)
                {
                    Conn = new OleDbConnection(ConnStr);
                    Conn.Open(); 
                }
            }
            catch (OleDbException)
            {
                throw;
            }
        }


        public void Dispose()
        {
            if (ConnStr != null && Conn.State != ConnectionState.Closed)
            {
                Conn.Close();
                Conn.Dispose();
                Conn = null;
            }

            GC.SuppressFinalize(this);
        }
        
        public void BeginTrans()
        {
            //Conn.Open();
            Trans = Conn.BeginTransaction();
        }

        private void CommitTrans()
        {
            Trans.Commit();
        }

        private void RollBackTrans()
        {
            Trans.Rollback();
        }

        private void EndTrans(bool Pb_Ret)
        {
            try
            {
                if (Pb_Ret)
                {
                    CommitTrans();
                }
                else
                {
                    RollBackTrans();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Fill

        public DataSet Fill(string strQuery, string StrAlias, DataSet dsDataSet,
          OleDbParameter[] paramArray, CommandType cmdType, out OleDbParameterCollection paramCol)
        {
            paramCol = null;

            try
            {
                OleDbDataAdapter DA = new OleDbDataAdapter(strQuery, Conn);
                DA.SelectCommand.CommandType = cmdType;

                if (dsDataSet == null)
                {
                    dsDataSet = new DataSet();
                }

                if (paramArray != null)
                {
                    foreach (OleDbParameter param in paramArray)
                    {
                        DA.SelectCommand.Parameters.Add(param);
                    }
                }

                DA.Fill(dsDataSet);
                paramCol = DA.SelectCommand.Parameters;
            }
            catch
            {
                throw;
            }
            return dsDataSet;
        }

        public DataSet Fill(string strQuery, string strAlias, DataSet dsDataSet,
          OleDbParameter[] paramArray, CommandType cmdType)
        {
            OleDbParameterCollection col = null;
            return Fill(strQuery, strAlias, dsDataSet, paramArray, cmdType, out col);
        }

        
        public DataSet Fill(string strQuery, string strAlias, DataSet dsDataSet,
          OleDbParameter[] paramArray)
        {
            OleDbParameterCollection col = null;
            return Fill(strQuery, strAlias, dsDataSet, paramArray, CommandType.Text, out col);
        }

        
        public DataSet Fill(string strQuery, string strAlias, DataSet dsDataSet)
        {
            OleDbParameterCollection col = null;
            return Fill(strQuery, strAlias, dsDataSet, null, CommandType.Text, out col);
        }

        
        public DataSet Fill(string strQuery, string strAlias)
        {
            OleDbParameterCollection col = null;
            return Fill(strQuery, strAlias, null, null, CommandType.Text, out col);
        }

        
        public DataSet Fill(string strQuery)
        {
            OleDbParameterCollection col = null;
            return Fill(strQuery, null, null, null, CommandType.Text, out col);
        }

        #endregion



        #region ExecuteNonQuery

        public int ExecuteNonQuery(string strQuery, OleDbParameter paramArray,
          CommandType cmdType, out OleDbParameterCollection col)
        {
            int Li_RowCnt = 0;
            col = null;

            try
            {
                OleDbCommand cmd = new OleDbCommand(strQuery, Conn);
                cmd.CommandType = cmdType;

                if (paramArray != null)
                {
                    cmd.Parameters.Add(paramArray);
                }

                //if (Conn.State == ConnectionState.Closed)
                //{
                //    Conn.Open();
                //}

                Li_RowCnt = cmd.ExecuteNonQuery();
                col = cmd.Parameters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Li_RowCnt;
        }

        public int ExecuteNonQuery(string strQuery, OleDbParameter paramArray, CommandType cmdType)
        {
            OleDbParameterCollection col = null;
            return ExecuteNonQuery(strQuery, paramArray, cmdType, out col);
        }

        public int ExecuteNonQuery(string strQuery, OleDbParameter paramArray)
        {
            OleDbParameterCollection col = null;
            return ExecuteNonQuery(strQuery, paramArray, CommandType.Text, out col);
        }

        public int ExecuteNonQuery(string strQuery)
        {
            OleDbParameterCollection col = null;
            return ExecuteNonQuery(strQuery, null, CommandType.Text, out col);
        }
        
        #endregion




        #region TransactionQuery

        public int TransactionQuery(string strQuery, OleDbParameter[] paramArray,
          CommandType cmdType, out OleDbParameterCollection col)
        {
            int Li_RowCnt = 0;

            try
            {
                OleDbCommand cmd = new OleDbCommand(strQuery, Conn);
                cmd.CommandType = cmdType;

                if (paramArray != null)
                {
                    foreach (OleDbParameter param in paramArray)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                if (Trans == null)
                {
                    BeginTrans();
                }
                cmd.Transaction = Trans;

                Li_RowCnt = cmd.ExecuteNonQuery();
                col = cmd.Parameters;
            }
            catch (Exception)
            {
                Li_RowCnt = -1;
                throw;
            }
            return Li_RowCnt;
        }

        public int TransactionQuery(string strQuery, OleDbParameter[] paramArray, CommandType cmdType)
        {
            OleDbParameterCollection col = null;
            return TransactionQuery(strQuery, paramArray, cmdType, out col);
        }

        public int TransactionQuery(string strQuery, OleDbParameter[] paramArray)
        {
            OleDbParameterCollection col = null;
            return TransactionQuery(strQuery, paramArray, CommandType.Text, out col);
        }

        public int TransactionQuery(string strQuery)
        {
            OleDbParameterCollection col = null;
            return TransactionQuery(strQuery, null, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery, string StrAlias, DataSet dsDataSet,
          OleDbParameter[] paramArray, CommandType cmdType, out OleDbParameterCollection paramCol)
        {
            paramCol = null;

            try
            {
                OleDbDataAdapter DA = new OleDbDataAdapter(strQuery, Conn);
                DA.SelectCommand.CommandType = cmdType;
                DA.SelectCommand.Transaction = Trans;

                if (dsDataSet == null)
                {
                    dsDataSet = new DataSet();
                }

                if (paramArray != null)
                {
                    foreach (OleDbParameter param in paramArray)
                    {
                        DA.SelectCommand.Parameters.Add(param);
                    }
                }

                DA.Fill(dsDataSet);
                paramCol = DA.SelectCommand.Parameters;
            }
            catch
            {
                throw;
            }
            return dsDataSet;
        }

        #endregion



        #region Transaction Fill


        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet, OleDbParameter[] paramArray, CommandType cmdType)
        {
            OleDbParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, paramArray, cmdType, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet,
          OleDbParameter[] paramArray)
        {
            OleDbParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, paramArray, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet)
        {
            OleDbParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, null, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias)
        {
            OleDbParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, null, null, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery)
        {
            OleDbParameterCollection col = null;
            return TransactionFill(strQuery, null, null, null, CommandType.Text, out col);
        }

        #endregion

        public static DataTable Ws_getDataFromXLS(string Ps_FilePath)
        {
            try
            {
                string strConnectionString = "";
                strConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                                                 "Data Source=" + Ps_FilePath + "; Jet OLEDB:Engine Type=5;" +
                                                 "Extended Properties=Excel 8.0;";
                OleDbConnection cnCSV = new OleDbConnection(strConnectionString);
                cnCSV.Open();
                OleDbCommand cmdSelect = new OleDbCommand(@"SELECT * FROM [Sheet1$]", cnCSV);
                OleDbDataAdapter daCSV = new OleDbDataAdapter(); 
                daCSV.SelectCommand = cmdSelect;
                DataTable dtCSV = new DataTable();
                daCSV.Fill(dtCSV);
                cnCSV.Close();
                daCSV = null;
                return dtCSV;
            }
            catch (Exception)
            {
                throw;
            }            
        }
    }
}
