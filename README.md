##DBManager
- Oracle, MS-SQL, MySQL, FireBird, MS-Access 에 접속하여 SQL문 수행이 가능한 Library

```C#

// DB 등록
DBA.ConnectionManager.Add("ORACLE","192.168.70.200", 1521, "MIS", "sales", "oraora");

// 등록된 DB중 명시하지 않았을 때 Default로 적용
DBA.ConnectionManager.MainConnection = DBA.ConnectionManager.ConnectionList["ORACLE"];


string Ps_SQL = @"SELECT * FROM SCOTT.EMP";
Oracle.DataAccess.Client.OracleParameter[] P_Params = null;
DataTable Ldt_Result = null;

try
{
    Ldt_Result = DBA.OraClient.Query(Ps_SQL, P_Params).Tables[0];
}
catch (Exception e)
{
	MessageBox.Show(e.Message, "DB오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```