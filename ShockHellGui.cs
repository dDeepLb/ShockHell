using ShockHell.Managers;
using System;
using UnityEngine;

namespace ShockHell
{
  /// <summary>
  /// Refactored into normal Monobehaviour object iso static internal class
  /// and also implemented singleton driven design pattern, like used in Unity
  /// eg to reference the player as Player.Get()
  /// </summary>
  public class ShockHellGui : MonoBehaviour
  {
    /// <summary>
    /// The main singleton instance.
    /// </summary>
    private static ShockHellGui Instance;
    private static readonly string ModName = nameof(ShockHellGui);
    public int ShockHellGuiScreenId;
    public string ShockHellGuiScreenTitle = $"{ModName} created by dDeepLb";
    private static float AuthBoxWidth { get; set; } = ShockHellGuiScreenTotalWidth / 4f;
    private static float ShockHellGuiScreenTotalWidth { get; set; } = 700f;
    private static float ShockHellGuiScreenTotalHeight { get; set; } = 350f;
    private static float ShockHellGuiScreenMinWidth { get; set; } = 700f;
    private static float ShockHellGuiScreenMaxWidth { get; set; } = Screen.width;
    private static float ShockHellGuiScreenMinHeight { get; set; } = 50f;
    private static float ShockHellGuiScreenMaxHeight { get; set; } = Screen.height;
    private static float ShockHellGuiScreenStartPositionX { get; set; } = Screen.width / 2f;
    private static float ShockHellGuiScreenStartPositionY { get; set; } = Screen.height / 2f;
    private static bool IsShockHellGuiScreenMinimized { get; set; } = false;
    private Color DefaultGuiColor = GUI.color;
    private bool ShowShockHellGui { get; set; } = false;    

    public static Rect ShockHellGuiScreen = new Rect(ShockHellGuiScreenStartPositionX, ShockHellGuiScreenStartPositionY, ShockHellGuiScreenTotalWidth, ShockHellGuiScreenTotalHeight);
    private static CursorManager LocalCursorManager;
    private static Player LocalPlayer;
    private static PiShockManager LocalPiShockManager;

    public ShockHellGui()
    {
      ///Flag to enable using GUILayout and GUI related functionality in Unity like OnGUI()
      useGUILayout = true;
      Instance = this;
    }

    /// <summary>
    /// Get the singleton reference to this <see cref="ShockHellGui"/> instance.
    /// </summary>
    /// <returns></returns>
    public static ShockHellGui Get()
    {
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

    public void Start()
    {
      ///Initialize any locally used instance types in here, like CursorManager, Player...
      ///to assure their existance and availability
      InitData();
    }

    private void InitData()
    {
      LocalCursorManager = CursorManager.Get();
      LocalPlayer = Player.Get();
      LocalPiShockManager = PiShockManager.Get();
    }

    private void InitSkinUI()
    {
      GUI.skin = ModAPI.Interface.Skin;
    }

    public void DrawGUI()
    {
      InitData();
      InitSkinUI();
      ShockHellGuiScreenId = GetHashCode();
      ShockHellGuiScreen = GUILayout.Window(
                            ShockHellGuiScreenId,
                            ShockHellGuiScreen,
                            DrawWindow,
                            ShockHellGuiScreenTitle,
                            GUI.skin.window,
                            GUILayout.ExpandWidth(true),
                            GUILayout.MinWidth(ShockHellGuiScreenMinWidth),
                            GUILayout.MaxWidth(ShockHellGuiScreenMaxWidth),
                            GUILayout.ExpandHeight(true),
                            GUILayout.MinHeight(ShockHellGuiScreenMinHeight),
                            GUILayout.MaxHeight(ShockHellGuiScreenMaxHeight));
    }

    public void DrawWindow(int windowID)
    {
      ShockHellGuiScreenStartPositionX = ShockHellGuiScreen.x;
      ShockHellGuiScreenStartPositionY = ShockHellGuiScreen.y;
      ShockHellGuiScreenTotalWidth = ShockHellGuiScreen.width;

      using (new GUILayout.VerticalScope(GUI.skin.box))
      {
        ShockHellGuiScreenMenuBox();
        if (!IsShockHellGuiScreenMinimized)
        {
          ShockHellGuiManagerBox();
        }
      }
      GUI.DragWindow(new Rect(0f, 0f, 10000f, 10000f));     
    }

    private void ShockHellGuiScreenMenuBox()
    {
      string CollapseButtonText = IsShockHellGuiScreenMinimized ? "O" : "-";

      if (GUI.Button(new Rect(ShockHellGuiScreen.width - 40f, 0f, 20f, 20f), CollapseButtonText, GUI.skin.button))
      {
        CollapseShockHellGuiWindow();
      }

      if (GUI.Button(new Rect(ShockHellGuiScreen.width - 20f, 0f, 20f, 20f), "X", GUI.skin.button))
      {
        CloseWindow();
      }
    }

    private void CloseWindow()
    {
      ShowShockHellGui = false;
      EnableCursor(false);
    }

    private void CollapseShockHellGuiWindow()
    {
      if (!IsShockHellGuiScreenMinimized)
      {
        ShockHellGuiScreen = new Rect(ShockHellGuiScreen.x, ShockHellGuiScreen.y, ShockHellGuiScreenTotalWidth, ShockHellGuiScreenMinHeight);
        IsShockHellGuiScreenMinimized = true;
      }
      else
      {
        ShockHellGuiScreen = new Rect(ShockHellGuiScreen.x, ShockHellGuiScreen.y, ShockHellGuiScreenTotalWidth, ShockHellGuiScreenTotalHeight);
        IsShockHellGuiScreenMinimized = false;
      }
      DrawGUI();
    }

    private void ShockHellGuiManagerBox()
    {
      using (new GUILayout.VerticalScope("API Connection", GUI.skin.box))
      {
        GUILayout.Space(ShockHellGuiScreen.height * 0.02f);
        using (new GUILayout.HorizontalScope())
        {
          GUILayout.Space(ShockHellGuiScreen.width / 2f - AuthBoxWidth / 2f);
          using (new GUILayout.VerticalScope(GUI.skin.box, GUILayout.Width(AuthBoxWidth)))
          {
            using (new GUILayout.HorizontalScope())
            {
              using (new GUILayout.VerticalScope())
              {
                GUILayout.Label("PiShock Username");
                GUILayout.Label("PiShock API Key");
                GUILayout.Label("PiShock Code");
              }

              using (new GUILayout.VerticalScope())
              {
                LocalPiShockManager.Username = GUILayout.TextField(LocalPiShockManager.Username, 32, GUI.skin.textField);
                LocalPiShockManager.Apikey = GUILayout.TextField(LocalPiShockManager.Apikey, 36, GUI.skin.textField);
                LocalPiShockManager.Code = GUILayout.TextField(LocalPiShockManager.Code, 32, GUI.skin.textField);
              }
            }

            GUILayout.Space(ShockHellGuiScreen.height * 0.02f);

            if (GUILayout.Button("Submit"))
            {
              LocalPiShockManager.SaveAuthConfig();
            }

            GUILayout.Space(ShockHellGuiScreen.height * 0.01f);

            if (GUILayout.Button("Test with vibration"))
            {
              LocalPiShockManager.Vibrate(50, 3);
            }

            if (GUILayout.Button("Test with weak shock"))
            {
              LocalPiShockManager.Shock(10, 1);
            }

            if (GUILayout.Button("Test with beep"))
            {
              LocalPiShockManager.Beep(1);
            }
          }
        }
      }
    }

    public void EnableCursor(bool blockPlayer = false)
    {
      LocalCursorManager.ShowCursor(blockPlayer);
      
      if (blockPlayer)
      {
        LocalPlayer.BlockMoves();
        LocalPlayer.BlockRotation();
        LocalPlayer.BlockInspection();
      }
      else
      {
        LocalPlayer.UnblockMoves();
        LocalPlayer.UnblockRotation();
        LocalPlayer.UnblockInspection();
      }
    }

  }
}


