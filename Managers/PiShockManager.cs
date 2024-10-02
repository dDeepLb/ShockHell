using ShockHell.Data;
using ShockHell.Data.Enums;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ShockHell.Managers
{
  public class PiShockManager : MonoBehaviour
  {
    public string Username { get; set; }
    public string Apikey { get; set; }
    public string Code { get; set; }

    private static readonly string ApiUrl = $"https://do.pishock.com/api/apioperate/";
    private static readonly string LocalSimpleConfigPath = Path.Combine(Application.persistentDataPath, $"{nameof(ShockHell)}.cfg");
    
    private static PiShockManager Instance;
    private SimpleConfig LocalSimpleConfig;

    public PiShockManager()
    {
      Instance = this;
    }

    public static PiShockManager Get()
    {
      if (Instance == null)
      {
        Instance = FindObjectOfType<PiShockManager>();
        if (Instance == null)
        {
          ModAPI.Log.Write("Instantiating PiShockManager");
          var go = new GameObject(nameof(PiShockManager));          
          go.SetActive(true);
          Instance = go.AddComponent<PiShockManager>();
          DontDestroyOnLoad(go);
        }
      }
      return Instance;
    }

    protected virtual void Awake()
    {
      if (Instance == null)
      {
#pragma warning disable S2696 // Instance members should not write to "static" fields
        Instance = this;
#pragma warning restore S2696 // Instance members should not write to "static" fields
        DontDestroyOnLoad(gameObject);
      }
      else if (Instance != this)
      {
        Destroy(gameObject);
      }
    }

    protected virtual void OnDestroy()
    {
      Instance = null;
    }

    public void Start()
    {
      /// Load any files, read configs etc
      /// and initialize any locally used instance types in here, like CursorManager, Player...
      ///to assure their existance and availability
      InitData();
      LoadAuthConfig();
    }

    private void InitData()
    {
      if (LocalSimpleConfig == null)
      {
        LocalSimpleConfig = new SimpleConfig(LocalSimpleConfigPath);
        ModAPI.Log.Write($"{nameof(PiShockManager)}.{nameof(LocalSimpleConfig)} instantiated!");
      }      
    }

    public void Shock(int intensity, int duration)
    {
      intensity = Mathf.Clamp(intensity, 1, 100);
      duration = Mathf.Clamp(duration, 1, 15);

      ModAPI.Log.Write($"Shock! for {duration} seconds at {intensity} power");

      if (Instance != null)
      {
        ModAPI.Log.Write("Starting Shock coroutine.");
        StartCoroutine(SendPiShockRequest(PiShockOperations.Shock, intensity, duration));
      }
      else
      {
        ModAPI.Log.Write("PiShockManager Instance is null!");
      }
    }

    public void Vibrate(int intensity, int duration)
    {
      intensity = Mathf.Clamp(intensity, 1, 100);

      ModAPI.Log.Write($"Vibrating for {duration} seconds at {intensity} power!");

      StartCoroutine(SendPiShockRequest(PiShockOperations.Vibrate, intensity, duration));
    }

    public void Beep(int duration)
    {
      ModAPI.Log.Write($"Beep for {duration} seconds!");

      StartCoroutine(SendPiShockRequest(PiShockOperations.Beep, 0, duration));
    }

    public void SaveAuthConfig()
    {
      InitData();
      ModAPI.Log.Write("Saving PiShock Auth");
      LocalSimpleConfig.SetValue("Auth", nameof(Username), Username);
      LocalSimpleConfig.SetValue("Auth", nameof(Apikey), Apikey);
      LocalSimpleConfig.SetValue("Auth", nameof(Code), Code);
      LocalSimpleConfig.SaveConfig();
    }

    public void LoadAuthConfig()
    {
      InitData();
      ModAPI.Log.Write("Loading PiShock Auth");
      LocalSimpleConfig.LoadOrCreateConfig(LocalSimpleConfigPath);
      Username = LocalSimpleConfig.GetValue("Auth", nameof(Username), "");
      Apikey = LocalSimpleConfig.GetValue("Auth", nameof(Apikey), "");
      Code = LocalSimpleConfig.GetValue("Auth", nameof(Code), "");
    }

    private IEnumerator SendPiShockRequest(PiShockOperations operation, int intensity, int duration)
    {
      ModAPI.Log.Write("Sending PiShock Request");
      List<IMultipartFormSection> formData = new List<IMultipartFormSection> {
            new MultipartFormDataSection(nameof(Username), Username),
            new MultipartFormDataSection(nameof(Apikey), Apikey),
            new MultipartFormDataSection(nameof(Code), Code),
            new MultipartFormDataSection("Name", nameof(ShockHell)),
            new MultipartFormDataSection("Op", operation.ToString()),
            new MultipartFormDataSection("Intensity", intensity.ToString()),
            new MultipartFormDataSection("Duration", duration.ToString()),
      };

      ModAPI.Log.Write($"POST Request" +
        $"\n{nameof(ApiUrl)}: {ApiUrl}" +
        $"\n{nameof(formData)}:" +
        $"\n\t{nameof(Username)}: {Username}" +
        $"\n\t{nameof(Apikey)}: {Apikey}" +
        $"\n\t{nameof(Code)}: {Code}" +
        $"\n\tName: {nameof(ShockHell)}" +
        $"\n\tOp: {operation}" + 
        $"\n\tIntensity: {intensity}" +
        $"\n\tDuration: : {duration}");

      using (UnityWebRequest request = UnityWebRequest.Post(ApiUrl, formData))
      {
        request.SetRequestHeader("Content-Type", "application/json");
        ModAPI.Log.Write(request.GetResponseHeaders());

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
          ModAPI.Log.Write("Error: " + request.error);
        }
        else
        {
          ModAPI.Log.Write("Response: " + request.downloadHandler.text);
        }
      }
    }
  }
}
