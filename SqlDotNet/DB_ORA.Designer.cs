namespace SqlDotNet
{
    partial class DB_ORA
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_UserList = new System.Windows.Forms.Label();
            this.combo_UserList = new System.Windows.Forms.ComboBox();
            this.splitCon_main = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_Tables = new System.Windows.Forms.TabPage();
            this.tab_Procs = new System.Windows.Forms.TabPage();
            this.grid_Tables = new System.Windows.Forms.DataGridView();
            this.grid_Procs = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_main)).BeginInit();
            this.splitCon_main.Panel1.SuspendLayout();
            this.splitCon_main.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tab_Tables.SuspendLayout();
            this.tab_Procs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Tables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Procs)).BeginInit();
            this.SuspendLayout();
            // 
            // label_UserList
            // 
            this.label_UserList.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_UserList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_UserList.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_UserList.ForeColor = System.Drawing.Color.White;
            this.label_UserList.Location = new System.Drawing.Point(5, 5);
            this.label_UserList.Name = "label_UserList";
            this.label_UserList.Size = new System.Drawing.Size(80, 25);
            this.label_UserList.TabIndex = 0;
            this.label_UserList.Text = "User List";
            this.label_UserList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // combo_UserList
            // 
            this.combo_UserList.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.combo_UserList.FormattingEnabled = true;
            this.combo_UserList.Location = new System.Drawing.Point(90, 7);
            this.combo_UserList.Name = "combo_UserList";
            this.combo_UserList.Size = new System.Drawing.Size(132, 23);
            this.combo_UserList.TabIndex = 1;
            // 
            // splitCon_main
            // 
            this.splitCon_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCon_main.Location = new System.Drawing.Point(0, 0);
            this.splitCon_main.Name = "splitCon_main";
            // 
            // splitCon_main.Panel1
            // 
            this.splitCon_main.Panel1.BackColor = System.Drawing.Color.White;
            this.splitCon_main.Panel1.Controls.Add(this.tabControl1);
            this.splitCon_main.Panel1.Controls.Add(this.label_UserList);
            this.splitCon_main.Panel1.Controls.Add(this.combo_UserList);
            this.splitCon_main.Size = new System.Drawing.Size(818, 514);
            this.splitCon_main.SplitterDistance = 227;
            this.splitCon_main.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tab_Tables);
            this.tabControl1.Controls.Add(this.tab_Procs);
            this.tabControl1.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabControl1.Location = new System.Drawing.Point(5, 36);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(217, 475);
            this.tabControl1.TabIndex = 2;
            // 
            // tab_Tables
            // 
            this.tab_Tables.Controls.Add(this.grid_Tables);
            this.tab_Tables.Location = new System.Drawing.Point(4, 24);
            this.tab_Tables.Name = "tab_Tables";
            this.tab_Tables.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Tables.Size = new System.Drawing.Size(209, 447);
            this.tab_Tables.TabIndex = 0;
            this.tab_Tables.Text = "Tables";
            this.tab_Tables.UseVisualStyleBackColor = true;
            // 
            // tab_Procs
            // 
            this.tab_Procs.Controls.Add(this.grid_Procs);
            this.tab_Procs.Location = new System.Drawing.Point(4, 24);
            this.tab_Procs.Name = "tab_Procs";
            this.tab_Procs.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Procs.Size = new System.Drawing.Size(209, 447);
            this.tab_Procs.TabIndex = 1;
            this.tab_Procs.Text = "Procedures";
            this.tab_Procs.UseVisualStyleBackColor = true;
            // 
            // grid_Tables
            // 
            this.grid_Tables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Tables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_Tables.Location = new System.Drawing.Point(3, 3);
            this.grid_Tables.Name = "grid_Tables";
            this.grid_Tables.RowTemplate.Height = 23;
            this.grid_Tables.Size = new System.Drawing.Size(203, 441);
            this.grid_Tables.TabIndex = 0;
            // 
            // grid_Procs
            // 
            this.grid_Procs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Procs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_Procs.Location = new System.Drawing.Point(3, 3);
            this.grid_Procs.Name = "grid_Procs";
            this.grid_Procs.RowTemplate.Height = 23;
            this.grid_Procs.Size = new System.Drawing.Size(203, 441);
            this.grid_Procs.TabIndex = 1;
            // 
            // DB_ORA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 514);
            this.Controls.Add(this.splitCon_main);
            this.Name = "DB_ORA";
            this.Text = "SQL.Net for Oracle";
            this.splitCon_main.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_main)).EndInit();
            this.splitCon_main.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tab_Tables.ResumeLayout(false);
            this.tab_Procs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_Tables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Procs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_UserList;
        private System.Windows.Forms.ComboBox combo_UserList;
        private System.Windows.Forms.SplitContainer splitCon_main;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab_Tables;
        private System.Windows.Forms.DataGridView grid_Tables;
        private System.Windows.Forms.TabPage tab_Procs;
        private System.Windows.Forms.DataGridView grid_Procs;
    }
}

