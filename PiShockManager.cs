using System.IO;
using UnityEngine;

﻿namespace ShockHell {
  class PiShockManager {
    public string Username;
    public string APIKey;
    public string Code;

    public SimpleConfig Config = null;

    public PiShockManager() {
      ModAPI.Log.Write("Initing PiShock Manager");
      string configPath = Path.Combine(Application.persistentDataPath, "ShockHell.cfg");
      Config = new SimpleConfig(configPath);

      LoadAuthConfig();

      ModAPI.Log.Write($"Loaded PiSHock Auth at {configPath}");
    }

    public void SaveAuthConfig() {
      ModAPI.Log.Write("Saving PiShock Auth");
      Config.SetValue("Auth", "Username", Username);
      Config.SetValue("Auth", "APIKey", APIKey);
      Config.SetValue("Auth", "Code", Code);
      Config.SaveConfig();
    }

    public void LoadAuthConfig() {
      ModAPI.Log.Write("Loading PiShock Auth");
      Username = Config.GetValue("Auth", "Username", "");
      APIKey = Config.GetValue("Auth", "APIKey", "");
      Code = Config.GetValue("Auth", "Code", "");
    }
  }
}
