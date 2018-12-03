using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DbHelper.Properties;
using NLog;
using Oracle.ManagedDataAccess.Client;

namespace DbHelper
{
    public partial class FormMain : Form
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public FormMain()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OracleDb.Init(txtHostname.Text,
                              txtUsername.Text,
                              txtPassword.Text,
                              txtServiceName.Text,
                              txtPort.Text);
                if (OracleDb.CheckConnection())
                {
                    Settings.Default.DbHost = txtHostname.Text;
                    Settings.Default.DbName = txtServiceName.Text;
                    Settings.Default.DbPassword = txtPassword.Text;
                    Settings.Default.DbPort = txtPort.Text;
                    Settings.Default.DbUsername = txtUsername.Text;
                    Settings.Default.Save();

                    UpdateViewCombo();
                    UpdateProcCombo();

                    MessageBox.Show(@"Success!",
                                    @"Info",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                else
                {
                    throw new Exception("Invalid data provided!");
                }
            }
            catch (Exception exp)
            {
                ActionException(exp);
            }

            Cursor = Cursors.Default;
        }

        private void UpdateViewCombo()
        {
            cmbView.Items.Clear();
            OracleDb.ListViews().ForEach(item => { cmbView.Items.Add(item); });

        }

        private void UpdateProcCombo()
        {
            cmbProcedureList.Items.Clear();
            OracleDb.ListPackages(Settings.Default.DbUsername)
                    .ForEach(item => cmbProcedureList.Items.Add(item.Key + "." + item.Value));
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            txtHostname.Text = Settings.Default.DbHost;
            txtServiceName.Text = Settings.Default.DbName;
            txtPassword.Text = Settings.Default.DbPassword;
            txtPort.Text = Settings.Default.DbPort;
            txtUsername.Text = Settings.Default.DbUsername;
#if DEBUG
            //OracleDb.Init(txtHostname.Text,
            //              txtUsername.Text,
            //              txtPassword.Text,
            //              txtServiceName.Text,
            //              txtPort.Text);
            //UpdateViewCombo();
            //UpdateProcCombo();
            //cmbProcedureList.SelectedIndex = 10;
#endif
        }

        private void ActionException(Exception exp)
        {
            Log.Error(exp, exp.Message);
            MessageBox.Show(exp.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnGenerateView_Click(object sender, EventArgs e)
        {
            var sel = cmbView.SelectedItem;
            if (!String.IsNullOrEmpty(sel?.ToString()))
            {
                var selectedItem = sel.ToString();
                var list = OracleDb.ListColumns(selectedItem);
                if (list == null)
                {
                    ActionException(new Exception("Column list is null!"));
                    return;
                }

                var className = Utils.ToUpperCamelCase(selectedItem, false, checkCleanPlural.Checked);

                // Gen class
                txtClass.Text = Utils.GenerateClassData(className, list);

                var funcData = Utils.GenerateViewFunction(className, list, selectedItem);

                txtViewFunction.Text = funcData;
            }
        }



        private void btnGenerateProcedure_Click(object sender, EventArgs e)
        {
            var sel = cmbProcedureList.SelectedItem;
            if (!String.IsNullOrEmpty(sel?.ToString()))
            {
                var spl = sel.ToString().Split('.');
                var packageName = spl[0];
                var procName = spl[1];

                var list = OracleDb.ListProcedureParameters(packageName, procName);

                var str = new StringBuilder();
                var methodHeader = "public static void " +
                                      Utils.ToUpperCamelCase(packageName, true) +
                                      Utils.ToUpperCamelCase(procName, true) + "(\n";
                foreach (var paramList in list.ParamList)
                {
                    str.Append(methodHeader);
                    int i = 0;
                    if (radioSeparate.Checked)
                    {

                        foreach (var info in paramList)
                        {
                            str.Append(info.NetType).Append(" ").Append(info.NameLowerCamelCase);

                            i++;
                            if (i < paramList.Count)
                            {
                                str.Append(",\n");
                            }
                        }
                    }
                    else
                    {
                        str.Append("FooClass item");
                    }

                    str.Append(")\n{\nOracleConnection connection = null;\n");
                    str.Append("try\n{\nconnection = GetConnection();\n");
                    str.Append("const string cmdText =\n\"begin ")
                       .Append(packageName)
                       .Append(".")
                       .Append(procName).Append("(");
                    i = 0;
                    foreach (var info in paramList)
                    {
                        str.Append(info.DbName).Append(" => :").Append(info.DbName);

                        i++;
                        if (i < paramList.Count)
                        {
                            str.Append(",");
                        }
                    }

                    str.Append("); end;\";\n\n");
                    str.Append("using (var cmd = new OracleCommand(cmdText, connection))\n{\n");
                    bool hasOutputParam = false;
                    foreach (var info in paramList)
                    {
                        str.Append("cmd.Parameters.Add(\"").Append(info.DbName);
                        str.Append("\", ").Append(Utils.GetOracleType(info.NetType)).Append(", ");
                        if (info.InParam)
                        {
                            str.Append("ParameterDirection.Input).Value = ");
                            if (radioSeparate.Checked)
                            {
                                str.Append(info.NameLowerCamelCase).Append(";\n");
                            }
                            else
                            {
                                str.Append("item.").Append(info.Name).Append(";\n");
                            }
                        }
                        else
                        {
                            hasOutputParam = true;
                            str.Append("ParameterDirection.Output);\n");
                        }
                    }

                    str.Append("cmd.ExecuteNonQuery();\n");

                    if (hasOutputParam)
                    {
                        i = 0;
                        foreach (var info in paramList)
                        {
                            if (!info.InParam)
                            {
                                str.Append("var var").Append(i).Append(" = ");
                                str.Append("cmd.Parameters[\"").Append(info.DbName).Append("\"]");
                                str.Append(
                                       ".Value == null ? 0 : ((OracleDecimal)")
                                   .Append("cmd.Parameters[\"")
                                   .Append(info.DbName)
                                   .Append("\"]")
                                   .Append(").ToInt64();\n");
                            }
                        }
                    }

                    str.Append(
                        "}\n}\nfinally\n{\nif (connection != null)\n{\nconnection.Close();\nconnection.Dispose();\n}\n}\n}\n\n\n");
                }

                txtProcedure.Text = str.ToString();
            }
        }
    }
}
