using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using DbHelperPostgre.Properties;
using DbHelperPostgre.Properties.SettingsElements;
using NLog;
using Shared;

namespace DbHelperPostgre
{
    public partial class FormMain : Form
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        private readonly Settings _Settings;

        private Db.Db _DataAccess;

        public FormMain()
        {
            _Settings = Program.Settings;
            InitializeComponent();

            var needSave = false;
            if (_Settings.Ui.Width == 0)
            {
                needSave = true;
                _Settings.Ui.Width = Width;
            }

            if (_Settings.Ui.Height == 0)
            {
                needSave = true;
                _Settings.Ui.Height = Height;
            }

            if (needSave)
            {
                _Settings.Save();
            }
        }

        private async void ButtonConnectClick(object sender, EventArgs e)
        {
            await ConnectDb();
        }

        private async Task ConnectDb()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                _DataAccess = new Db.Db(new DbConfigSettingsElement()
                {
                    HostName = txtHostname.Text,
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    Database = txtServiceName.Text
                }
                                       );

                if (await _DataAccess.CheckConnection())
                {
                    _Settings.DbConfig.HostName = txtHostname.Text;
                    _Settings.DbConfig.Database = txtServiceName.Text;
                    _Settings.DbConfig.Password = txtPassword.Text;
                    _Settings.DbConfig.Username = txtUsername.Text;
                    _Settings.Save();

                    UpdateProcCombo();
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

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (await _DataAccess.CheckConnection())
                {
                    UpdateViewCombo();
                    UpdateProcCombo();
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

        private async void UpdateViewCombo()
        {
            ComboView.Items.Clear();
            ComboTablesForProcedureGeneration.Items.Clear();

            var tables = await _DataAccess.ListTables();
            var views = await _DataAccess.ListViews();

            foreach (var item in tables)
            {
                ComboView.Items.Add(new ComboboxItem
                {
                    Id = item,
                    Value = $"{item} (Table)",
                    IsTable = true
                }
                                   );
                ComboTablesForProcedureGeneration.Items.Add(new ComboboxItem
                {
                    Id = item,
                    Value = $"{item} (Table)",
                    IsTable = true
                });
            }

            foreach (var item in views)
            {
                ComboView.Items.Add(new ComboboxItem
                {
                    Id = item,
                    Value = $"{item} (Table)",
                    IsTable = true
                }
                                   );
            }

            ComboView.DisplayMember = "Value";
            ComboView.ValueMember = "Id";

            int i = 0;
            foreach (ComboboxItem item in ComboView.Items)
            {
                if (item.Id == _Settings.Ui.ComboView)
                {
                    ComboView.SelectedIndex = i;
                    break;
                }

                i++;
            }
        }

        private async void UpdateProcCombo()
        {
            ComboProcedureList.Items.Clear();
            var list = await _DataAccess.ListProcedures();
            foreach (var item in list)
            {
                ComboProcedureList.Items.Add(new ComboboxItem
                {
                    Id = item.SpecificName,
                    Value = $"{item.Name} ({item.DbType})",
                    IsTable = false,
                    AdditionalData = item.DbType,
                    ClearName = item.Name
                });
            }

            ComboView.DisplayMember = "Value";
            ComboView.ValueMember = "Id";

            var i = 0;
            foreach (ComboboxItem item in ComboProcedureList.Items)
            {
                if (item.Id == _Settings.Ui.ComboProcedureList)
                {
                    ComboProcedureList.SelectedIndex = i;
                    break;
                }

                i++;
            }
        }

        private async void FormMain_Load(object sender, EventArgs e)
        {
            txtHostname.Text = _Settings.DbConfig.HostName;
            txtServiceName.Text = _Settings.DbConfig.Database;
            txtPassword.Text = _Settings.DbConfig.Password;
            txtUsername.Text = _Settings.DbConfig.Username;

#if DEBUG
            if (!string.IsNullOrEmpty(txtHostname.Text))
            {
                await ConnectDb();
            }

            tabMain.SelectedIndex = _Settings.Ui.TabIndex;
#endif
        }

        private void ActionException(Exception exp)
        {
            Log.Error(exp, exp.Message);
            MessageBox.Show(this, exp.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void ButtonGenerateView_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(ComboView.SelectedItem is ComboboxItem sel))
                {
                    return;
                }

                var selectedItem = sel.Id;
                var list = await _DataAccess.ListColumns(selectedItem, sel.IsTable);
                if (list == null)
                {
                    ActionException(new Exception("Column list is null!"));
                    return;
                }

                var className = selectedItem.ToUpperCamelCase(true, checkCleanPlural.Checked);

                // Gen class
                txtClass.Text = Utils.GenerateClassData(className, list);
                txtViewFunction.Text = Utils.GenerateSelectTableOrViewMethod(className, list, selectedItem);

                _Settings.Ui.ComboView = selectedItem;
                _Settings.Save();
            }
            catch (Exception exp)
            {
                ActionException(exp);
            }
        }

        private async void ButtonGenerateProcedure_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(ComboProcedureList.SelectedItem is ComboboxItem sel))
                {
                    return;
                }

                var selectedName = sel.Id;
                if (string.IsNullOrEmpty(selectedName))
                {
                    return;
                }

                var paramList = await _DataAccess.ListProcedureParameters(selectedName);
                txtProcedure.Text = Utils.GenerateProcedure(sel.ClearName,
                                                            sel.AdditionalData,
                                                            paramList,
                                                            radioSeparate.Checked);
                _Settings.Ui.ComboProcedureList = selectedName;
                _Settings.Save();
            }
            catch (Exception exp)
            {
                ActionException(exp);
            }
        }

        private async void btnGeneratePlSql_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(ComboTablesForProcedureGeneration.SelectedItem is ComboboxItem sel))
                {
                    return;
                }

                if (string.IsNullOrEmpty(sel.Id))
                {
                    return;
                }

                var selectedItem = sel.Id;
                var list = await _DataAccess.ListColumns(selectedItem, true);
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

        private void ComboProcedureList_SelectedValueChanged(object sender, EventArgs e)
        {
            ButtonGenerateProcedure.PerformClick();
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            _Settings.Ui.TabIndex = tabMain.SelectedIndex;
            _Settings.Save();
        }

        private void FormMain_ResizeEnd(object sender, EventArgs e)
        {
            _Settings.Ui.Width = Width;
            _Settings.Ui.Height = Height;
            _Settings.Save();
        }

        private void ComboView_SelectedValueChanged(object sender, EventArgs e)
        {
            ButtonGenerateView.PerformClick();
        }

        private void ComboTablesForProcedureGeneration_SelectedIndexChanged(object sender, EventArgs e)
        {
            ButtonGeneratePlSql.PerformClick();
        }
    }
}