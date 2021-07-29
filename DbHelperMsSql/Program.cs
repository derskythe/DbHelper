using System;
using System.Windows.Forms;
using DbHelper.Properties;

namespace DbHelper
{
    internal static class Program
    {
        public static Settings Settings { get; private set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var logger = NLog.LogManager.LoadConfiguration("NLog.config").GetCurrentClassLogger();
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

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
            catch (Exception exp)
            {
                logger.Error(exp, exp.Message);
                throw;
            }
        }
    }
}
