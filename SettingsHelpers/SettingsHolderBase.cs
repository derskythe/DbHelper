namespace SettingsHelper;


public abstract class SettingsHolderBase
{
    public (bool Success, string OutputMessage) Save()
    {
        return SettingsHelpers.Save(this);
    }
}
