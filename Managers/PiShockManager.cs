using ShockHell.Data;
using ShockHell.Data.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ShockHell.Managers
{
  public class PiShockManager : MonoBehaviour
  {
    public string Username { get; set; } = string.Empty;
    public string Apikey { get; set; } = string.Empty ;
    public string Code { get; set; } = string.Empty;
    public string ResponseText { get; private set; } = string.Empty;
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
      var operation = PiShockOperations.Shock;

      ModAPI.Log.Write($"Shock for {duration} seconds at {intensity} power");
      ModAPI.Log.Write("Starting Shock coroutine.");
      ModAPI.Log.Write("Sending PiShock Request");
      List<IMultipartFormSection> formData = new List<IMultipartFormSection> {
            new MultipartFormDataSection(nameof(Username), Username),
            new MultipartFormDataSection(nameof(Apikey), Apikey),
            new MultipartFormDataSection(nameof(Code), Code),
            new MultipartFormDataSection("Name", nameof(ShockHell)),
            new MultipartFormDataSection("Op", operation.ToString()),
            new MultipartFormDataSection("Intensity", intensity.ToString()),
            new MultipartFormDataSection("Duration", duration.ToString())
        };

      ModAPI.Log.Write($"POST Request form data" +
        $"\n{nameof(ApiUrl)}: {ApiUrl}" +
        $"\n{nameof(formData)}:" +
        $"\n\t{nameof(Username)}: {Username}" +
        $"\n\t{nameof(Apikey)}: {Apikey}" +
        $"\n\t{nameof(Code)}: {Code}" +
        $"\n\tName: {nameof(ShockHell)}" +
        $"\n\tOp: {operation}" +
        $"\n\tIntensity: {intensity}" +
        $"\n\tDuration: : {duration}");

      StartCoroutine(SendPiShockRequest(formData));
      ModAPI.Log.Write($"Response: {ResponseText}");
    }

    public void Vibrate(int intensity, int duration)
    {
      intensity = Mathf.Clamp(intensity, 1, 100);
      duration = Mathf.Clamp(duration, 1, 15);
      var operation = PiShockOperations.Vibrate;

      ModAPI.Log.Write($"Vibrating for {duration} seconds at {intensity} power!");
      ModAPI.Log.Write("Starting Vibrate coroutine.");
      ModAPI.Log.Write("Sending PiShock Request");
      List<IMultipartFormSection> formData = new List<IMultipartFormSection> {
            new MultipartFormDataSection(nameof(Username), Username),
            new MultipartFormDataSection(nameof(Apikey), Apikey),
            new MultipartFormDataSection(nameof(Code), Code),
            new MultipartFormDataSection("Name", nameof(ShockHell)),
            new MultipartFormDataSection("Op", operation.ToString()),
            new MultipartFormDataSection("Intensity", intensity.ToString()),
            new MultipartFormDataSection("Duration", duration.ToString())
      };

      ModAPI.Log.Write($"POST Request form data" +
        $"\n{nameof(ApiUrl)}: {ApiUrl}" +
        $"\n{nameof(formData)}:" +
        $"\n\t{nameof(Username)}: {Username}" +
        $"\n\t{nameof(Apikey)}: {Apikey}" +
        $"\n\t{nameof(Code)}: {Code}" +
        $"\n\tName: {nameof(ShockHell)}" +
        $"\n\tOp: {operation}" +
        $"\n\tIntensity: {intensity}" +
        $"\n\tDuration: : {duration}");

      StartCoroutine(SendPiShockRequest(formData));
      ModAPI.Log.Write($"Response: {ResponseText}");
    }

    public void Beep(int duration)
    {
      int intensity = 0;
      duration = Mathf.Clamp(duration, 1, 15);
      var operation = PiShockOperations.Beep;

      ModAPI.Log.Write($"Beep for {duration} seconds!");
      ModAPI.Log.Write("Starting Beep coroutine.");
      ModAPI.Log.Write("Sending PiShock Request");
      List<IMultipartFormSection> formData = new List<IMultipartFormSection> {
       new MultipartFormDataSection(nameof(Username), Username),
       new MultipartFormDataSection(nameof(Apikey), Apikey),
       new MultipartFormDataSection(nameof(Code), Code),
       new MultipartFormDataSection("Name", nameof(ShockHell)),
       new MultipartFormDataSection("Op", operation.ToString()),
       new MultipartFormDataSection("Intensity", intensity.ToString()),
       new MultipartFormDataSection("Duration", duration.ToString())
      };

      ModAPI.Log.Write($"POST Request form data" +
        $"\n{nameof(ApiUrl)}: {ApiUrl}" +
        $"\n{nameof(formData)}:" +
        $"\n\t{nameof(Username)}: {Username}" +
        $"\n\t{nameof(Apikey)}: {Apikey}" +
        $"\n\t{nameof(Code)}: {Code}" +
        $"\n\tName: {nameof(ShockHell)}" +
        $"\n\tOp: {operation}" +
        $"\n\tIntensity: {intensity}" +
        $"\n\tDuration: : {duration}");

      StartCoroutine(SendPiShockRequest(formData));
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

    private IEnumerator SendPiShockRequest(List<IMultipartFormSection> form)
    {
      UnityWebRequest uwr = UnityWebRequest.Post(ApiUrl, form);
      uwr.SetRequestHeader("Content-Type", "application/json");
      yield return uwr.SendWebRequest();

      if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
      {
        ResponseText = uwr.error;       
      }
      else
      {
        ResponseText = uwr.downloadHandler.text;        
      }
    }
  }
}
