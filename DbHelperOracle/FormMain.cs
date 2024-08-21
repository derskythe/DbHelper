using System;
using System.Windows.Forms;
using DbHelperOracle.Db;
using DbHelperOracle.Properties;
using NLog;
using Shared;

namespace DbHelperOracle;


public partial class FormMain : Form
{
    // ReSharper disable FieldCanBeMadeReadOnly.Local
    // ReSharper disable InconsistentNaming
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // ReSharper restore InconsistentNaming
    // ReSharper restore FieldCanBeMadeReadOnly.Local
    private readonly Settings _Settings;

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

    private void ButtonConnect_Click(object sender, EventArgs e)
    {
        Connect();
    }

    private void Connect()
    {
        Cursor = Cursors.WaitCursor;

        try
        {
            OracleDb.Init(txtHostname.Text,
                          txtUsername.Text,
                          txtPassword.Text,
                          txtServiceName.Text,
                          txtPort.Text
            );

            if (OracleDb.CheckConnection())
            {
                _Settings.DbConfig.HostName = txtHostname.Text;
                _Settings.DbConfig.ServiceName = txtServiceName.Text;
                _Settings.DbConfig.Password = txtPassword.Text;
                _Settings.DbConfig.Port = Convert.ToInt32(txtPort.Text);
                _Settings.DbConfig.Username = txtUsername.Text;
                _Settings.Save();

                UpdateProcCombo();
                UpdateTableCombo();
                UpdateViewCombo();

                MessageBoxEx.Show(
                    this,
                    @"Success!",
                    @"Info",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
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

    private void ButtonRefresh_Click(object sender, EventArgs e)
    {
        Connect();
    }

    private void UpdateTableCombo()
    {
        ComboTableForProcedureGenerate.Items.Clear();

        // ReSharper disable once CoVariantArrayConversion
        ComboTableForProcedureGenerate.Items.AddRange(OracleDb.ListTables().ToArray());
        ComboTableForProcedureGenerate.DisplayMember = "Value";
        ComboTableForProcedureGenerate.ValueMember = "Id";

        var i = 0;

        foreach (ComboboxItem item in ComboTableForProcedureGenerate.Items)
        {
            if (item.Id == _Settings.Ui.ComboView)
            {
                ComboTableForProcedureGenerate.SelectedIndex = i;

                break;
            }

            i++;
        }
    }

    private void UpdateViewCombo()
    {
        ComboView.Items.Clear();

        // ReSharper disable once CoVariantArrayConversion
        ComboView.Items.AddRange(OracleDb.ListViews().ToArray());
        ComboView.DisplayMember = "Value";
        ComboView.ValueMember = "Id";

        var i = 0;

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

    private void UpdateProcCombo()
    {
        ComboProcedureList.Items.Clear();

        // ReSharper disable CoVariantArrayConversion
        var ownerName = _Settings.DbConfig.Username.ToUpperInvariant();
        ComboProcedureList.Items.AddRange(OracleDb.ListPackages(ownerName).ToArray());
        ComboProcedureList.Items.AddRange(OracleDb.ListProcedures(ownerName).ToArray());

        // ReSharper restore CoVariantArrayConversion
        ComboProcedureList.DisplayMember = "Value";
        ComboProcedureList.ValueMember = "Id";

        var i = 0;

        foreach (ComboboxItem item in ComboProcedureList.Items)
        {
            if (item.Id == _Settings.Ui.ComboView)
            {
                ComboProcedureList.SelectedIndex = i;

                break;
            }

            i++;
        }
    }

    private void FormMain_Load(object sender, EventArgs e)
    {
        txtHostname.Text = _Settings.DbConfig.HostName;
        txtServiceName.Text = _Settings.DbConfig.ServiceName;
        txtPassword.Text = _Settings.DbConfig.Password;
        txtPort.Text = _Settings.DbConfig.Port.ToString();
        txtUsername.Text = _Settings.DbConfig.Username;

        if (!string.IsNullOrEmpty(txtHostname.Text) && !string.IsNullOrEmpty(txtServiceName.Text) &&
            !string.IsNullOrEmpty(txtPassword.Text) && !string.IsNullOrEmpty(txtPort.Text)        &&
            !string.IsNullOrEmpty(txtUsername.Text))
        {
            Connect();
        }

        tabMain.SelectedIndex = _Settings.Ui.TabIndex;
    }

    private void ActionException(Exception exp)
    {
        Log.Error(exp, exp.Message);

        MessageBox.Show(
            this,
            exp.Message,
            @"Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
    }

    private void ButtonGenerateView_Click(object sender, EventArgs e)
    {
        try
        {
            var sel = ComboView.SelectedItem;

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

            var className = selectedItem.ToUpperCamelCase(true);

            // Gen class
            txtClass.Text = Utils.GenerateClassData(className, list);
            var funcData = Utils.GenerateViewFunction(className, list, selectedItem);

            txtViewFunction.Text = funcData;
            _Settings.Ui.ComboView = selectedItem;
            _Settings.Save();
        }
        catch (Exception exp)
        {
            ActionException(exp);
        }
    }

    private void ButtonGenerateProcedure_Click(object sender, EventArgs e)
    {
        try
        {
            if (ComboProcedureList.SelectedItem is not ComboboxItem selectedItem || string.IsNullOrEmpty(selectedItem.Id))
            {
                return;
            }

            txtProcedure.Text = Utils.GenerateProcedure(
                _Settings.DbConfig.Username.ToUpperInvariant(),
                selectedItem.AdditionalData,
                selectedItem.ClearName,
                radioSeparate.Checked
            );

            _Settings.Ui.ComboView = selectedItem.Id;
            _Settings.Save();
        }
        catch (Exception exp)
        {
            ActionException(exp);
        }
    }

    private void ButtonGeneratePlSql_Click(object sender, EventArgs e)
    {
        try
        {
            var sel = ComboTableForProcedureGenerate.SelectedItem;

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

    private void TabMain_SelectedIndexChanged(object sender, EventArgs e)
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

    private void ComboProcedureList_SelectedIndexChanged(object sender, EventArgs e)
    {
        ButtonGenerateProcedure.PerformClick();
    }

    private void ComboTableForProcedureGenerate_SelectedValueChanged(object sender, EventArgs e)
    {
        ButtonGeneratePlSql.PerformClick();
    }

    private void radioSeparate_CheckedChanged(object sender, EventArgs e)
    {
        ButtonGenerateProcedure.PerformClick();
    }
}
