using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ShockHell.Data {
  /// <summary>
  /// Represents a simple configuration
  /// </summary>
  public class SimpleConfig {
    public Dictionary<string, Dictionary<string, string>> LocalConfigData { get; private set; }
    public string LocalFilePath { get; private set; }

    public SimpleConfig() {
      LocalFilePath = Path.Combine(Application.dataPath.Replace("GH_Data", "Mods"), $"{nameof(ShockHell)}.cfg");
      ModAPI.Log.Write($"{nameof(LocalFilePath)}: {LocalFilePath}");
      LocalConfigData = new Dictionary<string, Dictionary<string, string>>();
      ModAPI.Log.Write($"{nameof(LocalConfigData)} initialized!");
      LoadConfig();
      ModAPI.Log.Write($"{nameof(SimpleConfig)} instance ready");
    }

    public void LoadConfig() {
      if (!File.Exists(LocalFilePath)) {
        using (var temp = File.Create(LocalFilePath)) {
          ModAPI.Log.Write($"Created config file {LocalFilePath}");
        }
      }
      LoadConfigurationFile();
    }

    private void LoadConfigurationFile() {
      foreach (string line in File.ReadAllLines(LocalFilePath)) {
        string currentSection = string.Empty;
        string trimmedLine = line.Trim();

        if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(';') || trimmedLine.StartsWith('#')) {
          continue;
        }

        if (trimmedLine.StartsWith('[') && trimmedLine.EndsWith(']')) {
          currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2).Trim();
        }

        if (!string.IsNullOrWhiteSpace(currentSection) && !LocalConfigData.ContainsKey(currentSection)) {
          LocalConfigData[currentSection] = new Dictionary<string, string>();
        }

        if (!string.IsNullOrWhiteSpace(currentSection) && trimmedLine.Contains("=")) {
          string[] kvp = trimmedLine.Split(new[] { '=' }, 2);
          string key = kvp[0].Trim();
          string value = kvp[1].Trim();

          LocalConfigData[currentSection][key] = value;
        }

      }
      ModAPI.Log.Write($"Finished loading config file as {nameof(LocalConfigData)}");
    }

    public void SaveConfig() {
      using (StreamWriter writer = new StreamWriter(LocalFilePath)) {
        foreach (var section in LocalConfigData) {
          writer.WriteLine($"[{section.Key}]");
          ModAPI.Log.Write($"[{section.Key}]");

          foreach (var keyValuePair in section.Value) {
            writer.WriteLine($"{keyValuePair.Key}={keyValuePair.Value}");
            ModAPI.Log.Write($"\t{keyValuePair.Key}={keyValuePair.Value}");
          }
          writer.WriteLine();
        }
      }
    }

    public string GetValue(string section, string key, string defaultValue = null) {
      if (LocalConfigData.ContainsKey(section) && LocalConfigData[section].ContainsKey(key)) {
        return LocalConfigData[section][key];
      }
      return defaultValue;
    }

    public void SetValue(string section, string key, string value) {
      if (!LocalConfigData.ContainsKey(section)) {
        LocalConfigData[section] = new Dictionary<string, string>();
      }
      LocalConfigData[section][key] = value;
    }

  }
}