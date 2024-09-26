using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace ShockHell {
  static class PiShockManager {
    public static string Username { get; set; }
    public static string APIKey { get; set; }
    public static string Code { get; set; }
    private static readonly SimpleConfig Config = new SimpleConfig(Path.Combine(Application.persistentDataPath, "ShockHell.cfg"));
    private static readonly string ApiUrl = "https://do.pishock.com/api/apioperate/";

    public static void Shock(int intensity, int duration) {
      intensity = Mathf.Clamp(intensity, 1, 100);
      duration = Mathf.Clamp(duration, 1, 15);

      ModAPI.Log.Write($"Shock! for {duration} seconds at {intensity} power");

      SendPiShockRequest(PiShockOperations.Shock, intensity, duration);
    }

    public static void Vibrate(int intensity, int duration) {
      SendPiShockRequest(PiShockOperations.Vibrate, intensity, duration);
    }

    public static void Beep(int duration) {
      SendPiShockRequest(PiShockOperations.Beep, 0, duration);
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

    private static string CreatePiShockPayload(PiShockOperations operation, int intensity, int duration) {
      PiShockRequestModel pishockRequest = new PiShockRequestModel() {
        Username = Username,
        Apikey = APIKey,
        Code = Code,
        Name = "ShockHell",
        Intensity = intensity,
        Duration = duration,
        Op = operation
      };
      return JsonConvert.SerializeObject(pishockRequest);
    }

    private static void SendPiShockRequest(PiShockOperations operation, int intensity, int duration) {
      string payload = CreatePiShockPayload(operation, intensity, duration);
      SimpleHttpClient.Post(ApiUrl, payload);
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
