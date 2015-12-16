using System;
using System.Windows.Forms;

namespace SqlDotNet
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SetDBList();
            Application.Run(new DB_ORA());
        }

        private static void SetDBList()
        {
            // DB Connection Manager 설정
            DBA.ConnectionManager.Add("ORACLE","172.16.30.200", 1521, "MIS", "sales2015", "war2vall2y");
            //DBA.ConnectionManager.Add("ORACLE", Statics.DB_ADDR, Statics.DB_PORT, Statics.DB_SID, Statics.DB_USER, Statics.DB_PW);

            DBA.ConnectionManager.MainConnection = DBA.ConnectionManager.ConnectionList["ORACLE"];
        }
    }
}
