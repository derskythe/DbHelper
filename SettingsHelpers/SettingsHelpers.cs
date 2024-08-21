using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SettingsHelper.Models;

namespace SettingsHelper;

public static class SettingsHelpers {
  public static string SettingsFileProd { get; set; } = "appsettings.json";
  public static string SettingsFileDev { get; set; } =
      "appsettings.Development.json";
  private static string _CurrentAppPath;
  private static string _BackupAppPath;
  private static string _DefaultDrive = "C:\\";

  // ReSharper disable once UnusedMember.Global
  public static string DefaultDrive {
    set {
      if (string.IsNullOrWhiteSpace(value) || !Directory.Exists(value)) {
        return;
      }

      _DefaultDrive = value;

      if (string.IsNullOrWhiteSpace(_BackupAppPath)) {
        _BackupAppPath = BackupAppPath;
      }
    }
    get => _DefaultDrive;
  }

  public static string BackupAppPath {
    get {
      if (string.IsNullOrWhiteSpace(_BackupAppPath)) {
        var assembly = Assembly.GetEntryAssembly();

        _BackupAppPath = Path.Combine(
            _DefaultDrive, assembly != null
                               ? assembly.GetName().Name
                               : Assembly.GetCallingAssembly().GetName().Name);
      }

      if (!Directory.Exists(_BackupAppPath)) {
        Directory.CreateDirectory(_BackupAppPath);
      }

      return _BackupAppPath;
    }
    set {
      if (string.IsNullOrWhiteSpace(value) || !Directory.Exists(value)) {
        return;
      }

      var parent = Directory.GetParent(value);

      if (parent == null) {
        throw new IOException($"Can't obtain parent of {value}");
      }

      _DefaultDrive = parent.FullName;
      _BackupAppPath = value;
    }
  }

  // ReSharper disable once UnusedMember.Global
  public static void SetBackupAppPath(string path) {
    if (string.IsNullOrWhiteSpace(path)) {
      return;
    }

    if (!Directory.Exists(path)) {
      Directory.CreateDirectory(path);
    }

    _BackupAppPath = path;
  }

  public static string CurrentAppPath {
    get {
      if (!string.IsNullOrEmpty(_CurrentAppPath)) {
        return _CurrentAppPath;
      }

      _CurrentAppPath =
          Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

      if (!string.IsNullOrEmpty(_CurrentAppPath)) {
        return _CurrentAppPath;
      }

      _CurrentAppPath = Path.GetDirectoryName(Directory.GetCurrentDirectory());

      if (string.IsNullOrEmpty(_CurrentAppPath)) {
        throw new Exception("Can't find path of current application!");
      }

      return _CurrentAppPath;
    }
  }

  public static (bool Success, string OutputMessage)
      Save(this SettingsHolderBase value, string appData = "") {
    string outputMessage;

    try {
      return Save(JToken.FromObject(value), appData, null);
    } catch (Exception exp) {
      outputMessage = $"Error writing app settings {exp.Message}";
    }

    return (false, outputMessage);
  }

  private static (bool Success, string OutputMessage)
      Save(JToken value, string appData, bool? isProduction) {
    var success = false;
    var outputMessage = string.Empty;

    try {
      if (string.IsNullOrWhiteSpace(appData)) {
        appData = BackupAppPath;
      }

      var jsonObj = JsonConvert.SerializeObject(value, Formatting.Indented);

      if (!isProduction.HasValue) {
        File.WriteAllText(Path.Combine(appData, SettingsFileProd), jsonObj);
        File.WriteAllText(Path.Combine(appData, SettingsFileDev), jsonObj);
      } else if (isProduction.Value) {
        File.WriteAllText(Path.Combine(appData, SettingsFileProd), jsonObj);
      } else {
        File.WriteAllText(Path.Combine(appData, SettingsFileDev), jsonObj);
      }

      success = true;
    } catch (Exception exp) {
      outputMessage = $"Error writing app settings {exp.Message}";
    }

    return (success, outputMessage);
  }

  public static (bool Success, T Value,
                 string OutputMessage) Load<T>(bool isProduction)
      where T : SettingsHolderBase, new() {
    var outputMessage = new StringBuilder();

    var file = isProduction ? SettingsFileProd : SettingsFileDev;
    var workingSettingsPath = Path.Combine(BackupAppPath, file);
    var scratchSettingsPath = Path.Combine(CurrentAppPath, file);

    outputMessage.Append("Using: ").AppendLine(workingSettingsPath);

    if (!File.Exists(workingSettingsPath)) {
      try {
        File.Copy(scratchSettingsPath, workingSettingsPath);
      } catch (Exception exp) {
        return (false, null, exp.Message);
      }
    } else {
      var checkSettingsForChanges = CheckSettingsForChanges(
          workingSettingsPath, scratchSettingsPath, isProduction);

      if (!string.IsNullOrEmpty(checkSettingsForChanges)) {
        outputMessage.AppendLine(checkSettingsForChanges);
      }
    }

    try {
      var builder =
          new ConfigurationBuilder()
              .SetBasePath(BackupAppPath)
              .AddJsonFile(file, optional: true, reloadOnChange: true);

      var configuration = builder.Build();

      // var configurationResult = configuration.GetSection("unknown");
      // configurationResult.Bind(settings);

      var settings = configuration.Get<T>();

      return (true, settings, settings.ToString());
    } catch (Exception exp) {
      outputMessage.AppendLine(exp.Message);

      return (false, null, outputMessage.ToString());
    }
  }

  private static string CheckSettingsForChanges(string workingSettingsPath,
                                                string scratchSettingsPath,
                                                bool isProduction) {
    var outputMessage = new StringBuilder();

    try {
      var scratchJson = JObject.Parse(File.ReadAllText(scratchSettingsPath));
      var workingJson = JObject.Parse(File.ReadAllText(workingSettingsPath));
      var (isEqual, differences) =
          scratchJson.CompareJson(workingJson, CheckForced(scratchJson));

      if (isEqual) {
        return outputMessage.ToString();
      }

      var applyChanges = false;

      foreach (var item in differences) {
#if DEBUG
        if (item.Type != DifferenceType.Missing) {
          outputMessage.Append("Found diff: ").AppendLine(item.ToString());
        }
#endif
        if (item.Type == DifferenceType.ValueChanged) {
          continue;
        }

        if (item.Type == DifferenceType.ForcedChange) {
          applyChanges = true;
          var node = scratchJson.SelectToken(item.Path);
          workingJson.ReplaceNested(item.Path, node);

          outputMessage.AppendLine("Force change node: ")
              .Append(item.Path)
              .Append(", JSON: ")
              .AppendLine(node?.ToString(Formatting.None));
        } else if (item.Side == MissedSide.Source) {
          outputMessage.Append("Scratch file missing path: ")
              .Append(item.Path)
              .Append(", JSON: ")
              .AppendLine(scratchJson.SelectToken(item.Path)?.ToString(
                  Formatting.None));
        } else {
          applyChanges = true;
          var node = scratchJson.SelectToken(item.Path);
          workingJson.ReplaceNested(item.Path, node);

          outputMessage.AppendLine("Added new node: ")
              .Append(item.Path)
              .Append(", JSON: ")
              .AppendLine(node?.ToString(Formatting.None));
        }
      }

      if (applyChanges) {
        Save(workingJson, string.Empty, isProduction);
      }
    } catch (Exception exp) {
      outputMessage.AppendLine(exp.Message);
    }

    return outputMessage.ToString();
  }

  private static List<string> CheckForced(JObject jObject) {
    var result = new List<string>();

    using (var nodes = jObject.GetEnumerator()) {
      while (nodes.MoveNext()) {
        if (!nodes.Current.Key.IsEqual("_forced") ||
            nodes.Current.Value?.Type != JTokenType.Array ||
            !(nodes.Current.Value is JArray array)) {
          continue;
        }

        foreach (var item in array) {
          result.Add(item.Value<string>());
        }
      }
    }

    return result;
  }

  // ReSharper disable once UnusedMember.Global
  public static (bool success, string errMsg)
      AddOrUpdate(this object value, string path) {
    var success = false;
    var errMsg = string.Empty;

    try {
      var jsonObj = JsonConvert.SerializeObject(value, Formatting.Indented);
      File.WriteAllText(path, jsonObj);
      success = true;
    } catch (Exception ex) {
      errMsg = $"Error writing app settings {ex.Message}";
    }

    return (success, errMsg);
  }

  public static bool IsEqual(this string value, string anotherValue) {
    if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(anotherValue)) {
      return false;
    }

    return value.Equals(anotherValue,
                        StringComparison.InvariantCultureIgnoreCase);
  }
}
