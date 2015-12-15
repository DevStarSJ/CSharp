using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Oracle.DataAccess.Client;

//
// D/B 설정을 저장
// Connection String 및 Connection 을 반환
//
// - 2012-10-08 윤석준
//
namespace DBA
{
    public enum EM_ServerType
    {
        NONE = 0,
        MSSQL = 1,
        ORACLE = 2
    }

    public class ConnectionElement
    {
        public string Address = string.Empty;
        public string Catalog = string.Empty;
        public string User = string.Empty;
        public string Password = string.Empty;
        public int Port = -1;
        public int ConnectionTimeout = 15;
        public int CommandTimeout = 180;

        #region 생성자

        public ConnectionElement() { }

        public ConnectionElement(string Ps_Address, string Ps_Catalog, string Ps_User, string Ps_Password)
        {
            Constructor(Ps_Address, Ps_Catalog, Ps_User, Ps_Password);
        }

        public ConnectionElement(string Ps_Address, int Pi_Port, string Ps_Catalog, string Ps_User, string Ps_Password)
        {
            Constructor(Ps_Address, Pi_Port, Ps_Catalog, Ps_User, Ps_Password);
        }

        public ConnectionElement(string Ps_Address, int Pi_Port, string Ps_Catalog, string Ps_User, string Ps_Password, int Pi_ConnectionTime)
        {
            Constructor(Ps_Address, Pi_Port, Ps_Catalog, Ps_User, Ps_Password, Pi_ConnectionTime);
        }

        public ConnectionElement(string Ps_Address, int Pi_Port, string Ps_Catalog, string Ps_User, string Ps_Password, int Pi_ConnectionTime, int Pi_CommandTime)
        {
            Constructor(Ps_Address, Pi_Port, Ps_Catalog, Ps_User, Ps_Password, Pi_ConnectionTime, Pi_CommandTime);
        }

        #endregion

        #region 내부 생성자

        private void Constructor(string Ps_Address, int Pi_Port, string Ps_Catalog, string Ps_User, string Ps_Password, int Pi_ConnectionTime, int Pi_CommandTime)
        {
            Constructor(Ps_Address, Pi_Port, Ps_Catalog, Ps_User, Ps_Password, Pi_ConnectionTime);
            CommandTimeout = Pi_CommandTime;
        }

        private void Constructor(string Ps_Address, int Pi_Port, string Ps_Catalog, string Ps_User, string Ps_Password, int Pi_ConnectionTime)
        {
            Constructor(Ps_Address, Pi_Port, Ps_Catalog, Ps_User, Ps_Password);
            ConnectionTimeout = Pi_ConnectionTime;
        }

        private void Constructor(string Ps_Address, int Pi_Port, string Ps_Catalog, string Ps_User, string Ps_Password)
        {
            Constructor(Ps_Address, Ps_Catalog, Ps_User, Ps_Password);
            Port = Pi_Port;
        }

        private void Constructor(string Ps_Address, string Ps_Catalog, string Ps_User, string Ps_Password)
        {
            Address = Ps_Address;
            Catalog = Ps_Catalog;
            User = Ps_User;
            Password = Ps_Password;
        }

        #endregion

        #region ConnectionString 및 Connection 반환자

        public string GetConnectionString(EM_ServerType P_ServerType)
        {
            string ConnStr = null;
            switch (P_ServerType)
            {
                case EM_ServerType.MSSQL:
                    string Ls_Address = (Port == -1) ? Address : string.Format("{0},{1}", Address, Port);
                    ConnStr = string.Format("Server={0};database={1};uid={2};pwd={3};Connect Timeout={4}",
                              Ls_Address, Catalog, User, Password, ConnectionTimeout);
                    break;

                case EM_ServerType.ORACLE:
                    int Li_Port = (Port == -1) ? 1521 : Port;

                    StringBuilder Ls_SB = new StringBuilder();
                    Ls_SB.Append("(DESCRIPTION =");
                    Ls_SB.Append("   (ADDRESS_LIST =");
                    Ls_SB.AppendFormat("     (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1}))", Address, Li_Port);
                    Ls_SB.Append("   )");
                    Ls_SB.Append("   (CONNECT_DATA =");
                    Ls_SB.AppendFormat("     (SERVICE_NAME = {0})", Catalog);
                    Ls_SB.Append("   )");
                    Ls_SB.Append(")");

                    ConnStr = string.Format("Password={0};User ID={1};Data Source={2}", Password, User, Ls_SB.ToString());
                    break;
            }
            return ConnStr;
        }

        public SqlConnection GetSqlConnection()
        {
            return new SqlConnection(GetConnectionString(EM_ServerType.MSSQL));
        }

        public OracleConnection GetOracleConnection()
        {
            string Ls_Str = GetConnectionString(EM_ServerType.ORACLE);
            return new OracleConnection(Ls_Str);
        }

        #endregion
    }

    public class ConnectionManager
    {
        public static ConnectionElement MainConnection = null;
        public static Dictionary<string, ConnectionElement> ConnectionList = new Dictionary<string,ConnectionElement>();

        #region Add

        public static void Add(string Ps_Name, string Ps_Address, string Ps_Catalog, string Ps_User, string Ps_Password)
        {
            ConnectionElement L_Element = new ConnectionElement(Ps_Address, Ps_Catalog, Ps_User, Ps_Password);
            ConnectionList.Add(Ps_Name, L_Element);
        }

        public static void Add(string Ps_Name, string Ps_Address, int Pi_Port, string Ps_Catalog, string Ps_User, string Ps_Password)
        {
            ConnectionElement L_Element = new ConnectionElement(Ps_Address, Pi_Port, Ps_Catalog, Ps_User, Ps_Password);
            ConnectionList.Add(Ps_Name, L_Element);
        }

        public static void Add(string Ps_Name, string Ps_Address, int Pi_Port, string Ps_Catalog, string Ps_User, string Ps_Password, int Pi_ConnectionTime)
        {
            ConnectionElement L_Element = new ConnectionElement(Ps_Address, Pi_Port, Ps_Catalog, Ps_User, Ps_Password, Pi_ConnectionTime);
            ConnectionList.Add(Ps_Name, L_Element);
        }

        public static void Add(string Ps_Name, string Ps_Address, int Pi_Port, string Ps_Catalog, string Ps_User, string Ps_Password, int Pi_ConnectionTime, int Pi_CommandTime)
        {
            ConnectionElement L_Element = new ConnectionElement(Ps_Address, Pi_Port, Ps_Catalog, Ps_User, Ps_Password, Pi_ConnectionTime, Pi_CommandTime);
            ConnectionList.Add(Ps_Name, L_Element);
        }

        #endregion

    }
}
