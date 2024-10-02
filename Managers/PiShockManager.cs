using ShockHell.Data;
using ShockHell.Data.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ShockHell.Managers
{
  public class PiShockManager : MonoBehaviour
  {
    public string Username { get; set; } = "dDeepLb";
    public string Apikey { get; set; } = string.Empty ;
    public string Code { get; set; } = string.Empty;
    public string ResponseText { get; private set; } = string.Empty;
    public byte[] ResponseData { get; private set; } = default;
    public SimpleConfig LocalSimpleConfig { get; private set; } = default;

    private static readonly string ApiUrl = $"https://do.pishock.com/api/apioperate/";    
  
    private static PiShockManager Instance;
    
    public PiShockManager()
    {
      LocalSimpleConfig = new SimpleConfig();
      Instance = this;
    }

    public static PiShockManager Get()
    {
      if (Instance == null)
      {
        Instance = FindObjectOfType<PiShockManager>();
        if (Instance == null)
        {
          ModAPI.Log.Write($"Instantiating {nameof(PiShockManager)}");
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
      Instance = this;
    }

    protected virtual void OnDestroy()
    {
      Instance = null;
    }

    protected virtual void Start()
    {
      /// Load any files, read configs etc
      /// and initialize any locally used instance types in here, like CursorManager, Player...
      ///to assure their existance and availability
      LoadAuthConfig();
    }

    private void InitData()
    {
      if (LocalSimpleConfig == null)
      {
        LocalSimpleConfig = new SimpleConfig();
        ModAPI.Log.Write($"{nameof(PiShockManager)}.{nameof(LocalSimpleConfig)} instantiated!");
      }      
    }

    public void Shock(int intensity, int duration)
    {
      intensity = Mathf.Clamp(intensity, 1, 100);
      duration = Mathf.Clamp(duration, 1, 15);
      var operation = (int)PiShockOperations.Shock;

      ModAPI.Log.Write($"Shock for {duration} seconds at {intensity} power");
      ModAPI.Log.Write("Starting Shock coroutine.");
      ModAPI.Log.Write("Sending PiShock Request");
      var json = $@"
      {{
        ""Username"": ""dDeepLb"",
        ""Apikey"": ""a8be78db-0bc6-425a-9f3a-289e3137cbb2"",
        ""Code"": ""2E86526CD40"",
        ""Name"": ""ShockHell"",
        ""Op"": ""{operation}"",
        ""Intensity"": ""{intensity}"",
        ""Duration"": ""{duration}""
      }}";

      ModAPI.Log.Write($"POST Request form data" +
      $"\n{nameof(ApiUrl)}: {ApiUrl}" +
      $"\n{nameof(json)}: {json}");

      StartCoroutine(SendPiShockRequest(json));
      ModAPI.Log.Write($"Response: {ResponseText}");
    }

    public void Vibrate(int intensity, int duration)
    {
      intensity = Mathf.Clamp(intensity, 1, 100);
      duration = Mathf.Clamp(duration, 1, 15);
      var operation = (int)PiShockOperations.Vibrate;

      ModAPI.Log.Write($"Vibrating for {duration} seconds at {intensity} power!");
      ModAPI.Log.Write("Starting Vibrate coroutine.");
      ModAPI.Log.Write("Sending PiShock Request");
      var json = $@"
      {{
        ""Username"": ""dDeepLb"",
        ""Apikey"": ""a8be78db-0bc6-425a-9f3a-289e3137cbb2"",
        ""Code"": ""2E86526CD40"",
        ""Name"": ""ShockHell"",
        ""Op"": ""{operation}"",
        ""Intensity"": ""{intensity}"",
        ""Duration"": ""{duration}""
      }}";

      ModAPI.Log.Write($"POST Request JSON" +
      $"\n{nameof(ApiUrl)}: {ApiUrl}" +
      $"\n{nameof(json)}: {json}");

      StartCoroutine(SendPiShockRequest(json));
      ModAPI.Log.Write($"Response: {ResponseText}");
    }

    public void Beep(int duration)
    {
      duration = Mathf.Clamp(duration, 1, 15);
      var operation = (int)PiShockOperations.Beep;

      ModAPI.Log.Write($"Beep for {duration} seconds!");
      ModAPI.Log.Write("Starting Beep coroutine.");
      ModAPI.Log.Write("Sending PiShock Request");
      var json = $@"
      {{
        ""Username"": ""dDeepLb"",
        ""Apikey"": ""a8be78db-0bc6-425a-9f3a-289e3137cbb2"",
        ""Code"": ""2E86526CD40"",
        ""Name"": ""ShockHell"",
        ""Op"": ""{operation}"",
        ""Duration"": ""{duration}""
      }}";

      ModAPI.Log.Write($"POST Request JSON" +
      $"\n{nameof(ApiUrl)}: {ApiUrl}" +
      $"\n{nameof(json)}: {json}");

      StartCoroutine(SendPiShockRequest(json));
      ModAPI.Log.Write($"Response: {ResponseText}");
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
      LocalSimpleConfig.LoadConfig();
      Username = LocalSimpleConfig.GetValue("Auth", nameof(Username), "");
      Apikey = LocalSimpleConfig.GetValue("Auth", nameof(Apikey), "");
      Code = LocalSimpleConfig.GetValue("Auth", nameof(Code), "");
    }

    private IEnumerator SendPiShockRequest(string jsonReq)
    {
      byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonReq);
      UnityWebRequest uwr = new UnityWebRequest(ApiUrl, UnityWebRequest.kHttpVerbPOST)
      {
        uploadHandler = new UploadHandlerRaw(bodyRaw),
        downloadHandler = new DownloadHandlerBuffer()
      };
      uwr.SetRequestHeader("Content-Type", "application/json");
      yield return uwr.SendWebRequest();

      if (uwr.result != UnityWebRequest.Result.Success)
      {
        ResponseText = uwr.error;        
        ResponseData = uwr.downloadHandler.data;
      }
      else
      {
        ResponseText = uwr.downloadHandler.text;
        ResponseData = uwr.downloadHandler.data;
      }
    }
  }
}
