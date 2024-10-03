﻿using ShockHell.Data.Enums;
using ShockHell.Managers;
using System;
using UnityEngine;
using static Mono.Security.X509.X520;

namespace ShockHell {
  /// <summary>
  /// Refactored into normal Monobehaviour object iso static internal class
  /// and also implemented singleton driven design pattern, like used in Unity
  /// eg to reference the player as Player.Get()
  /// </summary>
  public class ShockHellGui : MonoBehaviour {
    /// <summary>
    /// The main singleton instance.
    /// </summary>
    private static ShockHellGui Instance;
    private static readonly string ModName = nameof(ShockHellGui);
    public int GuiScreenId;
    public string GuiScreenTitle = $"{ModName} created by dDeepLb";
    private static float AuthBoxWidth { get; set; } = GuiScreenTotalWidth / 4f;
    private static float GuiScreenTotalWidth { get; set; } = 700f;
    private static float GuiScreenTotalHeight { get; set; } = 350f;
    private static float GuiScreenMinWidth { get; set; } = 700f;
    private static float GuiScreenMaxWidth { get; set; } = Screen.width;
    private static float GuiScreenMinHeight { get; set; } = 50f;
    private static float GuiScreenMaxHeight { get; set; } = Screen.height;
    private static float GuiScreenStartPositionX { get; set; } = Screen.width / 2f;
    private static float GuiScreenStartPositionY { get; set; } = Screen.height / 2f;
    private static bool IsGuiScreenMinimized { get; set; } = false;
    private Color DefaultGuiColor = GUI.color;
    public static bool ShowGui { get; set; } = false;

    public static Rect GuiScreen = new Rect(GuiScreenStartPositionX, GuiScreenStartPositionY, GuiScreenTotalWidth, GuiScreenTotalHeight);
    private static CursorManager LocalCursorManager;
    private static Player LocalPlayer;
    private static HUDManager LocalHUDManager;
    private static PiShockManager LocalPiShockManager;

    public ShockHellGui() {
      ///Flag to enable using GUILayout and GUI related functionality in Unity like OnGUI()
      useGUILayout = true;
      Instance = this;
    }

    /// <summary>
    /// Get the singleton reference to this <see cref="ShockHellGui"/> instance.
    /// </summary>
    /// <returns></returns>
    public static ShockHellGui Get() {
      return Instance;
    }

    protected virtual void Awake() {
      Instance = this;
    }

    protected virtual void OnDestroy() {
      Instance = null;
    }

    public void Start() {
      ///Initialize any locally used instance types in here, like CursorManager, Player...
      ///to assure their existance and availability
      InitData();
    }

    private void InitData() {
      LocalCursorManager = CursorManager.Get();
      LocalPlayer = Player.Get();
      LocalHUDManager = HUDManager.Get();
      LocalPiShockManager = PiShockManager.Get();
    }

    private void InitSkinUI() {
      GUI.skin = ModAPI.Interface.Skin;
    }

    public void DrawGUI() {
      InitData();
      InitSkinUI();
      GuiScreenId = GetHashCode();
      GuiScreen = GUILayout.Window(
                            GuiScreenId,
                            GuiScreen,
                            DrawWindow,
                            GuiScreenTitle,
                            GUI.skin.window,
                            GUILayout.ExpandWidth(true),
                            GUILayout.MinWidth(GuiScreenMinWidth),
                            GUILayout.MaxWidth(GuiScreenMaxWidth),
                            GUILayout.ExpandHeight(true),
                            GUILayout.MinHeight(GuiScreenMinHeight),
                            GUILayout.MaxHeight(GuiScreenMaxHeight));
    }

    public void DrawWindow(int windowID) {
      GuiScreenStartPositionX = GuiScreen.x;
      GuiScreenStartPositionY = GuiScreen.y;
      GuiScreenTotalWidth = GuiScreen.width;

      using (new GUILayout.VerticalScope(GUI.skin.box)) {
        GuiScreenMenuBox();
        if (!IsGuiScreenMinimized) {
          GuiManagerBox();
        }
      }
      GUI.DragWindow(new Rect(0f, 0f, 10000f, 10000f));
    }

    private void GuiScreenMenuBox() {
      string CollapseButtonText = IsGuiScreenMinimized ? "O" : "-";

      if (GUI.Button(new Rect(GuiScreen.width - 40f, 0f, 20f, 20f), CollapseButtonText, GUI.skin.button)) {
        CollapseGuiWindow();
      }

      if (GUI.Button(new Rect(GuiScreen.width - 20f, 0f, 20f, 20f), "X", GUI.skin.button)) {
        CloseWindow();
      }
    }

    private void CloseWindow() {
      ShowGui = false;
      EnableCursor(false);
    }

    private void CollapseGuiWindow() {
      if (!IsGuiScreenMinimized) {
        GuiScreen = new Rect(GuiScreen.x, GuiScreen.y, GuiScreenTotalWidth, GuiScreenMinHeight);
        IsGuiScreenMinimized = true;
      } else {
        GuiScreen = new Rect(GuiScreen.x, GuiScreen.y, GuiScreenTotalWidth, GuiScreenTotalHeight);
        IsGuiScreenMinimized = false;
      }
      DrawGUI();
    }

    private void GuiManagerBox() {
      using (new GUILayout.VerticalScope(GUI.skin.box)) {
        GUILayout.Label("PiShock API Connection", GUI.skin.label);
        using (new GUILayout.HorizontalScope(GUI.skin.box)) {
          GUILayout.Label(nameof(PiShockManager.Username), GUI.skin.label);
          PiShockManager.Username = GUILayout.TextField(PiShockManager.Username, GUI.skin.textField, GUILayout.Width(200f));
        }
        using (new GUILayout.HorizontalScope(GUI.skin.box)) {
          GUILayout.Label(nameof(PiShockManager.Apikey), GUI.skin.label);
          PiShockManager.Apikey = GUILayout.TextField(PiShockManager.Apikey, GUI.skin.textField, GUILayout.Width(200f));
        }
        using (new GUILayout.HorizontalScope(GUI.skin.box)) {
          GUILayout.Label(nameof(PiShockManager.Code), GUI.skin.label);
          PiShockManager.Code = GUILayout.TextField(PiShockManager.Code, GUI.skin.textField, GUILayout.Width(200f));
        }
        if (GUILayout.Button("Save", GUI.skin.button, GUILayout.MaxWidth(200f))) {
          LocalPiShockManager.SaveAuthConfig();
          ShowHUDBigInfo(HUDBigInfoMessage($"Configuration saved to {LocalPiShockManager.LocalSimpleConfig?.filePath}", MessageType.Info, DefaultGuiColor));
        }
      }
      using (new GUILayout.VerticalScope(GUI.skin.box)) {
        GUILayout.Label("PiShock Options", GUI.skin.label);
        if (GUILayout.Button("Vibrate", GUI.skin.button, GUILayout.MaxWidth(200f))) {
          LocalPiShockManager.Vibrate(50, 3);
          ShowHUDBigInfo(HUDBigInfoMessage(LocalPiShockManager.ResponseText, MessageType.Info, DefaultGuiColor));
        }
        if (GUILayout.Button("Weak shock", GUI.skin.button, GUILayout.MaxWidth(200f))) {
          LocalPiShockManager.Shock(10, 1);
          ShowHUDBigInfo(HUDBigInfoMessage(LocalPiShockManager.ResponseText, MessageType.Info, DefaultGuiColor));
        }
        if (GUILayout.Button("Beep", GUI.skin.button, GUILayout.MaxWidth(200f))) {
          LocalPiShockManager.Beep(1);
          ShowHUDBigInfo(HUDBigInfoMessage(LocalPiShockManager.ResponseText, MessageType.Info, DefaultGuiColor));
        }
      }
    }

    public static string HUDBigInfoMessage(string message, MessageType messageType, Color? headcolor = null)
      => $"<color=#{(headcolor != null ? ColorUtility.ToHtmlStringRGBA(headcolor.Value) : ColorUtility.ToHtmlStringRGBA(Color.red))}>{messageType}</color>\n{message}";

    public void ShowHUDBigInfo(string text) {
      string header = $"{ModName} Info";
      string textureName = HUDInfoLogTextureType.Count.ToString();

      HUDBigInfo bigInfo = (HUDBigInfo) LocalHUDManager.GetHUD(typeof(HUDBigInfo));
      HUDBigInfoData.s_Duration = 2f;
      HUDBigInfoData bigInfoData = new HUDBigInfoData {
        m_Header = header,
        m_Text = text,
        m_TextureName = textureName,
        m_ShowTime = Time.time
      };
      bigInfo.AddInfo(bigInfoData);
      bigInfo.Show(true);
    }

    public void EnableCursor(bool blockPlayer = false) {
      LocalCursorManager.ShowCursor(blockPlayer);

      if (blockPlayer) {
        LocalPlayer.BlockMoves();
        LocalPlayer.BlockRotation();
        LocalPlayer.BlockInspection();
      } else {
        LocalPlayer.UnblockMoves();
        LocalPlayer.UnblockRotation();
        LocalPlayer.UnblockInspection();
      }
    }

  }
}


