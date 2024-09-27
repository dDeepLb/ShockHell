using System.IO;
using UnityEngine;
using UnityEngine.Networking;

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

    private static void SendPiShockRequest(PiShockOperations operation, int intensity, int duration) {
      WWWForm payload = new WWWForm();
      payload.AddField("Username", Username);
      payload.AddField("Apikey", APIKey);
      payload.AddField("Code", Code);
      payload.AddField("Name", "ShockHell");
      payload.AddField("Op", (int) operation);
      if (intensity > 0) payload.AddField("Intensity", intensity);
      payload.AddField("duration", duration);

      UnityWebRequest request = UnityWebRequest.Post(ApiUrl, payload);
      request.SendWebRequest();

      ModAPI.Log.Write(request.result);
    }
  }

  public enum PiShockOperations {
    Shock = 0,
    Vibrate = 1,
    Beep = 2
  }
}
