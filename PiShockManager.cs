using System.IO;
using UnityEngine;

namespace ShockHell {
  class PiShockManager {
    public string Username;
    private static string Username;
    private static string APIKey;
    private static string Code;

    public SimpleConfig Config = null;

    public PiShockManager() {
      ModAPI.Log.Write("Initing PiShock Manager");
      string configPath = Path.Combine(Application.persistentDataPath, "ShockHell.cfg");
      Config = new SimpleConfig(configPath);

      LoadAuthConfig();

      ModAPI.Log.Write($"Loaded PiSHock Auth at {configPath}");
    }

    public static void SaveAuthConfig() {
      ModAPI.Log.Write("Saving PiShock Auth");
      Config.SetValue("Auth", "Username", Username);
      Config.SetValue("Auth", "APIKey", APIKey);
      Config.SetValue("Auth", "Code", Code);
      Config.SaveConfig();
    }

    public static void LoadAuthConfig() {
      ModAPI.Log.Write("Loading PiShock Auth");
      Username = Config.GetValue("Auth", "Username", "");
      APIKey = Config.GetValue("Auth", "APIKey", "");
      Code = Config.GetValue("Auth", "Code", "");
    }
  }

  public enum PiShockOperations {
    Shock = 0,
    Vibrate = 1,
    Beep = 2
  }

  public class PiShockRequestModel {
    public string Username { get; set; }
    public string Apikey { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Intensity { get; set; }
    public int Duration { get; set; }
    public PiShockOperations Op { get; set; }
  }
}
