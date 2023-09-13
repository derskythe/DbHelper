using System.Collections.Generic;
using DbHelperMsSql.Properties.SettingsElements;
using SettingsHelper;

namespace DbHelperMsSql.Properties;


public class Settings : SettingsHolderBase
{
    public DbConfigSettingsElement DbConfig { get; set; }
    public UiSettingsElement Ui { get; set; }

    public Settings()
    {
        DbConfig = new DbConfigSettingsElement();
        Ui = new UiSettingsElement();
    }

    public virtual List<SettingsElementBase> ListSettingsElements()
    {
        return new()
        {
            DbConfig,
            Ui
        };
    }

    public override string ToString()
    {
        return $"{nameof(DbConfig)}: {DbConfig}, {nameof(Ui)}: {Ui}";
    }
}