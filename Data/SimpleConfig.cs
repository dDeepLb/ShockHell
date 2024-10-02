using System.Collections.Generic;
using System.IO;

namespace ShockHell.Data
{
  /// <summary>
  /// Represents a simple configuration
  /// </summary>
  public class SimpleConfig
  {
    private Dictionary<string, Dictionary<string, string>> LocalConfigData { get; set; }
    private string LocalFilePath { get; set; }

    public SimpleConfig(string filePath)
    {
      if (string.IsNullOrWhiteSpace(filePath))
      {
        ModAPI.Log.Write($"Could not create instance of {nameof(SimpleConfig)} - ArgumentNull exception on ctor: {nameof(SimpleConfig)} {nameof(filePath)} was empty!");
        return;
      }
      LocalFilePath = filePath;
      LocalConfigData = new Dictionary<string, Dictionary<string, string>>();
      LoadOrCreateConfig(LocalFilePath);
    }

    public void LoadOrCreateConfig(string filePath)
    {
      try
      {
        LocalFilePath = filePath;
        if (string.IsNullOrWhiteSpace(LocalFilePath))
        {
          ModAPI.Log.Write($"Could not load config file: {nameof(SimpleConfig)} {nameof(LocalFilePath)}  was empty!");
          return;
        }
        if (!File.Exists(LocalFilePath))
        {
          File.Create(LocalFilePath);
          ModAPI.Log.Write($"Created config file {LocalFilePath}");          
        }
        if (LocalConfigData == null) 
        {
          LocalConfigData = new Dictionary<string, Dictionary<string, string>>();
        }
        string[] lines = File.ReadAllLines(LocalFilePath);
        foreach (string line in lines)
        {
          string currentSection = default;
          string trimmedLine = line.Trim();

          if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(';') || trimmedLine.StartsWith('#'))
          {
            continue;
          }

          if (trimmedLine.StartsWith('[') && trimmedLine.EndsWith(']'))
          {
            currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2).Trim();           
          }

          if (!string.IsNullOrWhiteSpace(currentSection) && !LocalConfigData.ContainsKey(currentSection))
          {
            LocalConfigData[currentSection] = new Dictionary<string, string>();
          }

          if (!string.IsNullOrWhiteSpace(currentSection) && trimmedLine.Contains("="))
          {
            string[] kvp = trimmedLine.Split(new[] { '=' }, 2);
            string key = kvp[0].Trim();
            string value = kvp[1].Trim();

            LocalConfigData[currentSection][key] = value;
          }

        }
      }
      catch (System.Exception exc)
      {
        ModAPI.Log.Write(exc);        
      }
    }

    public void SaveConfig()
    {
      try
      {
        if (string.IsNullOrWhiteSpace(LocalFilePath))
        {
          ModAPI.Log.Write($"Could not save config file: {nameof(SimpleConfig)} {nameof(LocalFilePath)}  was empty!");
          return;
        }
        using (StreamWriter writer = new StreamWriter(LocalFilePath))
        {
          foreach (var section in LocalConfigData)
          {
            writer.WriteLine($"[{section.Key}]");
            foreach (var keyValuePair in section.Value)
            {
              writer.WriteLine($"{keyValuePair.Key}={keyValuePair.Value}");
            }
            writer.WriteLine();
          }
        }
      }
      catch (System.Exception exc)
      {
        ModAPI.Log.Write(exc);
      }
    }

    public string GetValue(string section, string key, string defaultValue = null)
    {
      if (LocalConfigData.ContainsKey(section) && LocalConfigData[section].ContainsKey(key))
      {
        return LocalConfigData[section][key];
      }
      return defaultValue;
    }

    public void SetValue(string section, string key, string value)
    {
      if (!LocalConfigData.ContainsKey(section))
      {
        LocalConfigData[section] = new Dictionary<string, string>();
      }
      LocalConfigData[section][key] = value;
    }

  }
}