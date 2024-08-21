using System.Collections.Generic;
using DbHelperOracle.Properties.SettingsElements;
using SettingsHelper;

namespace DbHelperOracle.Properties;

public sealed class Settings : SettingsHolderBase {
  public DbConfigSettingsElement DbConfig { get; set; }
  public UiSettingsElement Ui { get; set; }

  public Settings() {
    DbConfig = new DbConfigSettingsElement();
    Ui = new UiSettingsElement();
  }

  public List<SettingsElementBase> ListSettingsElements() {
    return new() { DbConfig, Ui };
  }

  public override string ToString() {
    return $"{nameof(DbConfig)}: {DbConfig}, {nameof(Ui)}: {Ui}";
  }
}
