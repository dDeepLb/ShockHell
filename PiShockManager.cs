using System.IO;
using UnityEngine;

namespace ShockHell {
  static class PiShockManager {
    public static string Username { get; set; }
    public static string APIKey { get; set; }
    public static string Code { get; set; }
    private static readonly SimpleConfig Config = new SimpleConfig(Path.Combine(Application.persistentDataPath, "ShockHell.cfg"));


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
