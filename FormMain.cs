using System;
using System.Windows.Forms;
using DbHelper.Properties;
using NLog;

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

                    UpdateProcCombo();
                    UpdateTableCombo();
                    UpdateViewCombo();
                    MessageBoxEx.Show(this, @"Success!", @"Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (OracleDb.CheckConnection())
                {
                    UpdateViewCombo();
                    UpdateProcCombo();
                    UpdateTableCombo();
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

        private void UpdateTableCombo()
        {
            cmbTable.Items.Clear();
            OracleDb.ListTables()?.ForEach(item => { cmbTable.Items.Add(item); });
        }

        private void UpdateViewCombo()
        {
            cmbView.Items.Clear();
            OracleDb.ListViews()?.ForEach(item => { cmbView.Items.Add(item); });
        }

        private void UpdateProcCombo()
        {
            cmbProcedureList.Items.Clear();
            OracleDb.ListPackages(Settings.Default.DbUsername)
                    ?.ForEach(item => cmbProcedureList.Items.Add(item.Key + "." + item.Value));
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
            MessageBox.Show(this, exp.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnGenerateView_Click(object sender, EventArgs e)
        {
            try
            {
                var sel = cmbView.SelectedItem;
                if (string.IsNullOrEmpty(sel?.ToString()))
                {
                    return;
                }

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
            catch (Exception exp)
            {
                ActionException(exp);
            }
        }

        private void btnGenerateProcedure_Click(object sender, EventArgs e)
        {
            try
            {
                var sel = cmbProcedureList.SelectedItem;
                var selectedItem = sel?.ToString();
                if (string.IsNullOrEmpty(selectedItem))
                {
                    return;
                }

                txtProcedure.Text = Utils.GenerateProcedure(selectedItem, radioSeparate.Checked);
            }
            catch (Exception exp)
            {
                ActionException(exp);
            }
        }

        private void btnGeneratePlSql_Click(object sender, EventArgs e)
        {
            try
            {
                var sel = cmbTable.SelectedItem;
                if (string.IsNullOrEmpty(sel?.ToString()))
                {
                    return;
                }

                var selectedItem = sel.ToString();
                var list = OracleDb.ListColumns(selectedItem);
                if (list == null)
                {
                    ActionException(new Exception("Column list is null!"));
                    return;
                }

                var result = Utils.GeneratePlSqlProcedure(selectedItem, list);
                txtPlSql.Text = result;
            }
            catch (Exception exp)
            {
                ActionException(exp);
            }
        }
    }
}
