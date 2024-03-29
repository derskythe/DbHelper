﻿namespace DbHelperPostgre
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
            this.txtPort = new System.Windows.Forms.TextBox();
            this.LabelPort = new System.Windows.Forms.Label();
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
            this.webViewViewFunction = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.webViewClass = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.checkCleanPlural = new System.Windows.Forms.CheckBox();
            this.txtViewFunction = new System.Windows.Forms.TextBox();
            this.txtClass = new System.Windows.Forms.TextBox();
            this.ButtonGenerateView = new System.Windows.Forms.Button();
            this.ComboView = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabProc = new System.Windows.Forms.TabPage();
            this.webViewProcedure = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.radioClass = new System.Windows.Forms.RadioButton();
            this.radioSeparate = new System.Windows.Forms.RadioButton();
            this.txtProcedure = new System.Windows.Forms.TextBox();
            this.ButtonGenerateProcedure = new System.Windows.Forms.Button();
            this.ComboProcedureList = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabGenPlSql = new System.Windows.Forms.TabPage();
            this.webViewPlSql = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.txtPlSql = new System.Windows.Forms.TextBox();
            this.ButtonGeneratePlSql = new System.Windows.Forms.Button();
            this.ComboTablesForProcedureGeneration = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabMain.SuspendLayout();
            this.tabConnection.SuspendLayout();
            this.tabViews.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webViewViewFunction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.webViewClass)).BeginInit();
            this.tabProc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webViewProcedure)).BeginInit();
            this.tabGenPlSql.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webViewPlSql)).BeginInit();
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
            this.tabMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1222, 718);
            this.tabMain.TabIndex = 0;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.TabMain_SelectedIndexChanged);
            // 
            // tabConnection
            // 
            this.tabConnection.Controls.Add(this.txtPort);
            this.tabConnection.Controls.Add(this.LabelPort);
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
            this.tabConnection.Location = new System.Drawing.Point(4, 24);
            this.tabConnection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabConnection.Name = "tabConnection";
            this.tabConnection.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabConnection.Size = new System.Drawing.Size(1214, 690);
            this.tabConnection.TabIndex = 0;
            this.tabConnection.Text = "Connection";
            this.tabConnection.UseVisualStyleBackColor = true;
            // 
            // txtPort
            // 
            this.txtPort.HideSelection = false;
            this.txtPort.Location = new System.Drawing.Point(115, 134);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(334, 23);
            this.txtPort.TabIndex = 8;
            // 
            // LabelPort
            // 
            this.LabelPort.AutoSize = true;
            this.LabelPort.Location = new System.Drawing.Point(9, 137);
            this.LabelPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelPort.Name = "LabelPort";
            this.LabelPort.Size = new System.Drawing.Size(29, 15);
            this.LabelPort.TabIndex = 7;
            this.LabelPort.Text = "Port";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(210, 203);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(88, 27);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.ButtonRefresh_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(115, 203);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(88, 27);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.ButtonConnectClick);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(115, 105);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(334, 23);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(115, 76);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(334, 23);
            this.txtUsername.TabIndex = 2;
            // 
            // txtServiceName
            // 
            this.txtServiceName.Location = new System.Drawing.Point(115, 47);
            this.txtServiceName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtServiceName.Name = "txtServiceName";
            this.txtServiceName.Size = new System.Drawing.Size(334, 23);
            this.txtServiceName.TabIndex = 1;
            // 
            // txtHostname
            // 
            this.txtHostname.Location = new System.Drawing.Point(115, 18);
            this.txtHostname.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtHostname.Name = "txtHostname";
            this.txtHostname.Size = new System.Drawing.Size(334, 23);
            this.txtHostname.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 108);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 79);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 51);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Database";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hostname";
            // 
            // tabViews
            // 
            this.tabViews.Controls.Add(this.webViewViewFunction);
            this.tabViews.Controls.Add(this.webViewClass);
            this.tabViews.Controls.Add(this.checkCleanPlural);
            this.tabViews.Controls.Add(this.txtViewFunction);
            this.tabViews.Controls.Add(this.txtClass);
            this.tabViews.Controls.Add(this.ButtonGenerateView);
            this.tabViews.Controls.Add(this.ComboView);
            this.tabViews.Controls.Add(this.label3);
            this.tabViews.Location = new System.Drawing.Point(4, 24);
            this.tabViews.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabViews.Name = "tabViews";
            this.tabViews.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabViews.Size = new System.Drawing.Size(1214, 690);
            this.tabViews.TabIndex = 1;
            this.tabViews.Text = "Views and tables";
            this.tabViews.UseVisualStyleBackColor = true;
            // 
            // webViewViewFunction
            // 
            this.webViewViewFunction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.webViewViewFunction.CreationProperties = null;
            this.webViewViewFunction.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webViewViewFunction.Location = new System.Drawing.Point(611, 78);
            this.webViewViewFunction.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.webViewViewFunction.Name = "webViewViewFunction";
            this.webViewViewFunction.Size = new System.Drawing.Size(598, 612);
            this.webViewViewFunction.TabIndex = 11;
            this.webViewViewFunction.ZoomFactor = 1D;
            // 
            // webViewClass
            // 
            this.webViewClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webViewClass.CreationProperties = null;
            this.webViewClass.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webViewClass.Location = new System.Drawing.Point(7, 78);
            this.webViewClass.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.webViewClass.Name = "webViewClass";
            this.webViewClass.Size = new System.Drawing.Size(602, 612);
            this.webViewClass.TabIndex = 10;
            this.webViewClass.ZoomFactor = 1D;
            // 
            // checkCleanPlural
            // 
            this.checkCleanPlural.AutoSize = true;
            this.checkCleanPlural.Location = new System.Drawing.Point(92, 54);
            this.checkCleanPlural.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkCleanPlural.Name = "checkCleanPlural";
            this.checkCleanPlural.Size = new System.Drawing.Size(89, 19);
            this.checkCleanPlural.TabIndex = 4;
            this.checkCleanPlural.Text = "Clean plural";
            this.checkCleanPlural.UseVisualStyleBackColor = true;
            // 
            // txtViewFunction
            // 
            this.txtViewFunction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtViewFunction.Location = new System.Drawing.Point(616, 81);
            this.txtViewFunction.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtViewFunction.Multiline = true;
            this.txtViewFunction.Name = "txtViewFunction";
            this.txtViewFunction.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtViewFunction.Size = new System.Drawing.Size(572, 588);
            this.txtViewFunction.TabIndex = 3;
            this.txtViewFunction.Visible = false;
            // 
            // txtClass
            // 
            this.txtClass.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClass.Location = new System.Drawing.Point(36, 81);
            this.txtClass.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtClass.Multiline = true;
            this.txtClass.Name = "txtClass";
            this.txtClass.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtClass.Size = new System.Drawing.Size(574, 588);
            this.txtClass.TabIndex = 3;
            this.txtClass.Visible = false;
            // 
            // ButtonGenerateView
            // 
            this.ButtonGenerateView.Location = new System.Drawing.Point(1104, 23);
            this.ButtonGenerateView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ButtonGenerateView.Name = "ButtonGenerateView";
            this.ButtonGenerateView.Size = new System.Drawing.Size(88, 27);
            this.ButtonGenerateView.TabIndex = 2;
            this.ButtonGenerateView.Text = "Generate";
            this.ButtonGenerateView.UseVisualStyleBackColor = true;
            this.ButtonGenerateView.Click += new System.EventHandler(this.ButtonGenerateView_Click);
            // 
            // ComboView
            // 
            this.ComboView.DisplayMember = "Id";
            this.ComboView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboView.Location = new System.Drawing.Point(164, 23);
            this.ComboView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ComboView.Name = "ComboView";
            this.ComboView.Size = new System.Drawing.Size(931, 23);
            this.ComboView.Sorted = true;
            this.ComboView.TabIndex = 1;
            this.ComboView.ValueMember = "Value";
            this.ComboView.SelectedValueChanged += new System.EventHandler(this.ComboView_SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 26);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "View and tables list";
            // 
            // tabProc
            // 
            this.tabProc.Controls.Add(this.webViewProcedure);
            this.tabProc.Controls.Add(this.radioClass);
            this.tabProc.Controls.Add(this.radioSeparate);
            this.tabProc.Controls.Add(this.txtProcedure);
            this.tabProc.Controls.Add(this.ButtonGenerateProcedure);
            this.tabProc.Controls.Add(this.ComboProcedureList);
            this.tabProc.Controls.Add(this.label7);
            this.tabProc.Location = new System.Drawing.Point(4, 24);
            this.tabProc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabProc.Name = "tabProc";
            this.tabProc.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabProc.Size = new System.Drawing.Size(1214, 690);
            this.tabProc.TabIndex = 2;
            this.tabProc.Text = "Procedures";
            this.tabProc.UseVisualStyleBackColor = true;
            // 
            // webViewProcedure
            // 
            this.webViewProcedure.CreationProperties = null;
            this.webViewProcedure.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webViewProcedure.Location = new System.Drawing.Point(27, 94);
            this.webViewProcedure.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.webViewProcedure.Name = "webViewProcedure";
            this.webViewProcedure.Size = new System.Drawing.Size(1156, 573);
            this.webViewProcedure.TabIndex = 9;
            this.webViewProcedure.ZoomFactor = 1D;
            // 
            // radioClass
            // 
            this.radioClass.AutoSize = true;
            this.radioClass.Location = new System.Drawing.Point(241, 54);
            this.radioClass.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioClass.Name = "radioClass";
            this.radioClass.Size = new System.Drawing.Size(88, 19);
            this.radioClass.TabIndex = 8;
            this.radioClass.Text = "Class values";
            this.radioClass.UseVisualStyleBackColor = true;
            // 
            // radioSeparate
            // 
            this.radioSeparate.AutoSize = true;
            this.radioSeparate.Checked = true;
            this.radioSeparate.Location = new System.Drawing.Point(105, 54);
            this.radioSeparate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioSeparate.Name = "radioSeparate";
            this.radioSeparate.Size = new System.Drawing.Size(106, 19);
            this.radioSeparate.TabIndex = 8;
            this.radioSeparate.TabStop = true;
            this.radioSeparate.Text = "Separate values";
            this.radioSeparate.UseVisualStyleBackColor = true;
            this.radioSeparate.CheckedChanged += new System.EventHandler(this.radioSeparate_CheckedChanged);
            // 
            // txtProcedure
            // 
            this.txtProcedure.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProcedure.Location = new System.Drawing.Point(30, 113);
            this.txtProcedure.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtProcedure.Multiline = true;
            this.txtProcedure.Name = "txtProcedure";
            this.txtProcedure.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProcedure.Size = new System.Drawing.Size(1154, 555);
            this.txtProcedure.TabIndex = 7;
            this.txtProcedure.Visible = false;
            // 
            // ButtonGenerateProcedure
            // 
            this.ButtonGenerateProcedure.Location = new System.Drawing.Point(1098, 22);
            this.ButtonGenerateProcedure.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ButtonGenerateProcedure.Name = "ButtonGenerateProcedure";
            this.ButtonGenerateProcedure.Size = new System.Drawing.Size(88, 27);
            this.ButtonGenerateProcedure.TabIndex = 6;
            this.ButtonGenerateProcedure.Text = "Generate";
            this.ButtonGenerateProcedure.UseVisualStyleBackColor = true;
            this.ButtonGenerateProcedure.Click += new System.EventHandler(this.ButtonGenerateProcedure_Click);
            // 
            // ComboProcedureList
            // 
            this.ComboProcedureList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboProcedureList.Location = new System.Drawing.Point(105, 22);
            this.ComboProcedureList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ComboProcedureList.Name = "ComboProcedureList";
            this.ComboProcedureList.Size = new System.Drawing.Size(984, 23);
            this.ComboProcedureList.Sorted = true;
            this.ComboProcedureList.TabIndex = 5;
            this.ComboProcedureList.SelectedValueChanged += new System.EventHandler(this.ComboProcedureList_SelectedValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 24);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 15);
            this.label7.TabIndex = 4;
            this.label7.Text = "Procedures";
            // 
            // tabGenPlSql
            // 
            this.tabGenPlSql.Controls.Add(this.webViewPlSql);
            this.tabGenPlSql.Controls.Add(this.txtPlSql);
            this.tabGenPlSql.Controls.Add(this.ButtonGeneratePlSql);
            this.tabGenPlSql.Controls.Add(this.ComboTablesForProcedureGeneration);
            this.tabGenPlSql.Controls.Add(this.label8);
            this.tabGenPlSql.Location = new System.Drawing.Point(4, 24);
            this.tabGenPlSql.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabGenPlSql.Name = "tabGenPlSql";
            this.tabGenPlSql.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabGenPlSql.Size = new System.Drawing.Size(1214, 690);
            this.tabGenPlSql.TabIndex = 3;
            this.tabGenPlSql.Text = "Generate PL/SQL proc from table";
            this.tabGenPlSql.UseVisualStyleBackColor = true;
            // 
            // webViewPlSql
            // 
            this.webViewPlSql.CreationProperties = null;
            this.webViewPlSql.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webViewPlSql.Location = new System.Drawing.Point(7, 113);
            this.webViewPlSql.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.webViewPlSql.Name = "webViewPlSql";
            this.webViewPlSql.Size = new System.Drawing.Size(1201, 573);
            this.webViewPlSql.TabIndex = 10;
            this.webViewPlSql.ZoomFactor = 1D;
            // 
            // txtPlSql
            // 
            this.txtPlSql.Location = new System.Drawing.Point(30, 113);
            this.txtPlSql.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtPlSql.Multiline = true;
            this.txtPlSql.Name = "txtPlSql";
            this.txtPlSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPlSql.Size = new System.Drawing.Size(1154, 552);
            this.txtPlSql.TabIndex = 8;
            this.txtPlSql.Visible = false;
            // 
            // ButtonGeneratePlSql
            // 
            this.ButtonGeneratePlSql.Location = new System.Drawing.Point(1098, 32);
            this.ButtonGeneratePlSql.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ButtonGeneratePlSql.Name = "ButtonGeneratePlSql";
            this.ButtonGeneratePlSql.Size = new System.Drawing.Size(88, 27);
            this.ButtonGeneratePlSql.TabIndex = 5;
            this.ButtonGeneratePlSql.Text = "Generate";
            this.ButtonGeneratePlSql.UseVisualStyleBackColor = true;
            this.ButtonGeneratePlSql.Click += new System.EventHandler(this.ButtonGeneratePlSql_Click);
            // 
            // ComboTablesForProcedureGeneration
            // 
            this.ComboTablesForProcedureGeneration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboTablesForProcedureGeneration.Location = new System.Drawing.Point(86, 32);
            this.ComboTablesForProcedureGeneration.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ComboTablesForProcedureGeneration.Name = "ComboTablesForProcedureGeneration";
            this.ComboTablesForProcedureGeneration.Size = new System.Drawing.Size(1003, 23);
            this.ComboTablesForProcedureGeneration.Sorted = true;
            this.ComboTablesForProcedureGeneration.TabIndex = 4;
            this.ComboTablesForProcedureGeneration.SelectedIndexChanged += new System.EventHandler(this.ComboTablesForProcedureGeneration_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(27, 32);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 15);
            this.label8.TabIndex = 3;
            this.label8.Text = "Table list";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1222, 718);
            this.Controls.Add(this.tabMain);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FormMain";
            this.Text = ".NET Database Helper";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResizeEnd += new System.EventHandler(this.FormMain_ResizeEnd);
            this.tabMain.ResumeLayout(false);
            this.tabConnection.ResumeLayout(false);
            this.tabConnection.PerformLayout();
            this.tabViews.ResumeLayout(false);
            this.tabViews.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webViewViewFunction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.webViewClass)).EndInit();
            this.tabProc.ResumeLayout(false);
            this.tabProc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webViewProcedure)).EndInit();
            this.tabGenPlSql.ResumeLayout(false);
            this.tabGenPlSql.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webViewPlSql)).EndInit();
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
        private System.Windows.Forms.Button ButtonGenerateView;
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
        private System.Windows.Forms.Button ButtonGeneratePlSql;
        private System.Windows.Forms.ComboBox ComboTablesForProcedureGeneration;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPlSql;
        private System.Windows.Forms.Button btnRefresh;
        private Microsoft.Web.WebView2.WinForms.WebView2 webViewProcedure;
        private Microsoft.Web.WebView2.WinForms.WebView2 webViewClass;
        private Microsoft.Web.WebView2.WinForms.WebView2 webViewViewFunction;
        private Microsoft.Web.WebView2.WinForms.WebView2 webViewPlSql;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label LabelPort;
    }
}

