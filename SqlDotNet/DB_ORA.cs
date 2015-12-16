using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlDotNet
{
    public partial class DB_ORA : Form
    {
        public DB_ORA()
        {
            InitializeComponent();
            InitCombo();
        }

        private void InitCombo()
        {
            combo_UserList.DataSource = GetResult("SELECT USERNAME FROM ALL_USERS ORDER BY USERNAME");
            combo_UserList.DisplayMember = "USERNAME";
        }

        protected DataTable GetResult(string Ps_SQL, OracleParameter[] P_Params = null)
        {
            DataTable Ldt_Result = null;
            try
            {
                Ldt_Result = DBA.OraClient.Query(Ps_SQL, P_Params).Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "DB오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return Ldt_Result;
        }
    }
}
