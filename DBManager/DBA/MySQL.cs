using System;
using System.Data;
using Globals;
using MySql.Data.MySqlClient;


namespace DBA
{
    public class MySQL: IDisposable
    {
        public MySqlConnection Conn;
        public MySqlTransaction Trans;
       
        public MySQL()
        {
            try
            {
                string Ls_ConnStr ="";
               
                //if (Statics.MODE == EM_MODE.TEST)
                //{
                //    Ls_ConnStr = @" Server= 192.100.0.12; "
                //        + @" Database=SL_POP;"
                //        + @" User=root;"
                //        + @" Password=sa:;";
                //}
                //else
                //{
                //    Ls_ConnStr = @" Server= " + Statics.DB_SERVER + "; "
                //        + @" Database=" + Statics.DB_CATALOG + ";"
                //        + @" User=" + Statics.DB_USER + ";"
                //        + @" Password=" + Statics.DB_PASS + ";";
                //}

                Conn = new MySqlConnection(Ls_ConnStr);
                Conn.Open();
            }
            catch (MySqlException)
            {
                throw;
            }
        }


        public bool IsConnect()
        {
            bool Lb_Ret = false;

            if (Conn.State == ConnectionState.Open)
            {
                Lb_Ret = true;
            }
            return Lb_Ret;
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


        public DataSet Fill(string strQuery, string StrAlias, DataSet dsDataSet, MySqlParameter[] paramArray, CommandType cmdType, out MySqlParameterCollection paramCol)
        {
            paramCol = null;

            try
            {
                MySqlDataAdapter DA = new MySqlDataAdapter(strQuery, Conn);
                DA.SelectCommand.CommandType = cmdType;

                if (dsDataSet == null)
                {
                    dsDataSet = new DataSet();
                }

                if (paramArray != null)
                {
                   foreach (MySqlParameter MyParam in paramArray)
                    {
                        DA.SelectCommand.Parameters.Add(MyParam);
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

        public DataSet Fill(string strQuery, string strAlias, DataSet dsDataSet, MySqlParameter[] paramArray, CommandType cmdType)
        {
            MySqlParameterCollection col = null;
            return Fill(strQuery, strAlias, dsDataSet, paramArray, cmdType, out col);
        }


        public DataSet Fill(string strQuery, string strAlias, DataSet dsDataSet, MySqlParameter[] paramArray)
        {
            MySqlParameterCollection col = null;
            return Fill(strQuery, strAlias, dsDataSet, paramArray, CommandType.Text, out col);
        }


        public DataSet Fill(string strQuery, string strAlias, DataSet dsDataSet)
        {
            MySqlParameterCollection col = null;
            return Fill(strQuery, strAlias, dsDataSet, null, CommandType.Text, out col);
        }


        public DataSet Fill(string strQuery, string strAlias)
        {
            MySqlParameterCollection col = null;
            return Fill(strQuery, strAlias, null, null, CommandType.Text, out col);
        }


        public DataSet Fill(string strQuery)
        {
            MySqlParameterCollection col = null;
            return Fill(strQuery, null, null, null, CommandType.Text, out col);
        }

        #endregion



        #region ExecuteNonQuery

        public int ExecuteNonQuery(string strQuery, MySqlParameter paramArray,CommandType cmdType, out MySqlParameterCollection col)
        {
            int Li_RowCnt = 0;
            col = null;

            try
            {
                MySqlCommand cmd = new MySqlCommand(strQuery, Conn);
                cmd.CommandType = cmdType;

                if (paramArray != null)
                {
                    cmd.Parameters.Add(paramArray);
                }

                if (Conn.State == ConnectionState.Closed)
                {
                    Conn.Open();
                }

                Li_RowCnt = cmd.ExecuteNonQuery();
                col = cmd.Parameters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Li_RowCnt;
        }

        public int ExecuteNonQuery(string strQuery, MySqlParameter paramArray, CommandType cmdType)
        {
            MySqlParameterCollection col = null;
            return ExecuteNonQuery(strQuery, paramArray, cmdType, out col);
        }

        public int ExecuteNonQuery(string strQuery, MySqlParameter paramArray)
        {
            MySqlParameterCollection col = null;
            return ExecuteNonQuery(strQuery, paramArray, CommandType.Text, out col);
        }

        public int ExecuteNonQuery(string strQuery)
        {
            MySqlParameterCollection col = null;
            return ExecuteNonQuery(strQuery, null, CommandType.Text, out col);
        }

        #endregion




        #region TransactionQuery

        public int TransactionQuery(string strQuery, MySqlParameter[] paramArray, CommandType cmdType, out MySqlParameterCollection col)
        {
            int Li_RowCnt = 0;
            MySqlCommand cmd = null;
            try
            {
                cmd = new MySqlCommand(strQuery, Conn);
                cmd.CommandType = cmdType;

                if (paramArray != null)
                {
                    foreach (MySqlParameter param in paramArray)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                if (Trans == null)
                {
                    Conn.BeginTransaction();
                }
                cmd.Transaction = Trans;

                Li_RowCnt = cmd.ExecuteNonQuery();
                col = cmd.Parameters;


                //cmd.Transaction.Commit();
            }
            catch (Exception)
            {
                //cmd.Transaction.Rollback();

                Li_RowCnt = -1;
                throw;
            }
            return Li_RowCnt;
        }

        public int TransactionQuery(string strQuery, MySqlParameter[] paramArray, CommandType cmdType)
        {
            MySqlParameterCollection col = null;
            return TransactionQuery(strQuery, paramArray, cmdType, out col);
        }

        public int TransactionQuery(string strQuery, MySqlParameter[] paramArray)
        {
            MySqlParameterCollection col = null;
            return TransactionQuery(strQuery, paramArray, CommandType.Text, out col);
        }

        public int TransactionQuery(string strQuery)
        {
            MySqlParameterCollection col = null;
            return TransactionQuery(strQuery, null, CommandType.Text, out col);
        }

        #endregion


        #region Transaction Fill

        public DataSet TransactionFill(string strQuery, string StrAlias, DataSet dsDataSet, MySqlParameter[] paramArray, CommandType cmdType, out MySqlParameterCollection paramCol)
        {
            paramCol = null;

            try
            {
                MySqlDataAdapter DA = new MySqlDataAdapter(strQuery, Conn);
                DA.SelectCommand.CommandType = cmdType;
                DA.SelectCommand.Transaction = Trans;

                if (dsDataSet == null)
                {
                    dsDataSet = new DataSet();
                }

                if (paramArray != null)
                {
                    foreach (MySqlParameter param in paramArray)
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


        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet, MySqlParameter[] paramArray, CommandType cmdType)
        {
            MySqlParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, paramArray, cmdType, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet, MySqlParameter[] paramArray)
        {
            MySqlParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, paramArray, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet)
        {
            MySqlParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, null, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias)
        {
            MySqlParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, null, null, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery)
        {
            MySqlParameterCollection col = null;
            return TransactionFill(strQuery, null, null, null, CommandType.Text, out col);
        }

        #endregion

        public void EndTrans(bool Pb_Ret)
        {
            try
            {
                if (Trans != null)
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
            }
            catch 
            {
                throw;
            }
        }

        private void CommitTrans()
        {
            Trans.Commit();
        }

        private void RollBackTrans()
        {
            Trans.Rollback();
        }

    }
}
