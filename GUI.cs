using UnityEngine;

namespace ShockHell {
  class ShockHellGUI {
    private static float GUIXOffset = 50f;
    private static float GUIYOffset = 50f;
    private static float GUIWidth = Screen.width - GUIXOffset * 2f;
    private static float GUIHeight = Screen.height - GUIYOffset * 2f;

    private GUILayout GUIScreen;

    private GUIStyle titleStyle;
    private GUIStyle labelStyle;

    public void DrawGUI() {
      GUI.skin = ModAPI.Interface.Skin;

      if (titleStyle == null) {
        titleStyle = new GUIStyle(GUI.skin.label) {
          fontSize = 24,
          alignment = TextAnchor.UpperCenter,
        };
      }

      if (titleStyle == null) {
        titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 14;
      }

      GUI.Box(new Rect(GUIXOffset, GUIYOffset, GUIWidth, GUIHeight), "", GUI.skin.window);
      GUI.Label(new Rect(Screen.width / 2f - 100f, 65f, 200f, 30f), "Shock Hell", titleStyle);
    }

    public void EnableCursor(bool enableCursor) {
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
