using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ShockHell {
  class PiShockManager : MonoBehaviour {
    public static string Username { get; set; }
    public static string APIKey { get; set; }
    public static string Code { get; set; }
    private static readonly SimpleConfig Config = new SimpleConfig(Path.Combine(Application.persistentDataPath, "ShockHell.cfg"));
    private static readonly string ApiUrl = "https://do.pishock.com/api/apioperate/";
    private static PiShockManager _Instance;

    public static PiShockManager Instance {
      get {
        if (_Instance == null) {
          _Instance = FindObjectOfType<PiShockManager>();

          if (_Instance == null) {
            ModAPI.Log.Write("Instantiating PiShockManager");
            GameObject singletonObject = new GameObject("PiShockManager");
            singletonObject.SetActive(true);
            _Instance = singletonObject.AddComponent<PiShockManager>();
            DontDestroyOnLoad(singletonObject);
          }
        }
        return _Instance;
      }
    }

    private void Awake() {
      if (_Instance == null) {
#pragma warning disable S2696 // Instance members should not write to "static" fields
        _Instance = this;
#pragma warning restore S2696 // Instance members should not write to "static" fields
        DontDestroyOnLoad(gameObject);
      } else if (_Instance != this) {
        Destroy(gameObject);
      }
    }

    public void Shock(int intensity, int duration) {
      intensity = Mathf.Clamp(intensity, 1, 100);
      duration = Mathf.Clamp(duration, 1, 15);

      ModAPI.Log.Write($"Shock! for {duration} seconds at {intensity} power");

      if (_Instance != null) {
        ModAPI.Log.Write("Starting Shock coroutine.");
        StartCoroutine(SendPiShockRequest(PiShockOperations.Shock, intensity, duration));
      } else {
        ModAPI.Log.Write("PiShockManager Instance is null!");
      }
    }

    public void Vibrate(int intensity, int duration) {
      intensity = Mathf.Clamp(intensity, 1, 100);

      ModAPI.Log.Write($"Vibrating for {duration} seconds at {intensity} power!");

      StartCoroutine(SendPiShockRequest(PiShockOperations.Vibrate, intensity, duration));
    }

    public void Beep(int duration) {
      ModAPI.Log.Write($"Beep for {duration} seconds!");

      StartCoroutine(SendPiShockRequest(PiShockOperations.Beep, 0, duration));
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

    private static IEnumerator SendPiShockRequest(PiShockOperations operation, int intensity, int duration) {
      ModAPI.Log.Write("Sending PiShock Request");
      List<IMultipartFormSection> formData = new List<IMultipartFormSection> {
            new MultipartFormDataSection("Username", Username),
            new MultipartFormDataSection("Apikey", APIKey),
            new MultipartFormDataSection("Code", Code),
            new MultipartFormDataSection("Name", "ShockHell"),
            new MultipartFormDataSection("Op", operation.ToString()),
            new MultipartFormDataSection("Intensity", intensity.ToString()),
            new MultipartFormDataSection("Duration", duration.ToString()),
      };

      UnityWebRequest request = UnityWebRequest.Post(ApiUrl, formData);
      request.SetRequestHeader("Content-Type", "application/json");
      ModAPI.Log.Write(request.GetResponseHeaders());


      yield return request.SendWebRequest();

      if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
        ModAPI.Log.Write("Error: " + request.error);
      } else {
        ModAPI.Log.Write("Response: " + request.downloadHandler.text);
      }
    }
  }

  public enum PiShockOperations {
    Shock = 0,
    Vibrate = 1,
    Beep = 2
  }
}
