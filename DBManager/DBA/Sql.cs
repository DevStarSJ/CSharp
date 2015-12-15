using System;
using System.Data;
using System.Data.SqlClient;
using WebService;


namespace DBA
{
    /* Class : SQL
     * Content :    Database에 접속하여 sql문을 실행한다.
     *
     *              Singleton으로 구현되어서 한 Project에 1개의 개체밖에 생성되지 않는다.
     *
     * Reference : SQL.cs - 김상수(JS-System)
     *
     * Author : 윤석준 (JS-System)
     * Date        : 2010.12.22
     * Last Update : 2011.11.14
     */
    public class SQL : IDisposable
    {
        private String Ms_ConnString;
        private SqlConnection Msc_Conn;
        private WebServiceInterface MWb_Conn;

        public SQL() { }

        #region Singleton
        private static SQL sql_instance = new SQL();

        /// <summary>
        /// Singleton 개체를 가져옵니다. 일반 class의 new 랑 같은 기능입니다.
        /// </summary>
        public static SQL Instance
        {
            get
            {
                if (sql_instance == null)
                {
                    sql_instance = new SQL();
                }
                return sql_instance;
            }
        }
        #endregion

        #region Transaction Control

        // Transaction 사용을 위한 Connection 및 Transaction
        private SqlTransaction trans;
        private SqlConnection conn;

        /// <summary>
        /// Connection 자원 해제 : Trasnsaction 구현용
        /// </summary>
        //public void Dispose()
        //{
        //    if (conn.State != ConnectionState.Closed)
        //    {
        //        conn.Dispose();
        //        conn = null;
        //    }

        //    GC.SuppressFinalize(this);
        //}

        /// <summary>
        /// Connection이 열려있는지 여부를 판단
        /// </summary>
        /// <returns>열여있다면 True</returns>
        public bool IsOpen()
        {
            bool Lb_Ret = false;
            if (conn != null) { if (conn.State == ConnectionState.Open) { Lb_Ret = true; } }
            return Lb_Ret;
        }


        /// <summary>
        /// Transaction Begin : Connection 얻고 Open 다음에 Transaction 얻기
        /// </summary>
        public void Begin()
        {
            //conn = dbconn.getConnection();
            conn.Open();
            trans = conn.BeginTransaction();
        }

        /// <summary>
        /// 정상적으로 수행되었을 경우 Commit
        /// </summary>
        public void Commit()
        {
            trans.Commit();
            Dispose();
        }

        /// <summary>
        /// 비정상적인 상황 발생시 RollBack
        /// </summary>
        public void RollBack()
        {
            trans.Rollback();
            Dispose();
        }

        /// <summary>
        /// Transaction 종료
        /// </summary>
        /// <param name="Pb_Ret">true면 Commit, false면 Rollback</param>
        public void End(bool Pb_Ret)
        {
            try
            {
                if (Pb_Ret) { Commit(); }
                else { RollBack(); }
            }
            catch (Exception) { throw; }
        }

        #endregion

        #region Transaction() : Transaction Mode에서 사용하는 Excute()

        /// <summary>
        /// Transaction Mode에서 Insert,Delete,Update 의 SQL문을 실행
        /// </summary>
        /// <param name="strQuery">수행될 SQL문</param>
        /// <returns>적용된 Row수</returns>
        public int Transaction(string strQuery)
        {
            SqlParameterCollection col = null;
            return Transaction(strQuery, null, CommandType.Text, out col);
        }

        /// <summary>
        /// Transaction Mode에서 Insert,Delete,Update 의 SQL문을 실행
        /// </summary>
        /// <param name="strQuery">수행될 SQL문</param>
        /// <returns>적용된 Row수</returns>
        public int Transaction(string strQuery, SqlParameter[] paramArray)
        {
            SqlParameterCollection col = null;
            return Transaction(strQuery, paramArray, CommandType.Text, out col);
        }

        /// <summary>
        /// Transaction Mode에서 Insert,Delete,Update 의 SQL문을 실행
        /// </summary>
        /// <param name="strQuery">수행될 SQL문</param>
        /// <returns>적용된 Row수</returns>
        public int Transaction(string strQuery, SqlParameter[] paramArray, CommandType cmdType)
        {
            SqlParameterCollection col = null;
            return Transaction(strQuery, paramArray, cmdType, out col);
        }

        /// <summary>
        /// Transaction Mode에서 Insert,Delete,Update 의 SQL문을 실행
        /// </summary>
        /// <param name="strQuery">수행될 SQL문</param>
        /// <returns>적용된 Row수</returns>
        public int Transaction(string strQuery, SqlParameter[] paramArray,
          CommandType cmdType, out SqlParameterCollection col)
        {
            int Li_RowCnt = 0;

            try
            {
                if (trans == null)
                {
                    Begin();
                }

                using (SqlCommand cmd = new SqlCommand(strQuery, conn))
                {
                    cmd.CommandType = cmdType;

                    if (paramArray != null)
                    {
                        foreach (SqlParameter param in paramArray)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }

                    cmd.Transaction = trans;

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

        #endregion

        #region TransactionQuery() : Transaction Mode에서 사용하는 Query()

        /// <summary>
        /// Transaction Mode에서 Select SQL문을 실행
        /// </summary>
        /// <param name="strQuery">수행될 SQL문</param>
        /// <returns>Query 결과를 저장한 DataSet</returns>
        public DataSet TransactionQuery(string strQuery)
        {
            SqlParameterCollection col = null;
            return TransactionQuery(strQuery, null, null, null, CommandType.Text, out col);
        }

        /// <summary>
        /// Transaction Mode에서 Select SQL문을 실행
        /// </summary>
        /// <param name="strQuery">수행될 SQL문</param>
        /// <returns>Query 결과를 저장한 DataSet</returns>
        public DataSet TransactionQuery(string strQuery, string strAlias)
        {
            SqlParameterCollection col = null;
            return TransactionQuery(strQuery, strAlias, null, null, CommandType.Text, out col);
        }

        /// <summary>
        /// Transaction Mode에서 Select SQL문을 실행
        /// </summary>
        /// <param name="strQuery">수행될 SQL문</param>
        /// <returns>Query 결과를 저장한 DataSet</returns>
        public DataSet TransactionQuery(string strQuery, string strAlias, DataSet dsDataSet)
        {
            SqlParameterCollection col = null;
            return TransactionQuery(strQuery, strAlias, dsDataSet, null, CommandType.Text, out col);
        }

        /// <summary>
        /// Transaction Mode에서 Select SQL문을 실행
        /// </summary>
        /// <param name="strQuery">수행될 SQL문</param>
        /// <returns>Query 결과를 저장한 DataSet</returns>
        public DataSet TransactionQuery(string strQuery, string strAlias, DataSet dsDataSet,
          SqlParameter[] paramArray)
        {
            SqlParameterCollection col = null;
            return TransactionQuery(strQuery, strAlias, dsDataSet, paramArray, CommandType.Text, out col);
        }

        /// <summary>
        /// Transaction Mode에서 Select SQL문을 실행
        /// </summary>
        /// <param name="strQuery">수행될 SQL문</param>
        /// <returns>Query 결과를 저장한 DataSet</returns>
        public DataSet TransactionQuery(string strQuery, string strAlias, DataSet dsDataSet, SqlParameter[] paramArray, CommandType cmdType)
        {
            SqlParameterCollection col = null;
            return TransactionQuery(strQuery, strAlias, dsDataSet, paramArray, cmdType, out col);
        }

        /// <summary>
        /// Transaction Mode에서 Select SQL문을 실행
        /// </summary>
        /// <param name="strQuery">수행될 SQL문</param>
        /// <returns>Query 결과를 저장한 DataSet</returns>
        public DataSet TransactionQuery(string strQuery, string StrAlias, DataSet dsDataSet,
                SqlParameter[] paramArray, CommandType cmdType, out SqlParameterCollection paramCol)
        {
            paramCol = null;

            try
            {
                using (SqlDataAdapter DA = new SqlDataAdapter(strQuery, conn))
                {
                    DA.SelectCommand.CommandType = cmdType;
                    DA.SelectCommand.Transaction = trans;

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

        #endregion

        #region SetSpread() : Spread에 Query 결과 저장

        //public void SetSpread(FarPoint.Win.Spread.FpSpread P_Spd, String Ps_Sql)
        //{
        //    FarPoint.Win.Spread.SheetView L_Sheet = P_Spd.Sheets[0];
        //    DataRowCollection L_drc = Query(Ps_Sql).Tables[0].Rows;

        //    L_Sheet.RowCount = L_drc.Count;
        //    if (L_drc.Count > 0)
        //    {
        //        if (L_drc[0].ItemArray.Length > 0)
        //        {
        //            L_Sheet.ColumnCount = L_drc[0].ItemArray.Length;
        //            for (int i = 0; i < L_Sheet.RowCount; i++)
        //            {
        //                for (int j = 0; j < L_Sheet.ColumnCount; j++)
        //                {
        //                    Object obj = L_drc[i][j];
        //                    L_Sheet.SetText(i, j, GetString(obj));
        //                }
        //            }
        //        }
        //    }
        //}



        public static String GetString(Object obj)
        {
            String text = "";
            if (obj.GetType() == typeof(int))
            {
                text = ((int)obj).ToString();
            }
            if (obj.GetType() == typeof(short))
            {
                text = ((short)obj).ToString();
            }
            if (obj.GetType() == typeof(Byte))
            {
                text = ((Byte)obj).ToString();
            }
            if (obj.GetType() == typeof(Decimal))
            {
                text = ((Decimal)obj).ToString();
            }
            if (obj.GetType() == typeof(int))
            {
                text = ((int)obj).ToString();
            }
            if (obj.GetType() == typeof(DateTime))
            {
                text = ((DateTime)obj).ToString();
            }
            else if (obj.GetType() == typeof(String))
            {
                text = (String)obj;
            }

            return text;
        }

        #endregion

        //#region Connect
        //public EM_RET Connect()
        //{
        //    EM_RET Le_Ret = EM_RET.EOF;

        //    try
        //    {
        //        // ADO CONNECTION
        //        if (Statics.ConnectType == EM_CONNECT_TYPE.ADO)
        //        {
        //            // CONNECTION STRING
        //            if (Statics.MODE == EM_MODE.REAL)
        //            {
        //                Ms_ConnString = "SERVER = " + Statics.DB_SERVER + "; "
        //                    + "database = " + Statics.DB_CATALOG + "; "
        //                    + "uid = " + Statics.DB_USER + "; "
        //                    + "pwd =" + Statics.DB_PASS + ";Connect Timeout=" + Convert.ToString(Statics.DB_ConnectionTimeout);

        //            }
        //            else
        //            {
        //                Ms_ConnString = "SERVER = DEVSERVER; database = MESDB; uid = sa; pwd = sa:";
        //            }

        //            //CONNECTION
        //            if (Msc_Conn != null && Msc_Conn.State != ConnectionState.Closed)
        //            {
        //                Msc_Conn.Close();
        //                Msc_Conn.Dispose();
        //            }
        //            Msc_Conn = new SqlConnection();
        //            Msc_Conn.ConnectionString = Ms_ConnString;
        //            Msc_Conn.Open();

        //            if (Msc_Conn.State == ConnectionState.Open)
        //            {
        //                Le_Ret = EM_RET.OK;
        //            }
        //            else
        //            {
        //                Le_Ret = EM_RET.ERR;
        //            }
        //        }
        //        else
        //        {
        //            //WEB CONNECTION
        //            MWb_Conn = new WebServiceInterface("https://e-partner.co.kr:5051/MES.DMZ.SSL.Common_Service/wsCommon.asmx?wsdl");

        //            if (MWb_Conn != null)
        //            {
        //                Le_Ret = EM_RET.OK;
        //            }
        //            else
        //            {
        //                Le_Ret = EM_RET.ERR;
        //            }
        //        }

        //        return Le_Ret;

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //#endregion

        //#region Fill

        ////************************************************************************************
        //// Query 구문과 함께, 데이터테이블 명과  데이터를 담을 데이터셋을 파라메터로 넘겨,
        //// 데이터셋에 데이터를 채운다
        ////************************************************************************************
        ////[AutoComplete]
        //public DataSet Fill(string strQuery,ref DataSet dsDataSet,
        //  SqlParameter[] paramArray, CommandType cmdType, out SqlParameterCollection paramCol)
        //{
        //    SqlDataAdapter DA = null;
        //    paramCol = null;
        //    string[] Ls_Param = new string[25];
        //    int Li_Cnt = 0;
        //    int Li_Ret = 0;

        //    try
        //    {

        //        Connect();

        //        if (Statics.ConnectType == EM_CONNECT_TYPE.ADO)
        //        {
        //            // ADO
        //            DA = new SqlDataAdapter(strQuery, Msc_Conn);

        //            DA.SelectCommand.CommandType = cmdType;
        //            DA.SelectCommand.CommandTimeout = Statics.DB_CommandTimeout;

        //            if (dsDataSet == null)
        //            {
        //                dsDataSet = new DataSet();
        //            }

        //            if (paramArray != null)
        //            {
        //                foreach (SqlParameter param in paramArray)
        //                {
        //                    DA.SelectCommand.Parameters.Add(param);
        //                }
        //            }

        //            DA.Fill(dsDataSet);
        //            paramCol = DA.SelectCommand.Parameters;
        //        }
        //        else
        //        {
        //            //WebService
        //            if (paramArray != null)
        //            {
        //                foreach (SqlParameter param in paramArray)
        //                {
        //                    Ls_Param[Li_Cnt] = param.Value.ToString();
        //                    Li_Cnt++;
        //                    //Ls_Tmp += param.Value.ToString() + ",";
        //                }
        //                dsDataSet = ((DataSet)MWb_Conn.InvokeMethod("Execution", new object[] { strQuery, Ls_Param, Li_Ret }));
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return dsDataSet;
        //}

        //public DataSet Fill(string strQuery,ref DataSet dsDataSet,
        //  SqlParameter[] paramArray, CommandType cmdType)
        //{
        //    SqlParameterCollection col = null;
        //    return Fill(strQuery,ref dsDataSet, paramArray, cmdType, out col);
        //}

        //public DataSet Fill(string strQuery, ref DataSet dsDataSet,
        //  SqlParameter[] paramArray)
        //{
        //    SqlParameterCollection col = null;
        //    return Fill(strQuery,ref dsDataSet, paramArray, CommandType.Text, out col);
        //}

        //public DataSet Fill(string strQuery,ref DataSet dsDataSet)
        //{
        //    SqlParameterCollection col = null;
        //    return Fill(strQuery,ref dsDataSet, null, CommandType.Text, out col);
        //}

        //public DataSet Fill(string strQuery)
        //{
        //    SqlParameterCollection col = null;
        //    DataSet Lds_Data = new DataSet();

        //    return Fill(strQuery, ref Lds_Data, null, CommandType.Text, out col);
        //}

        //#endregion

        //#region ExcuteNonQuery
        //public int ExecuteNonQuery(string strQuery, SqlParameter[] paramArray,
        //  CommandType cmdType, out SqlParameterCollection col)
        //{
        //    int Li_RowCnt = 0;
        //    col = null;
        //    SqlCommand cmd = null;
        //    string[] Ls_Param = new string[25];
        //    int Li_Cnt = 0;
        //    int Li_Ret = 0;

        //    try
        //    {
        //        Connect();

        //        if (Statics.ConnectType == EM_CONNECT_TYPE.ADO)
        //        {
        //            cmd = new SqlCommand(strQuery, Msc_Conn);

        //            cmd.CommandType = cmdType;

        //            if (paramArray != null)
        //            {
        //                foreach (SqlParameter param in paramArray)
        //                {
        //                    cmd.Parameters.Add(param);
        //                }
        //            }

        //            Li_RowCnt = cmd.ExecuteNonQuery();
        //            col = cmd.Parameters;
        //        }
        //        else
        //        {
        //            //WebService
        //            if (paramArray != null)
        //            {
        //                foreach (SqlParameter param in paramArray)
        //                {
        //                    Ls_Param[Li_Cnt] = param.Value.ToString();
        //                    Li_Cnt++;
        //                    //Ls_Tmp += param.Value.ToString() + ",";
        //                }
        //                //Li_RowCnt = ((int)MWb_Conn.InvokeMethod("Execution", new object[] { strQuery, Ls_Param, Li_Ret }));
        //                MWb_Conn.InvokeMethod("Execution", new object[] { strQuery, Ls_Param, Li_Ret });
        //                Li_RowCnt = Li_Cnt;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //throw new Exception(ex.Message.ToString());
        //        throw;
        //    }
        //    return Li_RowCnt;
        //}



        //public int ExecuteNonQuery(string strQuery, SqlParameter[] paramArray, CommandType cmdType)
        //{
        //    SqlParameterCollection col = null;
        //    return ExecuteNonQuery(strQuery, paramArray, cmdType, out col);
        //}

        //public int ExecuteNonQuery( string strQuery, SqlParameter[] paramArray)
        //{
        //    SqlParameterCollection col = null;
        //    return ExecuteNonQuery(strQuery, paramArray, CommandType.Text, out col);
        //}

        //public int ExecuteNonQuery( string strQuery)
        //{
        //    SqlParameterCollection col = null;
        //    return ExecuteNonQuery(strQuery, null, CommandType.Text, out col);
        //}
        //#endregion

        public void Dispose()
        {
            if (Msc_Conn != null && Msc_Conn.State != ConnectionState.Closed)
            {
                Msc_Conn.Close();
                Msc_Conn.Dispose();
                Msc_Conn = null;
            }

            if (conn != null && conn.State != ConnectionState.Closed)
            {
                conn.Dispose();
                conn = null;
            }

            GC.SuppressFinalize(this);
        }


    }
}