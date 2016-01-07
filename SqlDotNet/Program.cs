using System;
using System.Windows.Forms;
using Globals;

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

            SetStaticEnvironment();
            SetDBList();

            Application.Run(new DB_ORA());
        }

        private static void SetStaticEnvironment()
        {
            Globals.APP_PATH = Application.StartupPath;
        }

        private static void SetDBList()
        {
            // DB Connection Manager 설정
            //DBA.ConnectionManager.Add("ORACLE","172.16.30.200", 1521, "MIS", "sales2015", "war2vall2y");
            //DBA.ConnectionManager.Add("ORACLE", Statics.DB_ADDR, Statics.DB_PORT, Statics.DB_SID, Statics.DB_USER, Statics.DB_PW);
            //DBA.ConnectionManager.Add("ORACLE", "LOCALHOST", 1521, "ORCL", "scott", "tiger");
            DBA.ConnectionManager.Add("ORACLE", Globals.DB_ADDR, Globals.DB_PORT, Globals.DB_SERVICE, Globals.DB_USER, Globals.DB_PASS);

            DBA.ConnectionManager.MainConnection = DBA.ConnectionManager.ConnectionList["ORACLE"];
        }
    }
}
