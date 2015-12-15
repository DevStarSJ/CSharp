using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace DBA
{
    public class FireBird : IDisposable
    {
        private FbConnection Conn;
        private FbTransaction Trans;

        public FireBird(string Ps_Path)
        {
            try
            {
                FbConnectionStringBuilder fb = new FbConnectionStringBuilder();

                //네트워크 접속                
                //fb.ConnectionString = @"Server=192.100.0.4;"
                    //+ @"Database=" + Ps_Path + ";"
                    //+ "User=SYSDBA;Password=masterkey;";

                fb.UserID = "SYSDBA";
                fb.Password = "masterkey";
                fb.Database = Ps_Path;

                fb.ServerType = FbServerType.Default;

                Conn = new FbConnection(fb.ToString());
                Conn.Open();
            }
            catch (FbException)
            {
                throw;
            }
        }


        public void Dispose()
        {
            if (Conn.State != ConnectionState.Closed)
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


        #region Fill


        public DataSet Fill(string strQuery, string StrAlias, DataSet dsDataSet,
          FbParameter[] paramArray, CommandType cmdType, out FbParameterCollection paramCol)
        {
            paramCol = null;

            try
            {
                FbDataAdapter DA = new FbDataAdapter(strQuery, Conn);
                DA.SelectCommand.CommandType = cmdType;

                if (dsDataSet == null)
                {
                    dsDataSet = new DataSet();
                }

                if (paramArray != null)
                {
                    foreach (FbParameter param in paramArray)
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
          FbParameter[] paramArray, CommandType cmdType)
        {
            FbParameterCollection col = null;
            return Fill(strQuery, strAlias, dsDataSet, paramArray, cmdType, out col);
        }


        public DataSet Fill(string strQuery, string strAlias, DataSet dsDataSet,
          FbParameter[] paramArray)
        {
            FbParameterCollection col = null;
            return Fill(strQuery, strAlias, dsDataSet, paramArray, CommandType.Text, out col);
        }


        public DataSet Fill(string strQuery, string strAlias, DataSet dsDataSet)
        {
            FbParameterCollection col = null;
            return Fill(strQuery, strAlias, dsDataSet, null, CommandType.Text, out col);
        }


        public DataSet Fill(string strQuery, string strAlias)
        {
            FbParameterCollection col = null;
            return Fill(strQuery, strAlias, null, null, CommandType.Text, out col);
        }


        public DataSet Fill(string strQuery)
        {
            FbParameterCollection col = null;
            return Fill(strQuery, null, null, null, CommandType.Text, out col);
        }



        #endregion



        #region ExecuteNonQuery

        public int ExecuteNonQuery(string strQuery, FbParameter paramArray,
          CommandType cmdType, out FbParameterCollection col)
        {
            int Li_RowCnt = 0;
            col = null;

            try
            {
                FbCommand cmd = new FbCommand(strQuery, Conn);
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

        public int ExecuteNonQuery(string strQuery, FbParameter paramArray, CommandType cmdType)
        {
            FbParameterCollection col = null;
            return ExecuteNonQuery(strQuery, paramArray, cmdType, out col);
        }

        public int ExecuteNonQuery(string strQuery, FbParameter paramArray)
        {
            FbParameterCollection col = null;
            return ExecuteNonQuery(strQuery, paramArray, CommandType.Text, out col);
        }

        public int ExecuteNonQuery(string strQuery)
        {
            FbParameterCollection col = null;
            return ExecuteNonQuery(strQuery, null, CommandType.Text, out col);
        }

        #endregion




        #region TransactionQuery

        public int TransactionQuery(string strQuery, FbParameter[] paramArray,
          CommandType cmdType, out FbParameterCollection col)
        {
            int Li_RowCnt = 0;
            FbCommand cmd = null;
            try
            {
                cmd = new FbCommand(strQuery, Conn);
                cmd.CommandType = cmdType;

                if (paramArray != null)
                {
                    foreach (FbParameter param in paramArray)
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


                cmd.Transaction.Commit();
            }
            catch (Exception)
            {
                cmd.Transaction.Rollback();

                Li_RowCnt = -1;
                throw;
            }
            return Li_RowCnt;
        }

        public int TransactionQuery(string strQuery, FbParameter[] paramArray, CommandType cmdType)
        {
            FbParameterCollection col = null;
            return TransactionQuery(strQuery, paramArray, cmdType, out col);
        }

        public int TransactionQuery(string strQuery, FbParameter[] paramArray)
        {
            FbParameterCollection col = null;
            return TransactionQuery(strQuery, paramArray, CommandType.Text, out col);
        }

        public int TransactionQuery(string strQuery)
        {
            FbParameterCollection col = null;
            return TransactionQuery(strQuery, null, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery, string StrAlias, DataSet dsDataSet,
          FbParameter[] paramArray, CommandType cmdType, out FbParameterCollection paramCol)
        {
            paramCol = null;

            try
            {
                FbDataAdapter DA = new FbDataAdapter(strQuery, Conn);
                DA.SelectCommand.CommandType = cmdType;
                DA.SelectCommand.Transaction = Trans;

                if (dsDataSet == null)
                {
                    dsDataSet = new DataSet();
                }

                if (paramArray != null)
                {
                    foreach (FbParameter param in paramArray)
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


        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet, FbParameter[] paramArray, CommandType cmdType)
        {
            FbParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, paramArray, cmdType, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet,
          FbParameter[] paramArray)
        {
            FbParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, paramArray, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet)
        {
            FbParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, null, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias)
        {
            FbParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, null, null, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery)
        {
            FbParameterCollection col = null;
            return TransactionFill(strQuery, null, null, null, CommandType.Text, out col);
        }

        #endregion
    }
}