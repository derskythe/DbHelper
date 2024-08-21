using System;
using System.Windows.Forms;
using DbHelperOracle.Properties;
using NLog;

namespace DbHelperOracle;


internal static class Program
{
    public static Settings Settings { get; private set; }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        var logger = LogManager.Setup()
                               .LoadConfigurationFromFile("NLog.config")
                               .GetCurrentClassLogger();

        try
        {
            logger.Info("Starting");

            var loadSettings = SettingsHelper.SettingsHelpers.Load<Settings>(
#if DEBUG
                false
#else
                    true
#endif
            );

            if (!loadSettings.Success)
            {
                throw new Exception(loadSettings.OutputMessage);
            }

            if (!string.IsNullOrWhiteSpace(loadSettings.OutputMessage))
            {
                logger.Info(loadSettings.OutputMessage);
            }

            Settings = loadSettings.Value;

#if DEBUG
            logger.Debug(Settings.ToString);
#endif

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
        catch (Exception exp)
        {
            logger.Error(exp, exp.Message);

            throw;
        }
        finally
        {
            logger.Info("Shutting down");
            LogManager.Shutdown();
        }
    }
}
