using SettingsHelper;

namespace DbHelperOracle.Properties.SettingsElements;


public sealed class UiSettingsElement : SettingsElementBase
{
    public int Height {
        get;
        set;
    }
    public int Width {
        get;
        set;
    }
    public int TabIndex {
        get;
        set;
    }
    public string ComboView {
        get;
        set;
    }
    public string ComboProcedureList {
        get;
        set;
    }

    public override string ToString()
    {
        return $"{nameof(Height)}: {Height}, {nameof(Width)}: {Width}, {nameof(TabIndex)}: {TabIndex}, {nameof(ComboView)}: {ComboView}, {nameof(ComboProcedureList)}: {ComboProcedureList}";
    }
}
