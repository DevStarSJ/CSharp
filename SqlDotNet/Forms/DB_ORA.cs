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

// SELECT * FROM ALL_OBJECTS WHERE OBJECT_TYPE IN ('FUNCTION','PROCEDURE','PACKAGE')

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

        private void combo_UserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sUser = combo_UserList.Text.Trim();
            grid_Tables.DataSource = GetResult(
                "SELECT TABLE_NAME FROM ALL_TABLES WHERE OWNER = :owner ORDER BY TABLE_NAME",
                new OracleParameter[] { new OracleParameter("owner", sUser) });
        }

        private void grid_Tables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string sTable = grid_Tables.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;

            if (sTable == null || sTable == "") return;

            string sUser = combo_UserList.Text.Trim();

            grid_Result.DataSource = GetResult(
                string.Format("SELECT * FROM {0}.{1}", sUser, sTable));
        }

        private void btn_Run_Click(object sender, EventArgs e)
        {
            string sText = text_Input.Text.Trim();
            string[] sToken = sText.Split(' ');
            if (sToken[0].ToUpper() == "SELECT")
            {
                grid_Result.DataSource = GetResult(sText);
            }
            else
            {

            }
        }
    }
}
