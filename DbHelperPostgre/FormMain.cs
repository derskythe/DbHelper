using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ColorCode;
using DbHelperPostgre.Properties;
using DbHelperPostgre.Properties.SettingsElements;
using EnumsNET;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using NLog;
using Shared;

namespace DbHelperPostgre;


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
            var port = Convert.ToInt32(txtPort.Text);
            _DataAccess = new Db.Db(new DbConfigSettingsElement()
                {
                    HostName = txtHostname.Text,
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    Database = txtServiceName.Text,
                    Port = port
                }
            );

            if (await _DataAccess.CheckConnection())
            {
                _Settings.DbConfig.HostName = txtHostname.Text;
                _Settings.DbConfig.Database = txtServiceName.Text;
                _Settings.DbConfig.Password = txtPassword.Text;
                _Settings.DbConfig.Username = txtUsername.Text;
                _Settings.DbConfig.Port = port;
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

    private async void ButtonRefresh_Click(object sender, EventArgs e)
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

        var objectType = ObjectType.Table;
        foreach (var item in tables)
        {
            ComboView.Items.Add(new ComboboxItem
                {
                    Id = item,
                    Value = $"{item} ({objectType.AsString()})",
                    ObjectType = objectType
                }
            );
            ComboTablesForProcedureGeneration.Items.Add(new ComboboxItem
                {
                    Id = item,
                    Value = $"{item} ({objectType.AsString()})",
                    ObjectType = objectType
                }
            );
        }

        objectType = ObjectType.View;
        foreach (var item in views)
        {
            ComboView.Items.Add(new ComboboxItem
                {
                    Id = item,
                    Value = $"{item} ({objectType.AsString()})",
                    ObjectType = objectType
                }
            );
        }

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
                ObjectType = ObjectType.Procedure,
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
        txtPort.Text = _Settings.DbConfig.Port.ToString();

        if (!string.IsNullOrEmpty(txtHostname.Text) && !string.IsNullOrEmpty(txtServiceName.Text) &&
            !string.IsNullOrEmpty(txtPassword.Text) && !string.IsNullOrEmpty(txtUsername.Text)    && !string.IsNullOrEmpty(txtPort.Text))
        {
            await ConnectDb();
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

    private async void ButtonGenerateView_Click(object sender, EventArgs e)
    {
        try
        {
            if (ComboView.SelectedItem is not ComboboxItem sel)
            {
                return;
            }

            var selectedItem = sel.Id;
            var list = await _DataAccess.ListColumns(selectedItem, sel.ObjectType);
            if (list == null)
            {
                ActionException(new Exception("Column list is null!"));
                return;
            }

            var className = selectedItem.ToUpperCamelCase(true, checkCleanPlural.Checked);
            if (sel.ObjectType == ObjectType.View && sel.Value.StartsWith("v_", StringComparison.OrdinalIgnoreCase))
            {
                className = className[1..];
            }

            // Gen class
            txtClass.Text = FormatUsingRoslyn(Utils.GenerateClassData(className, list));
            txtViewFunction.Text = FormatUsingRoslyn(
                Utils.GenerateSelectTableOrViewMethod(className, list, selectedItem)
            );

            await InitializeAsync(webViewClass);
            await InitializeAsync(webViewViewFunction);

            var formatter = new HtmlFormatter();
            var htmlClass = formatter.GetHtmlString(txtClass.Text, Languages.CSharp);
            var htmlViewFunction = formatter.GetHtmlString(txtViewFunction.Text, Languages.CSharp);

            webViewClass.NavigateToString(htmlClass);
            webViewViewFunction.NavigateToString(htmlViewFunction);

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
            if (ComboProcedureList.SelectedItem is not ComboboxItem sel)
            {
                return;
            }

            var selectedName = sel.Id;
            if (string.IsNullOrEmpty(selectedName))
            {
                return;
            }

            var paramList = await _DataAccess.ListProcedureParameters(selectedName);
            txtProcedure.Text = FormatUsingRoslyn(Utils.GenerateProcedure(sel.ClearName,
                                                                          sel.AdditionalData,
                                                                          paramList,
                                                                          radioSeparate.Checked
                                                  )
            );
            var formatter = new HtmlFormatter();
            var html = formatter.GetHtmlString(txtProcedure.Text, Languages.CSharp);

            await InitializeAsync(webViewProcedure);
            webViewProcedure.NavigateToString(html);
            _Settings.Ui.ComboProcedureList = selectedName;
            _Settings.Save();
        }
        catch (Exception exp)
        {
            ActionException(exp);
        }
    }

    private async void ButtonGeneratePlSql_Click(object sender, EventArgs e)
    {
        try
        {
            if (ComboTablesForProcedureGeneration.SelectedItem is not ComboboxItem comboBoxItem)
            {
                return;
            }

            if (string.IsNullOrEmpty(comboBoxItem.Id))
            {
                return;
            }

            var selectedItem = comboBoxItem.Id;
            var list = await _DataAccess.ListColumns(selectedItem, comboBoxItem.ObjectType);
            if (list == null)
            {
                ActionException(new Exception("Column list is null!"));
                return;
            }

            txtPlSql.Text = Utils.GeneratePlSqlProcedure(selectedItem, list);

            var formatter = new HtmlFormatter();
            var html = formatter.GetHtmlString(txtPlSql.Text, Languages.Sql);

            await InitializeAsync(webViewPlSql);
            webViewPlSql.NavigateToString(html);
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

    private void ComboTablesForProcedureGeneration_SelectedIndexChanged(object sender, EventArgs e)
    {
        ButtonGeneratePlSql.PerformClick();
    }

    private void radioSeparate_CheckedChanged(object sender, EventArgs e)
    {
        ButtonGenerateProcedure.PerformClick();
    }

    private async Task InitializeAsync(Microsoft.Web.WebView2.WinForms.WebView2 component)
    {
        await component.EnsureCoreWebView2Async(null);
    }

    private string FormatUsingRoslyn(string originalCode)
    {
        using var workspace = new AdhocWorkspace();
        var syntaxTree = CSharpSyntaxTree.ParseText(originalCode);
        var formattedNode = Formatter.Format(syntaxTree.GetRoot(), workspace);
        return formattedNode.ToFullString();
    }
}