using System.Collections.Generic;
using System.IO;

namespace ShockHell {
  public class SimpleConfig {
    private Dictionary<string, Dictionary<string, string>> configData;
    private string filePath;

    public SimpleConfig(string filePath) {
      this.filePath = filePath;
      configData = new Dictionary<string, Dictionary<string, string>>();
      if (File.Exists(filePath)) {
        LoadConfig(filePath);
      } else {
        File.Create(filePath);
      }
    }

    private void LoadConfig(string filePath) {
      string[] lines = File.ReadAllLines(filePath);
      string currentSection = null;

      foreach (string line in lines) {
        string trimmedLine = line.Trim();

        if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#")) {
          continue;
        }

        if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]")) {
          currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2).Trim();
          if (!configData.ContainsKey(currentSection)) {
            configData[currentSection] = new Dictionary<string, string>();
          }
        } else if (currentSection != null && trimmedLine.Contains("=")) {
          string[] kvp = trimmedLine.Split(new[] { '=' }, 2);
          string key = kvp[0].Trim();
          string value = kvp[1].Trim();

          configData[currentSection][key] = value;
        }
      }
    }

    public void SaveConfig() {
      using (StreamWriter writer = new StreamWriter(filePath)) {
        foreach (var section in configData) {
          writer.WriteLine($"[{section.Key}]");
          foreach (var keyValuePair in section.Value) {
            writer.WriteLine($"{keyValuePair.Key}={keyValuePair.Value}");
          }
          writer.WriteLine();
        }
      }
    }

    public string GetValue(string section, string key, string defaultValue = null) {
      if (configData.ContainsKey(section) && configData[section].ContainsKey(key)) {
        return configData[section][key];
      }
      return defaultValue;
    }

    public void SetValue(string section, string key, string value) {
      if (!configData.ContainsKey(section)) {
        configData[section] = new Dictionary<string, string>();
      }
      configData[section][key] = value;
    }
  }
}