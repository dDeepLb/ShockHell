using System.IO;
using UnityEngine;

namespace ShockHell {
  class PiShockManager {
    private static PiShockManager instance;
    private static string Username;
    private static string APIKey;
    private static string Code;
    private static readonly SimpleConfig Config = new SimpleConfig(Path.Combine(Application.persistentDataPath, "ShockHell.cfg"));

    public static PiShockManager Instance {
      get {
        if (instance == null) {
          instance = new PiShockManager();
        }
        return instance;
      }
    }

    private PiShockManager() {
      ModAPI.Log.Write("Initing PiShock Manager");
      LoadAuthConfig();
    }

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
