using UnityEngine;

namespace ShockHell {
  static class ShockHellGui {
    private static float GUIXOffset = 50f;
    private static float GUIYOffset = 50f;
    private static float GUIWidth = Screen.width - GUIXOffset * 2f;
    private static float GUIHeight = Screen.height - GUIYOffset * 2f;

    private static Rect GUIScreen = new Rect(GUIXOffset, GUIYOffset, GUIWidth, GUIHeight);

    public static void DrawGUI() {
      GUI.skin = ModAPI.Interface.Skin;
      GUIScreen = GUILayout.Window(0, GUIScreen, DrawWindow, "Shock Hell");
    }

    public static void DrawWindow(int windowID) {
      using (new GUILayout.VerticalScope("API Connection", GUI.skin.box)) {
        GUILayout.Space(GUIHeight * 0.02f);
        using (new GUILayout.VerticalScope(GUILayout.Width(GUIHeight * 0.35f))) {
          using (new GUILayout.HorizontalScope()) {
            using (new GUILayout.VerticalScope()) {
              GUILayout.Label("PiShock Username");
              GUILayout.Label("PiShock API Key");
              GUILayout.Label("PiShock Code");
            }

            using (new GUILayout.VerticalScope()) {
              PiShockManager.Username = GUILayout.TextField(PiShockManager.Username, 32);
              PiShockManager.APIKey = GUILayout.TextField(PiShockManager.APIKey, 36);
              PiShockManager.Code = GUILayout.TextField(PiShockManager.Code, 32);
            }
          }

          GUILayout.Space(GUIHeight * 0.02f);

          if (GUILayout.Button("Submit")) {
            PiShockManager.SaveAuthConfig();
          }
        }
      }
    }

    public static void EnableCursor(bool enableCursor) {
      CursorManager.Get().ShowCursor(enableCursor, false);
      Player player = Player.Get();

      if (enableCursor) {
        player.BlockMoves();
        player.BlockRotation();
        player.BlockInspection();
      } else {
        player.UnblockMoves();
        player.UnblockRotation();
        player.UnblockInspection();
      }
    }
  }
}


