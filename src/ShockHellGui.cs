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

    private static float GUIXOffset { get; set; } = 50f;
    private static float GUIYOffset { get; set; } = 50f;
    private static float GUIWidth { get; set; } = Screen.width - GUIXOffset * 2f;
    private static float GUIHeight { get; set; } = Screen.height - GUIYOffset * 2f;
    private static float AuthBoxWidth { get; set; } = GUIWidth / 4f;
    private static Rect ShockHellGuiScreen = new Rect(GUIXOffset, GUIYOffset, GUIWidth, GUIHeight);

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
      ShockHellGuiScreen = GUILayout.Window(GetHashCode(), ShockHellGuiScreen, DrawWindow, "Shock Hell");
    }

    public void DrawWindow(int windowID)
    {
      using (new GUILayout.VerticalScope("API Connection", GUI.skin.box))
      {
        GUILayout.Space(GUIHeight * 0.02f);
        using (new GUILayout.HorizontalScope())
        {
          GUILayout.Space(GUIWidth / 2 - AuthBoxWidth / 2);
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
                LocalPiShockManager.Username = GUILayout.TextField(LocalPiShockManager.Username, 32);
                LocalPiShockManager.Apikey = GUILayout.TextField(LocalPiShockManager.Apikey, 36);
                LocalPiShockManager.Code = GUILayout.TextField(LocalPiShockManager.Code, 32);
              }
            }

            GUILayout.Space(GUIHeight * 0.02f);

            if (GUILayout.Button("Submit"))
            {
              LocalPiShockManager.SaveAuthConfig();
            }

            GUILayout.Space(GUIHeight * 0.01f);

            if (GUILayout.Button("Test with Vibration"))
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


