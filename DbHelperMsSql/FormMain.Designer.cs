namespace DbHelper
{
    partial class FormMain
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
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabConnection = new System.Windows.Forms.TabPage();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtServiceName = new System.Windows.Forms.TextBox();
            this.txtHostname = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabViews = new System.Windows.Forms.TabPage();
            this.checkCleanPlural = new System.Windows.Forms.CheckBox();
            this.txtViewFunction = new System.Windows.Forms.TextBox();
            this.txtClass = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.ComboView = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabProc = new System.Windows.Forms.TabPage();
            this.radioClass = new System.Windows.Forms.RadioButton();
            this.radioSeparate = new System.Windows.Forms.RadioButton();
            this.txtProcedure = new System.Windows.Forms.TextBox();
            this.ButtonGenerateProcedure = new System.Windows.Forms.Button();
            this.ComboProcedureList = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabGenPlSql = new System.Windows.Forms.TabPage();
            this.txtPlSql = new System.Windows.Forms.TextBox();
            this.btnGeneratePlSql = new System.Windows.Forms.Button();
            this.cmbTable = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabMain.SuspendLayout();
            this.tabConnection.SuspendLayout();
            this.tabViews.SuspendLayout();
            this.tabProc.SuspendLayout();
            this.tabGenPlSql.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabConnection);
            this.tabMain.Controls.Add(this.tabViews);
            this.tabMain.Controls.Add(this.tabProc);
            this.tabMain.Controls.Add(this.tabGenPlSql);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1397, 766);
            this.tabMain.TabIndex = 0;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            // 
            // tabConnection
            // 
            this.tabConnection.Controls.Add(this.btnRefresh);
            this.tabConnection.Controls.Add(this.btnConnect);
            this.tabConnection.Controls.Add(this.txtPassword);
            this.tabConnection.Controls.Add(this.txtUsername);
            this.tabConnection.Controls.Add(this.txtServiceName);
            this.tabConnection.Controls.Add(this.txtHostname);
            this.tabConnection.Controls.Add(this.label6);
            this.tabConnection.Controls.Add(this.label4);
            this.tabConnection.Controls.Add(this.label2);
            this.tabConnection.Controls.Add(this.label1);
            this.tabConnection.Location = new System.Drawing.Point(4, 25);
            this.tabConnection.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.tabConnection.Name = "tabConnection";
            this.tabConnection.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.tabConnection.Size = new System.Drawing.Size(1389, 737);
            this.tabConnection.TabIndex = 0;
            this.tabConnection.Text = "Connection";
            this.tabConnection.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(240, 217);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(101, 29);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(131, 217);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(101, 29);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.ButtonConnectClick);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(131, 134);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(381, 22);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(131, 96);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(381, 22);
            this.txtUsername.TabIndex = 2;
            // 
            // txtServiceName
            // 
            this.txtServiceName.Location = new System.Drawing.Point(131, 58);
            this.txtServiceName.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtServiceName.Name = "txtServiceName";
            this.txtServiceName.Size = new System.Drawing.Size(381, 22);
            this.txtServiceName.TabIndex = 1;
            // 
            // txtHostname
            // 
            this.txtHostname.Location = new System.Drawing.Point(131, 19);
            this.txtHostname.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtHostname.Name = "txtHostname";
            this.txtHostname.Size = new System.Drawing.Size(381, 22);
            this.txtHostname.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 138);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 17);
            this.label6.TabIndex = 1;
            this.label6.Text = "Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 99);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Database";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hostname";
            // 
            // tabViews
            // 
            this.tabViews.Controls.Add(this.checkCleanPlural);
            this.tabViews.Controls.Add(this.txtViewFunction);
            this.tabViews.Controls.Add(this.txtClass);
            this.tabViews.Controls.Add(this.btnGenerate);
            this.tabViews.Controls.Add(this.ComboView);
            this.tabViews.Controls.Add(this.label3);
            this.tabViews.Location = new System.Drawing.Point(4, 25);
            this.tabViews.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.tabViews.Name = "tabViews";
            this.tabViews.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.tabViews.Size = new System.Drawing.Size(1389, 737);
            this.tabViews.TabIndex = 1;
            this.tabViews.Text = "Views and tables";
            this.tabViews.UseVisualStyleBackColor = true;
            // 
            // checkCleanPlural
            // 
            this.checkCleanPlural.AutoSize = true;
            this.checkCleanPlural.Location = new System.Drawing.Point(105, 58);
            this.checkCleanPlural.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.checkCleanPlural.Name = "checkCleanPlural";
            this.checkCleanPlural.Size = new System.Drawing.Size(105, 21);
            this.checkCleanPlural.TabIndex = 4;
            this.checkCleanPlural.Text = "Clean plural";
            this.checkCleanPlural.UseVisualStyleBackColor = true;
            // 
            // txtViewFunction
            // 
            this.txtViewFunction.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtViewFunction.Location = new System.Drawing.Point(706, 86);
            this.txtViewFunction.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtViewFunction.Multiline = true;
            this.txtViewFunction.Name = "txtViewFunction";
            this.txtViewFunction.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtViewFunction.Size = new System.Drawing.Size(653, 626);
            this.txtViewFunction.TabIndex = 3;
            // 
            // txtClass
            // 
            this.txtClass.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClass.Location = new System.Drawing.Point(41, 86);
            this.txtClass.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtClass.Multiline = true;
            this.txtClass.Name = "txtClass";
            this.txtClass.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtClass.Size = new System.Drawing.Size(655, 626);
            this.txtClass.TabIndex = 3;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(1262, 25);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(101, 29);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.ButtonGenerateView_Click);
            // 
            // ComboView
            // 
            this.ComboView.DisplayMember = "Id";
            this.ComboView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboView.Location = new System.Drawing.Point(188, 25);
            this.ComboView.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ComboView.Name = "ComboView";
            this.ComboView.Size = new System.Drawing.Size(1063, 24);
            this.ComboView.Sorted = true;
            this.ComboView.TabIndex = 1;
            this.ComboView.ValueMember = "Value";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 27);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "View and tables list";
            // 
            // tabProc
            // 
            this.tabProc.Controls.Add(this.radioClass);
            this.tabProc.Controls.Add(this.radioSeparate);
            this.tabProc.Controls.Add(this.txtProcedure);
            this.tabProc.Controls.Add(this.ButtonGenerateProcedure);
            this.tabProc.Controls.Add(this.ComboProcedureList);
            this.tabProc.Controls.Add(this.label7);
            this.tabProc.Location = new System.Drawing.Point(4, 25);
            this.tabProc.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.tabProc.Name = "tabProc";
            this.tabProc.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.tabProc.Size = new System.Drawing.Size(1389, 737);
            this.tabProc.TabIndex = 2;
            this.tabProc.Text = "Procedures";
            this.tabProc.UseVisualStyleBackColor = true;
            // 
            // radioClass
            // 
            this.radioClass.AutoSize = true;
            this.radioClass.Location = new System.Drawing.Point(275, 58);
            this.radioClass.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.radioClass.Name = "radioClass";
            this.radioClass.Size = new System.Drawing.Size(108, 21);
            this.radioClass.TabIndex = 8;
            this.radioClass.Text = "Class values";
            this.radioClass.UseVisualStyleBackColor = true;
            // 
            // radioSeparate
            // 
            this.radioSeparate.AutoSize = true;
            this.radioSeparate.Checked = true;
            this.radioSeparate.Location = new System.Drawing.Point(120, 58);
            this.radioSeparate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.radioSeparate.Name = "radioSeparate";
            this.radioSeparate.Size = new System.Drawing.Size(132, 21);
            this.radioSeparate.TabIndex = 8;
            this.radioSeparate.TabStop = true;
            this.radioSeparate.Text = "Separate values";
            this.radioSeparate.UseVisualStyleBackColor = true;
            // 
            // txtProcedure
            // 
            this.txtProcedure.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProcedure.Location = new System.Drawing.Point(34, 121);
            this.txtProcedure.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtProcedure.Multiline = true;
            this.txtProcedure.Name = "txtProcedure";
            this.txtProcedure.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProcedure.Size = new System.Drawing.Size(1318, 592);
            this.txtProcedure.TabIndex = 7;
            // 
            // ButtonGenerateProcedure
            // 
            this.ButtonGenerateProcedure.Location = new System.Drawing.Point(1255, 23);
            this.ButtonGenerateProcedure.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ButtonGenerateProcedure.Name = "ButtonGenerateProcedure";
            this.ButtonGenerateProcedure.Size = new System.Drawing.Size(101, 29);
            this.ButtonGenerateProcedure.TabIndex = 6;
            this.ButtonGenerateProcedure.Text = "Generate";
            this.ButtonGenerateProcedure.UseVisualStyleBackColor = true;
            this.ButtonGenerateProcedure.Click += new System.EventHandler(this.ButtonGenerateProcedure_Click);
            // 
            // ComboProcedureList
            // 
            this.ComboProcedureList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboProcedureList.Location = new System.Drawing.Point(120, 23);
            this.ComboProcedureList.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ComboProcedureList.Name = "ComboProcedureList";
            this.ComboProcedureList.Size = new System.Drawing.Size(1124, 24);
            this.ComboProcedureList.Sorted = true;
            this.ComboProcedureList.TabIndex = 5;
            this.ComboProcedureList.SelectedValueChanged += new System.EventHandler(this.ComboProcedureList_SelectedValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 26);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 17);
            this.label7.TabIndex = 4;
            this.label7.Text = "Procedures";
            // 
            // tabGenPlSql
            // 
            this.tabGenPlSql.Controls.Add(this.txtPlSql);
            this.tabGenPlSql.Controls.Add(this.btnGeneratePlSql);
            this.tabGenPlSql.Controls.Add(this.cmbTable);
            this.tabGenPlSql.Controls.Add(this.label8);
            this.tabGenPlSql.Location = new System.Drawing.Point(4, 25);
            this.tabGenPlSql.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.tabGenPlSql.Name = "tabGenPlSql";
            this.tabGenPlSql.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.tabGenPlSql.Size = new System.Drawing.Size(1389, 737);
            this.tabGenPlSql.TabIndex = 3;
            this.tabGenPlSql.Text = "Generate PL/SQL proc from table";
            this.tabGenPlSql.UseVisualStyleBackColor = true;
            // 
            // txtPlSql
            // 
            this.txtPlSql.Location = new System.Drawing.Point(34, 121);
            this.txtPlSql.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtPlSql.Multiline = true;
            this.txtPlSql.Name = "txtPlSql";
            this.txtPlSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPlSql.Size = new System.Drawing.Size(1318, 589);
            this.txtPlSql.TabIndex = 8;
            // 
            // btnGeneratePlSql
            // 
            this.btnGeneratePlSql.Location = new System.Drawing.Point(1255, 34);
            this.btnGeneratePlSql.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnGeneratePlSql.Name = "btnGeneratePlSql";
            this.btnGeneratePlSql.Size = new System.Drawing.Size(101, 29);
            this.btnGeneratePlSql.TabIndex = 5;
            this.btnGeneratePlSql.Text = "Generate";
            this.btnGeneratePlSql.UseVisualStyleBackColor = true;
            this.btnGeneratePlSql.Click += new System.EventHandler(this.btnGeneratePlSql_Click);
            // 
            // cmbTable
            // 
            this.cmbTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTable.Location = new System.Drawing.Point(98, 34);
            this.cmbTable.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.cmbTable.Name = "cmbTable";
            this.cmbTable.Size = new System.Drawing.Size(1146, 24);
            this.cmbTable.Sorted = true;
            this.cmbTable.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 34);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 17);
            this.label8.TabIndex = 3;
            this.label8.Text = "Table list";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1397, 766);
            this.Controls.Add(this.tabMain);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "FormMain";
            this.Text = ".NET Database Helper";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResizeEnd += new System.EventHandler(this.FormMain_ResizeEnd);
            this.tabMain.ResumeLayout(false);
            this.tabConnection.ResumeLayout(false);
            this.tabConnection.PerformLayout();
            this.tabViews.ResumeLayout(false);
            this.tabViews.PerformLayout();
            this.tabProc.ResumeLayout(false);
            this.tabProc.PerformLayout();
            this.tabGenPlSql.ResumeLayout(false);
            this.tabGenPlSql.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabConnection;
        private System.Windows.Forms.TabPage tabViews;
        private System.Windows.Forms.TextBox txtServiceName;
        private System.Windows.Forms.TextBox txtHostname;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.ComboBox ComboView;
        private System.Windows.Forms.TextBox txtViewFunction;
        private System.Windows.Forms.TextBox txtClass;
        private System.Windows.Forms.TabPage tabProc;
        private System.Windows.Forms.TextBox txtProcedure;
        private System.Windows.Forms.Button ButtonGenerateProcedure;
        private System.Windows.Forms.ComboBox ComboProcedureList;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton radioClass;
        private System.Windows.Forms.RadioButton radioSeparate;
        private System.Windows.Forms.CheckBox checkCleanPlural;
        private System.Windows.Forms.TabPage tabGenPlSql;
        private System.Windows.Forms.Button btnGeneratePlSql;
        private System.Windows.Forms.ComboBox cmbTable;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPlSql;
        private System.Windows.Forms.Button btnRefresh;
    }
}

