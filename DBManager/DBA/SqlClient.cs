using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DBA
{
    public class SQLClient : IDisposable
    {
        public string DBName = string.Empty;
        private int CommandTimeout = 180;

        private SqlConnection Conn;
        private SqlTransaction Trans;

        #region 생성자

        public SQLClient()
        {
            ConnectionElement L_ConnEle = ConnectionManager.MainConnection;
            Conn = L_ConnEle.GetSqlConnection();
            CommandTimeout = L_ConnEle.CommandTimeout;

            Open();
        }

        public SQLClient(string Ps_Name)
        {
            DBName = Ps_Name;
            ConnectionElement L_ConnEle = (DBName == string.Empty) ? ConnectionManager.MainConnection :
                                                                     ConnectionManager.ConnectionList[DBName];
            Conn = L_ConnEle.GetSqlConnection();
            CommandTimeout = L_ConnEle.CommandTimeout;

            Open();
        }

        #endregion

        #region Instant Method - Connection 생성 -> 명령 수행 -> Connection Dispose - 2012-10-15 - 윤석준

        /// <summary>
        ///  Instact Fill
        /// </summary>
        /// <param name="Ps_SQL"></param>
        /// <returns></returns>
        public static DataSet Query(string Ps_SQL)
        {
            return Query(Ps_SQL, string.Empty);
        }

        /// <summary>
        ///  Instact Fill
        /// </summary>
        /// <param name="Ps_SQL"></param>
        /// <returns></returns>
        public static DataSet Query(string Ps_SQL, string Ps_ConnectionName)
        {
            DataSet L_DataSet = null;

            try
            {
                using (SQLClient SQL = new SQLClient(Ps_ConnectionName))
                {
                    using (SqlDataAdapter DA = new SqlDataAdapter(Ps_SQL, SQL.Conn))
                    {
                        DA.SelectCommand.CommandType = CommandType.Text;
                        DA.SelectCommand.CommandTimeout = SQL.CommandTimeout;

                        L_DataSet = new DataSet();

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


        public static string GetValueFromTable(string Ps_Sql, string Ps_ConnectionName)
        {
            string Ls_Ret = "";

            DataTable Ldt_Table = Query(Ps_Sql, Ps_ConnectionName).Tables[0];
            if (Ldt_Table.Rows.Count > 0)
            {
                Ls_Ret = Convert.ToString(Ldt_Table.Rows[0][0]).Trim();
            }

            return Ls_Ret;
        }

        public static string GetValueFromTable(string Ps_Table, string Ps_GetField, string[] Ps_FieldName, string[] Ps_FieldCode, string Ps_ConnectionName)
        {
            string Ls_Ret = string.Empty;

            if (Ps_FieldName.Length != Ps_FieldCode.Length) return null;

            StringBuilder SB = new StringBuilder();
            SB.AppendLine("SELECT {1} AS VALUE FROM {0}");
            SB.AppendLine(" WHERE 1 = 1");

            for (int i = 0; i < Ps_FieldName.Length; i++)
            {
                SB.AppendFormat("   AND {0} = '{1}'{2}", Ps_FieldName[i], Ps_FieldCode[i], Environment.NewLine);
            }

            string Ls_SQL = string.Format(SB.ToString(), Ps_Table, Ps_GetField);

            using (DataTable Ldt_Data = Query(Ls_SQL, Ps_ConnectionName).Tables[0])
            {
                if (Ldt_Data.Rows.Count > 0)
                {
                    Ls_Ret = Convert.ToString(Ldt_Data.Rows[0]["VALUE"]).Trim();
                }
            }
            return Ls_Ret;
        }


        /// <summary>
        /// Instant ExcuteNonQuery
        /// </summary>
        /// <param name="Ps_SQL"></param>
        /// <returns></returns>
        public static int NonQuery(string Ps_SQL)
        {
            return NonQuery(Ps_SQL, string.Empty);
        }

        /// <summary>
        /// Instant ExcuteNonQuery
        /// </summary>
        /// <param name="Ps_SQL"></param>
        /// <returns></returns>
        public static int NonQuery(string Ps_SQL, string Ps_ConnectionName)
        {
            int Li_RowCnt = 0;

            try
            {
                using (SQLClient SQL = new SQLClient(Ps_ConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand(Ps_SQL, SQL.Conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = SQL.CommandTimeout;

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
        /// Instact FillSchema
        /// </summary>
        /// <param name="Ps_SQL"></param>
        /// <returns></returns>
        public static DataSet Schema(string Ps_SQL)
        {
            return Schema(Ps_SQL, string.Empty);
        }

        /// <summary>
        /// Instanct FillSchema
        /// </summary>
        /// <param name="Ps_SQL"></param>
        /// <param name="Ps_ConnectionName"></param>
        /// <returns></returns>
        public static DataSet Schema(string Ps_SQL, string Ps_ConnectionName)
        {
            DataSet L_DataSet = null;

            try
            {
                using (SQLClient SQL = new SQLClient(Ps_ConnectionName))
                {
                    using (SqlDataAdapter DA = new SqlDataAdapter(Ps_SQL, SQL.Conn))
                    {
                        DA.SelectCommand.CommandType = CommandType.Text;
                        DA.SelectCommand.CommandTimeout = SQL.CommandTimeout;

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

        #region Open, Dispose, Transaction 관련

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
            catch (SqlException)
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

        public bool IsOpen()
        {
            bool Lb_Ret = false;

            if (Conn != null)
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Lb_Ret = true;
                }
            }
            return Lb_Ret;
        }

        public void BeginTrans()
        {
            //Conn.Open();
            Trans = Conn.BeginTransaction();
        }

        public void CommitTrans()
        {
            Trans.Commit();
        }

        public void RollBackTrans()
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

        #region Fill - SELECT

        public DataSet Fill(string strQuery, DataSet dsDataSet,
          SqlParameter[] paramArray, CommandType cmdType, out SqlParameterCollection paramCol)
        {
            paramCol = null;

            try
            {
                using (SqlDataAdapter DA = new SqlDataAdapter(strQuery, Conn))
                {
                    DA.SelectCommand.CommandType = cmdType;

                    if (dsDataSet == null)
                    {
                        dsDataSet = new DataSet();
                    }

                    if (paramArray != null)
                    {
                        foreach (SqlParameter param in paramArray)
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

        public DataSet Fill(string strQuery, SqlParameter[] paramArray, CommandType cmdType)
        {
            SqlParameterCollection col = null;
            return Fill(strQuery, null, paramArray, cmdType, out col);
        }

        public DataSet Fill(string strQuery, DataSet dsDataSet,
          SqlParameter[] paramArray, CommandType cmdType)
        {
            SqlParameterCollection col = null;
            return Fill(strQuery, dsDataSet, paramArray, cmdType, out col);
        }


        public DataSet Fill(string strQuery, DataSet dsDataSet,
          SqlParameter[] paramArray)
        {
            SqlParameterCollection col = null;
            return Fill(strQuery, dsDataSet, paramArray, CommandType.Text, out col);
        }


        public DataSet Fill(string strQuery, DataSet dsDataSet)
        {
            SqlParameterCollection col = null;
            return Fill(strQuery, dsDataSet, null, CommandType.Text, out col);
        }


        public DataSet Fill(string strQuery)
        {
            SqlParameterCollection col = null;
            return Fill(strQuery, null, null, CommandType.Text, out col);
        }

        #endregion

        #region ExecuteNonQuery - INSERT, DELETE, UPDATE

        public int ExecuteNonQuery(string strQuery, SqlParameter[] paramArray, CommandType cmdType, out SqlParameterCollection col)
        {
            int Li_RowCnt = 0;
            col = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand(strQuery, Conn))
                {
                    cmd.CommandType = cmdType;

                    if (paramArray != null)
                    {
                        foreach (SqlParameter param in paramArray)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }

                    //Conn.Open();
                    Li_RowCnt = cmd.ExecuteNonQuery();
                    col = cmd.Parameters;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Li_RowCnt;
        }

        public int ExecuteNonQuery(string strQuery, SqlParameter[] paramArray, CommandType cmdType)
        {
            SqlParameterCollection col = null;
            return ExecuteNonQuery(strQuery, paramArray, cmdType, out col);
        }

        public int ExecuteNonQuery(string strQuery, SqlParameter[] paramArray)
        {
            SqlParameterCollection col = null;
            return ExecuteNonQuery(strQuery, paramArray, CommandType.Text, out col);
        }

        public int ExecuteNonQuery(string strQuery)
        {
            SqlParameterCollection col = null;
            return ExecuteNonQuery(strQuery, null, CommandType.Text, out col);
        }

        #endregion

        #region TransactionQuery

        public int TransactionQuery(string strQuery, SqlParameter[] paramArray,
          CommandType cmdType, out SqlParameterCollection col)
        {
            int Li_RowCnt = 0;

            try
            {
                SqlCommand cmd = new SqlCommand(strQuery, Conn);
                cmd.CommandType = cmdType;

                if (paramArray != null)
                {
                    foreach (SqlParameter param in paramArray)
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

        public int TransactionQuery(string strQuery, SqlParameter[] paramArray, CommandType cmdType)
        {
            SqlParameterCollection col = null;
            return TransactionQuery(strQuery, paramArray, cmdType, out col);
        }

        public int TransactionQuery(string strQuery, SqlParameter[] paramArray)
        {
            SqlParameterCollection col = null;
            return TransactionQuery(strQuery, paramArray, CommandType.Text, out col);
        }

        public int TransactionQuery(string strQuery)
        {
            SqlParameterCollection col = null;
            return TransactionQuery(strQuery, null, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery, string StrAlias, DataSet dsDataSet,
          SqlParameter[] paramArray, CommandType cmdType, out SqlParameterCollection paramCol)
        {
            paramCol = null;

            try
            {
                SqlDataAdapter DA = new SqlDataAdapter(strQuery, Conn);
                DA.SelectCommand.CommandType = cmdType;
                DA.SelectCommand.Transaction = Trans;

                if (dsDataSet == null)
                {
                    dsDataSet = new DataSet();
                }

                if (paramArray != null)
                {
                    foreach (SqlParameter param in paramArray)
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


        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet, SqlParameter[] paramArray, CommandType cmdType)
        {
            SqlParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, paramArray, cmdType, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet,
          SqlParameter[] paramArray)
        {
            SqlParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, paramArray, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias, DataSet dsDataSet)
        {
            SqlParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, dsDataSet, null, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery, string strAlias)
        {
            SqlParameterCollection col = null;
            return TransactionFill(strQuery, strAlias, null, null, CommandType.Text, out col);
        }

        public DataSet TransactionFill(string strQuery)
        {
            SqlParameterCollection col = null;
            return TransactionFill(strQuery, null, null, null, CommandType.Text, out col);
        }

        #endregion

        #region SQL문 활용 함수들 - SetTable 관련 함수 : ChkPrimaryKey, Insert, Update, SetTable, AddArray

        /// <summary>
        /// Table에서 1개의 값을 찾는다.
        /// </summary>
        /// <param name="Ps_Table">대상 Table</param>
        /// <param name="Ps_GetField">찾아야할 값의 Field 혹은 관련 식</param>
        /// <param name="Ps_FieldName">조건절의 FieldName</param>
        /// <param name="Ps_FieldCode">조건절의 값</param>
        /// <returns></returns>
        public string GetValueFromTable(string Ps_Table, string Ps_GetField, string[] Ps_FieldName, string[] Ps_FieldCode)
        {
            string Ls_RET = string.Empty;

            if (Ps_FieldName.Length != Ps_FieldCode.Length) return null;

            StringBuilder SB = new StringBuilder();
            SB.AppendLine("SELECT {1} AS VALUE FROM {0}");
            SB.AppendLine(" WHERE 1 = 1");

            for (int i = 0; i < Ps_FieldName.Length; i++)
            {
                SB.AppendFormat("   AND {0} = '{1}'{2}", Ps_FieldName[i], Ps_FieldCode[i], Environment.NewLine);
            }

            string Ls_SQL = string.Format(SB.ToString(), Ps_Table, Ps_GetField);

            using (DataTable Ldt_Data = Fill(Ls_SQL).Tables[0])
            {
                if (Ldt_Data.Rows.Count > 0)
                {
                    Ls_RET = Convert.ToString(Ldt_Data.Rows[0]["VALUE"]).Trim();
                }
            }

            return Ls_RET;
        }

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
        /// <param name="Ps_Field1">Where 절의 Column들의 배열</param>
        /// <param name="Ps_Code1">Where 절의 값 배열</param>
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

        #region Bulk Insert - 2012-10-05 - 윤석준

        /// <summary>
        /// Bulk Insert
        /// </summary>
        /// <param name="Ps_TableName">Destination Target Table</param>
        /// <param name="P_Data">Data Table</param>
        public void BulkInsert(string Ps_TableName, DataTable P_Data)
        {
            using (SqlBulkCopy BulkCopy = new SqlBulkCopy(Conn))
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

        #region FillSchema - Table 정보 가져오기 - 2012-10-12 - 윤석준

        public DataSet FillSchema(string Ps_SQL)
        {
            return FillSchema(Ps_SQL, SchemaType.Source);
        }

        public DataSet FillSchema(string Ps_SQL, SchemaType P_SchemaType)
        {
            //SqlParameterCollection paramCol = null;
            DataSet L_DataSet = null;
            SqlParameter[] L_ParamArray = null;
            try
            {
                using (SqlDataAdapter DA = new SqlDataAdapter(Ps_SQL, Conn))
                {
                    DA.SelectCommand.CommandType = CommandType.Text;
                    DA.SelectCommand.CommandTimeout = CommandTimeout;

                    if (L_DataSet == null)
                    {
                        L_DataSet = new DataSet();
                    }

                    if (L_ParamArray != null)
                    {
                        foreach (SqlParameter param in L_ParamArray)
                        {
                            DA.SelectCommand.Parameters.Add(param);
                        }
                    }
                    DA.FillSchema(L_DataSet, P_SchemaType);
                    //paramCol = DA.SelectCommand.Parameters;
                }
            }
            catch
            {
                throw;
            }

            return L_DataSet;
        }

        #endregion

    }

}
