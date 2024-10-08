﻿using ShockHell.Data.Enums;
using ShockHell.Managers;
using UnityEngine;

namespace ShockHell {
  /// <summary>
  /// Refactored into normal Monobehaviour object iso static internal class
  /// and also implemented singleton driven design pattern, like used in Unity
  /// eg to reference the player as Player.Get()
  /// </summary>
  public class ShockHellGui : MonoBehaviour {
    public static bool ShowGui { get; set; } = false;

    private static ShockHellGui Instance;
    private static readonly string ModName = nameof(ShockHellGui);
    private static int GuiScreenId;
    private static readonly string GuiScreenTitle = $"{ModName} created by dDeepLb";
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
    private static Color DefaultGuiColor = GUI.color;
    private static Rect GuiScreen = new Rect(GuiScreenStartPositionX, GuiScreenStartPositionY, GuiScreenTotalWidth, GuiScreenTotalHeight);
    private static CursorManager LocalCursorManager;
    private static Player LocalPlayer;
    private static HUDManager LocalHUDManager;
    private static PiShockManager LocalPiShockManager;
    private static readonly string PiShockMainPageLink = "https://pishock.com";

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
      GuiScreenId = GetHashCode();
    }

    private static void InitData() {
      LocalCursorManager = CursorManager.Get();
      LocalPlayer = Player.Get();
      LocalHUDManager = HUDManager.Get();
      LocalPiShockManager = PiShockManager.Get();
    }

    private static void InitSkinUI() {
      GUI.skin = ModAPI.Interface.Skin;
    }

    public static void DrawGUI() {
      InitData();
      InitSkinUI();
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
          GUILayout.MaxHeight(GuiScreenMaxHeight)
      );
    }

    public static void DrawWindow(int windowID) {
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

    private static void GuiScreenMenuBox() {
      string CollapseButtonText = IsGuiScreenMinimized ? "O" : "-";

      if (GUI.Button(new Rect(GuiScreen.width - 40f, 0f, 20f, 20f), CollapseButtonText, GUI.skin.button)) {
        CollapseGuiWindow();
      }

      if (GUI.Button(new Rect(GuiScreen.width - 20f, 0f, 20f, 20f), "X", GUI.skin.button)) {
        CloseWindow();
      }
    }

    private static void CloseWindow() {
      ShowGui = false;
      EnableCursor(false);
    }

    private static void CollapseGuiWindow() {
      if (!IsGuiScreenMinimized) {
        GuiScreen = new Rect(GuiScreen.x, GuiScreen.y, GuiScreenTotalWidth, GuiScreenMinHeight);
        IsGuiScreenMinimized = true;
      } else {
        GuiScreen = new Rect(GuiScreen.x, GuiScreen.y, GuiScreenTotalWidth, GuiScreenTotalHeight);
        IsGuiScreenMinimized = false;
      }
      DrawGUI();
    }

    private static void GuiManagerBox() {
      using (new GUILayout.HorizontalScope()) {
        GUILayout.Space(AuthBoxWidth / 2);
        using (new GUILayout.VerticalScope(GUI.skin.box, GUILayout.Width(AuthBoxWidth))) {
          GUILayout.Label("PiShock API Connection", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
          if (GUILayout.Button("Open PiShock page in browser")) {
            Application.OpenURL(PiShockMainPageLink);
          }
          GUILayout.Label("On that page, on top, click 'API' -> 'API DOC' -> 'Authenticating'.\nIt has instructions where to find the values.");

          GUILayout.Space(GuiScreen.height * 0.02f);

          using (new GUILayout.HorizontalScope()) {
            using (new GUILayout.VerticalScope()) {
              GUILayout.Label("PiShock Username");
              GUILayout.Label("PiShock API Key");
              GUILayout.Label("PiShock Code");
            }

            using (new GUILayout.VerticalScope()) {
              PiShockManager.Username = GUILayout.TextField(PiShockManager.Username, 32);
              PiShockManager.Apikey = GUILayout.TextField(PiShockManager.Apikey, 36);
              PiShockManager.Code = GUILayout.TextField(PiShockManager.Code, 32);
            }
          }

          GUILayout.Space(GuiScreen.height * 0.02f);

          if (GUILayout.Button("Save configuration")) {
            LocalPiShockManager.SaveAuthConfig();
            ShowHUDBigInfo(HUDBigInfoMessage($"Configuration saved to {LocalPiShockManager.LocalSimpleConfig?.filePath}", MessageType.Info, ShockHellGui.DefaultGuiColor));
          }

          GUILayout.Space(GuiScreen.height * 0.01f);

          if (GUILayout.Button("Test with weak shock")) {
            LocalPiShockManager.Shock(10, 1);
            ShowHUDBigInfo(HUDBigInfoMessage(LocalPiShockManager.ResponseText, MessageType.Info, DefaultGuiColor));
          }

          if (GUILayout.Button("Test with vibration")) {
            LocalPiShockManager.Vibrate(50, 3);
            ShowHUDBigInfo(HUDBigInfoMessage(LocalPiShockManager.ResponseText, MessageType.Info, DefaultGuiColor));
          }

          if (GUILayout.Button("Test with beep")) {
            LocalPiShockManager.Beep(1);
            ShowHUDBigInfo(HUDBigInfoMessage(LocalPiShockManager.ResponseText, MessageType.Info, DefaultGuiColor));
          }
        }
      }
    }

    public static string HUDBigInfoMessage(string message, MessageType messageType, Color? headcolor = null)
      => $"<color=#{(headcolor != null ? ColorUtility.ToHtmlStringRGBA(headcolor.Value) : ColorUtility.ToHtmlStringRGBA(Color.red))}>{messageType}</color>\n{message}";

    public static void ShowHUDBigInfo(string text) {
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

    public static void EnableCursor(bool blockPlayer = false) {
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


