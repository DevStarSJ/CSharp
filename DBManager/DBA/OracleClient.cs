using System;
using System.Data;
using Oracle.DataAccess.Client;

namespace DBA
{
    public class OraClient : IDisposable
    {
        public string DBName = string.Empty;
        private int CommandTimeout = 180;

        private OracleConnection Conn;
        private OracleTransaction Trans;

        #region 생성자

        public OraClient()
        {
            ConnectionElement L_ConnEle = ConnectionManager.MainConnection;
            Conn = L_ConnEle.GetOracleConnection();
            CommandTimeout = L_ConnEle.CommandTimeout;

            Open();
        }

        public OraClient(string Ps_Name)
        {
            DBName = Ps_Name;
            ConnectionElement L_ConnEle = (DBName == string.Empty) ? ConnectionManager.MainConnection :
                                                                     ConnectionManager.ConnectionList[DBName];
            Conn = L_ConnEle.GetOracleConnection();
            CommandTimeout = L_ConnEle.CommandTimeout;

            Open();
        }

        #endregion

        #region Open, Dispose, Transaction, LoginTest, ConnTest

        public void Open()
        {
            try
            {
                if (Conn != null)
                {
                    if (Conn.State != ConnectionState.Open)
                    {
                        Conn.Open();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Boolean Ws_LoginTest(string Ps_Server, string Ps_Catalog, string Ps_Pass, string Ps_User, string Ps_Port, string Ps_Query)
        {
            OracleConnection Loc_Conn = null;
            string Ls_ConnStr;
            string Ls_DataSource;
            DataSet Lds_Data = null;

            Ls_DataSource = @""
                            + "(DESCRIPTION ="
                            + "   (ADDRESS_LIST ="
                            + "     (ADDRESS = (PROTOCOL = TCP)(HOST = " + Ps_Server + ")(PORT = 1521))"
                            + "   )"
                            + "  (CONNECT_DATA ="
                            + "     (SERVICE_NAME = " + Ps_Catalog + ")"
                            + "   )"
                            + " )";

            Ls_ConnStr = "Password=" + Ps_Pass + ";User=" + Ps_User + ";Data Source=" + Ls_DataSource + "";

            try
            {
                Loc_Conn = new OracleConnection(Ls_ConnStr);
                Loc_Conn.Open();

                OracleDataAdapter DA = new OracleDataAdapter(Ps_Query, Loc_Conn);
                DA.SelectCommand.CommandType = CommandType.Text;

                if (Lds_Data == null)
                {
                    Lds_Data = new DataSet();
                }

                DA.Fill(Lds_Data);

                if (Lds_Data.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            catch (OracleException)
            {                
                throw;
            }
            finally
            {
                if(Loc_Conn != null)
                    Loc_Conn.Close();
            }

            return false;
        }

        public static Boolean Ws_ConnTest(string Ps_Server, string Ps_Catalog, string Ps_Pass, string Ps_User, string Ps_Port)
        {
            OracleConnection Loc_Conn = null;
            string Ls_ConnStr;
            string Ls_DataSource;
            Boolean Lb_Result = false;

            Ls_DataSource = @""
                            + "(DESCRIPTION ="
                            + "   (ADDRESS_LIST ="
                            + "     (ADDRESS = (PROTOCOL = TCP)(HOST = " + Ps_Server + ")(PORT = 1521))"
                            + "   )"
                            + "  (CONNECT_DATA ="
                            + "     (SERVICE_NAME = " + Ps_Catalog + ")"
                            + "   )"
                            + " )";

            Ls_ConnStr = "Password=" + Ps_Pass + ";User=" + Ps_User + ";Data Source=" + Ls_DataSource + "";

            try
            {
                Loc_Conn = new OracleConnection(Ls_ConnStr);
                Loc_Conn.Open();

                Lb_Result = true;
                
            }
            catch (OracleException)
            {
                throw;                
            }
            finally
            {
                if (Loc_Conn != null)
                    Loc_Conn.Close();                
            }

            return Lb_Result;
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

        private void CommitTrans()
        {
            Trans.Commit();
        }

        private void RollBackTrans()
        {
            Trans.Rollback();
        }

        public void EndTrans(bool Pb_Ret)
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

        #endregion

        #region Instant Method - Connection 생성 -> 명령 수행 -> Connection Dispose - 2012-10-15 - 윤석준

        public static string InstantConnectionName = "";

        /// <summary>
        ///  Instact Fill
        /// </summary>
        /// <param name="Ps_SQL"></param>
        /// <returns></returns>
        public static DataSet Query(string Ps_SQL, OracleParameter[] P_Params = null, CommandType P_CmdType = CommandType.Text)
        {
            DataSet L_DataSet = null;

            try
            {
                using (OraClient ORA = new OraClient(InstantConnectionName))
                {
                    using (OracleDataAdapter DA = new OracleDataAdapter(Ps_SQL, ORA.Conn))
                    {
                        DA.SelectCommand.CommandType = P_CmdType;
                        DA.SelectCommand.CommandTimeout = ORA.CommandTimeout;

                        L_DataSet = new DataSet();

                        if (P_Params != null)
                        {
                            foreach (OracleParameter param in P_Params)
                            {
                                DA.SelectCommand.Parameters.Add(param);
                            }
                        }

                        DA.Fill(L_DataSet);
                    }
                }
            }
            catch
            {
                throw;
            }
            return L_DataSet;
        }

        /// <summary>
        /// Instant ExcuteNonQuery
        /// </summary>
        /// <param name="Ps_SQL"></param>
        /// <returns></returns>
        public static int NonQuery(string Ps_SQL, OracleParameter[] P_Params = null, CommandType P_CmdType = CommandType.Text)
        {
            int Li_RowCnt = 0;

            try
            {
                using (OraClient ORA = new OraClient(InstantConnectionName))
                {
                    using (OracleCommand cmd = new OracleCommand(Ps_SQL, ORA.Conn))
                    {
                        cmd.CommandType = P_CmdType;
                        cmd.CommandTimeout = ORA.CommandTimeout;

                        if (P_Params != null)
                        {
                            foreach (OracleParameter param in P_Params)
                            {
                                cmd.Parameters.Add(param);
                            }
                        }

                        Li_RowCnt = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                throw;
            }
            return Li_RowCnt;
        }

        /// <summary>
        /// Instanct FillSchema
        /// </summary>
        /// <param name="Ps_SQL"></param>
        /// <param name="Ps_ConnectionName"></param>
        /// <returns></returns>
        public static DataSet Schema(string Ps_SQL, CommandType P_CmdType = CommandType.Text)
        {
            DataSet L_DataSet = null;

            try
            {
                using (OraClient ORA = new OraClient(InstantConnectionName))
                {
                    using (OracleDataAdapter DA = new OracleDataAdapter(Ps_SQL, ORA.Conn))
                    {
                        DA.SelectCommand.CommandType = P_CmdType;
                        DA.SelectCommand.CommandTimeout = ORA.CommandTimeout;

                        L_DataSet = new DataSet();

                        DA.FillSchema(L_DataSet, SchemaType.Source);
                    }
                }
            }
            catch
            {
                throw;
            }
            return L_DataSet;
        }

        #endregion

        #region Fill

        public DataSet Fill(string Ps_Query,
                            OracleParameter [] P_Params = null,
                            CommandType cmdType = CommandType.Text)
        {
            DataSet DS = new DataSet();
            try
            {
                using (OracleDataAdapter DA = new OracleDataAdapter(Ps_Query, Conn))
                {
                    DA.SelectCommand.CommandType = cmdType;

                    if (P_Params != null)
                    {
                        foreach (OracleParameter param in P_Params)
                        {
                            DA.SelectCommand.Parameters.Add(param);
                        }
                    }

                    DA.Fill(DS);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DS;
        }

        #endregion

        #region ExecuteNonQuery

        public int ExecuteNonQuery(string Ps_Query,
                                   OracleParameter [] P_Params = null,
                                   CommandType cmdType = CommandType.Text)
        {
            int Li_RowCnt = 0;

            try
            {
                using (OracleCommand cmd = new OracleCommand(Ps_Query, Conn))
                {
                    cmd.CommandType = cmdType;

                    if (P_Params != null)
                    {
                        foreach (OracleParameter param in P_Params)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }

                    Li_RowCnt = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Li_RowCnt;
        }

        #endregion

        #region BulkInsert - 2012-10-06 - 윤석준

        /// <summary>
        /// Bulk Insert
        /// </summary>
        /// <param name="Ps_TableName">Destination Target Table</param>
        /// <param name="P_Data">Data Table</param>
        public void BulkInsert(string Ps_TableName, DataTable P_Data)
        {
            using (OracleBulkCopy BulkCopy = new OracleBulkCopy(Conn))
            {
                BulkCopy.DestinationTableName = Ps_TableName;
                try
                {
                    BulkCopy.WriteToServer(P_Data);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region TransactionQuery

        public int TransactionQuery(string strQuery, OracleParameter[] paramArray,
          CommandType cmdType, out OracleParameterCollection col)
        {
            int Li_RowCnt = 0;

            try
            {
                using (OracleCommand cmd = new OracleCommand(strQuery, Conn))
                {
                    cmd.CommandType = cmdType;

                    if (paramArray != null)
                    {
                        foreach (OracleParameter param in paramArray)
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
            }
            catch (Exception)
            {
                Li_RowCnt = -1;
                throw;
            }
            return Li_RowCnt;
        }

        public int TransactionQuery(string strQuery, OracleParameter[] paramArray, CommandType cmdType)
        {
            OracleParameterCollection col = null;
            return TransactionQuery(strQuery, paramArray, cmdType, out col);
        }

        public int TransactionQuery(string strQuery, OracleParameter[] paramArray)
        {
            OracleParameterCollection col = null;
            return TransactionQuery(strQuery, paramArray, CommandType.Text, out col);
        }

        public int TransactionQuery(string strQuery)
        {
            OracleParameterCollection col = null;
            return TransactionQuery(strQuery, null, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery, string StrAlias, DataSet dsDataSet,
          OracleParameter[] paramArray, CommandType cmdType, out OracleParameterCollection paramCol)
        {
            paramCol = null;

            try
            {
                using (OracleDataAdapter DA = new OracleDataAdapter(strQuery, Conn))
                {
                    DA.SelectCommand.CommandType = cmdType;
                    DA.SelectCommand.Transaction = Trans;

                    if (dsDataSet == null)
                    {
                        dsDataSet = new DataSet();
                    }

                    if (paramArray != null)
                    {
                        foreach (OracleParameter param in paramArray)
                        {
                            DA.SelectCommand.Parameters.Add(param);
                        }
                    }

                    DA.Fill(dsDataSet);
                    paramCol = DA.SelectCommand.Parameters;
                }
            }
            catch
            {
                throw;
            }
            return dsDataSet;
        }

        #endregion

        #region Transaction Fill


        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet, OracleParameter[] paramArray, CommandType cmdType)
        {
            OracleParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, paramArray, cmdType, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet,
          OracleParameter[] paramArray)
        {
            OracleParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, paramArray, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet)
        {
            OracleParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, null, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias)
        {
            OracleParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, null, null, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery)
        {
            OracleParameterCollection col = null;
            return TransactionFill(strQuery, null, null, null, CommandType.Text, out col);
        }

        #endregion

        #region SetTable 관련 함수 : ChkPrimaryKey, Insert, Update, SetTable, AddArray

        /// <summary>
        /// 조건에 해당하는 Instance가 몇개 있는지 알려줌
        /// </summary>
        /// <param name="Ps_Table">테이블 이름</param>
        /// <param name="Ps_Field">조건 비교할 Column들의 배열</param>
        /// <param name="Ps_Code">조건 비교할 값들의 배열</param>
        /// <returns>0이상 해당 Count, -1 : Field랑 Code수가 다름, -2 : Field의 수가 0, -3 : Code의 수가 0</returns>
        public int ChkPrimaryKey(String Ps_Table, String[] Ps_Field, String[] Ps_Code)
        {
            int Li_Result = 0;
            try
            {
                if (Ps_Field.Length != Ps_Code.Length)
                {
                    Li_Result = -1;
                }
                else if (Ps_Field.Length < 1)
                {
                    Li_Result = -2;
                }
                else if (Ps_Code.Length < 1)
                {
                    Li_Result = -3;
                }
                else
                {
                    String Ls_Sql = "SELECT COUNT(*) AS CNT FROM " + Ps_Table + " WHERE " + Ps_Field[0] + " ='" + Ps_Code[0] + "' ";
                    if (Ps_Field.Length > 1)
                    {
                        for (int Li_Cnt = 1; Li_Cnt < Ps_Field.Length; Li_Cnt++)
                        {
                            Ls_Sql = Ls_Sql + " AND " + Ps_Field[Li_Cnt] + " ='" + Ps_Code[Li_Cnt] + "' ";
                        }
                    }

                    DataRowCollection drc = Fill(Ls_Sql).Tables[0].Rows;
                    if (drc.Count > 0)
                    {
                        Li_Result = Convert.ToInt32(drc[0][0]);
                    }
                }
            }
            catch (Exception) { throw; }

            return Li_Result;
        }

        /// <summary>
        /// Table에 값을 Insert
        /// </summary>
        /// <param name="Ps_Table">테이블 이름</param>
        /// <param name="Ps_Field">저장할 Column들의 배열</param>
        /// <param name="Ps_Code">저장할 값 배열</param>
        /// <returns>적용된 Column의 수</returns>
        public int Insert(String Ps_Table, String[] Ps_Field, String[] Ps_Code)
        {
            int Li_Result = 0;
            try
            {
                if (Ps_Field.Length != Ps_Code.Length)
                {
                    Li_Result = -1;
                }
                else if (Ps_Field.Length < 1)
                {
                    Li_Result = -2;
                }
                else if (Ps_Code.Length < 1)
                {
                    Li_Result = -3;
                }
                else
                {
                    String Ls_Columns = "";
                    String Ls_Values = "";

                    for (int Li_Cnt = 0; Li_Cnt < Ps_Field.Length; Li_Cnt++)
                    {
                        Ls_Columns = Ls_Columns + Ps_Field[Li_Cnt] + ",";
                        Ls_Values = Ls_Values + "'" + Ps_Code[Li_Cnt] + "',";
                    }
                    Ls_Columns = Ls_Columns.Substring(0, Ls_Columns.Length - 1);
                    Ls_Values = Ls_Values.Substring(0, Ls_Values.Length - 1);


                    String Ls_Sql = "INSERT INTO " + Ps_Table + " (" + Ls_Columns + ") VALUES (" + Ls_Values + ")";

                    Li_Result = ExecuteNonQuery(Ls_Sql);
                }
            }
            catch (Exception) { throw; }
            return Li_Result;
        }

        /// <summary>
        /// Table에 값을 Update
        /// </summary>
        /// <param name="Ps_Table">테이블 이름</param>
        /// <param name="Ps_Field">Where 절의 Column들의 배열</param>
        /// <param name="Ps_Code">Where 절의 값 배열</param>
        /// <param name="Ps_Field2">저장할 Column들의 배열</param>
        /// <param name="Ps_Code2">저장할 값 배열</param>
        /// <returns>적용된 Column의 수</returns>
        public int Update(String Ps_Table, String[] Ps_Field1, String[] Ps_Code1, String[] Ps_Field2, String[] Ps_Code2)
        {
            int Li_Result = 0;
            try
            {
                if ((Ps_Field1.Length != Ps_Code1.Length) || (Ps_Field2.Length != Ps_Code2.Length))
                {
                    Li_Result = -1;
                }
                else if ((Ps_Field1.Length < 1) || (Ps_Field2.Length < 1))
                {
                    Li_Result = -2;
                }
                else if ((Ps_Code1.Length < 1) || (Ps_Code2.Length < 1))
                {
                    Li_Result = -3;
                }
                else
                {
                    String Ls_Sql = "UPDATE " + Ps_Table + " SET ";

                    for (int Li_Cnt = 0; Li_Cnt < Ps_Field2.Length; Li_Cnt++)
                    {
                        Ls_Sql = Ls_Sql + Ps_Field2[Li_Cnt] + " = '" + Ps_Code2[Li_Cnt] + "',";
                    }
                    Ls_Sql = Ls_Sql.Substring(0, Ls_Sql.Length - 1);
                    Ls_Sql = Ls_Sql + " WHERE ";
                    for (int Li_Cnt = 0; Li_Cnt < Ps_Field1.Length; Li_Cnt++)
                    {
                        Ls_Sql = Ls_Sql + Ps_Field1[Li_Cnt] + " = '" + Ps_Code1[Li_Cnt] + "' AND ";
                    }
                    Ls_Sql = Ls_Sql.Substring(0, Ls_Sql.Length - 5);

                    Li_Result = ExecuteNonQuery(Ls_Sql);
                }
            }
            catch (Exception) { throw; }
            return Li_Result;
        }

        /// <summary>
        /// Table에 값을 저장. 이미 존재하면 Update, 새로운 값이면 Insert
        /// </summary>
        /// <param name="Ps_Table">테이블 이름</param>
        /// <param name="Ps_Field">Where 절의 Column들의 배열</param>
        /// <param name="Ps_Code">Where 절의 값 배열</param>
        /// <param name="Ps_Field2">저장할 Column들의 배열</param>
        /// <param name="Ps_Code2">저장할 값 배열</param>
        /// <returns>적용된 Column의 수</returns>
        public int SetTable(String Ps_Table, String[] Ps_Field1, String[] Ps_Code1, String[] Ps_Field2, String[] Ps_Code2)
        {
            int Li_Result = 0;
            if (ChkPrimaryKey(Ps_Table, Ps_Field1, Ps_Code1) > 0)
            {
                Li_Result = Update(Ps_Table, Ps_Field1, Ps_Code1, Ps_Field2, Ps_Code2);
            }
            else
            {
                Li_Result = Insert(Ps_Table, ArrayAdd(Ps_Field1, Ps_Field2), ArrayAdd(Ps_Code1, Ps_Code2));
            }
            return Li_Result;
        }

        /// <summary>
        /// String 배열 2개를 합침
        /// </summary>
        /// <param name="Ps_Left">합칠 Array</param>
        /// <param name="Ps_Right">합칠 Array</param>
        /// <returns>합쳐진 Array</returns>
        public String[] ArrayAdd(String[] Ps_Left, String[] Ps_Right)
        {
            String[] Ls_Result = new String[Ps_Left.Length + Ps_Right.Length];
            Array.Copy(Ps_Left, Ls_Result, Ps_Left.Length);
            Array.Copy(Ps_Right, 0, Ls_Result, Ps_Left.Length, Ps_Right.Length);
            return Ls_Result;
        }

        #endregion

    }
}
