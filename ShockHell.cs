using UnityEngine;

namespace ShockHell {
  /// <summary>
  /// The main mod class.
  /// </summary>
  public class ShockHell : MonoBehaviour {
    /// <summary>
    /// The main singleton instance.
    /// </summary>
    private static ShockHell Instance;
    private KeyCode ShortcutKey { get; set; } = KeyCode.Keypad1;

    private static ShockHellGui LocalShockHellGui;

    /// <summary>
    /// Get the singleton reference to this <see cref="ShockHell"/> instance.
    /// </summary>
    /// <returns></returns>
    public static ShockHell Get() {
      return Instance;
    }

    public ShockHell() {
      ///Flag to enable using GUILayout and GUI related functionality in Unity like OnGUI()
      useGUILayout = true;
      Instance = this;
    }

    protected virtual void Awake() {
      Instance = this;
    }

    protected virtual void OnDestroy() {
      Instance = null;
    }

    protected virtual void Start() {
      ///Initialize any locally used instance types in here, like your custom type, CursorManager, Player...
      ///to assure their existance and availability
      InitData();
    }

    private void InitData() {
      LocalShockHellGui = ShockHellGui.Get();
    }

    protected virtual void Update() {
      if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(ShortcutKey)) {
        InitData();
        if (!ShockHellGui.ShowGui) {
          LocalShockHellGui.EnableCursor(blockPlayer: true);
        }
        ToggleShowUI(0);
        if (!ShockHellGui.ShowGui) {
          LocalShockHellGui.EnableCursor(blockPlayer: false);
        }
      }
    }

    private void ToggleShowUI(int controlId) {
      switch (controlId) {
        case 0:
          ShockHellGui.ShowGui = !ShockHellGui.ShowGui;
          return;
        default:
          ShockHellGui.ShowGui = !ShockHellGui.ShowGui;
          return;
      }
    }

    protected virtual void OnGUI() {
      if (ShockHellGui.ShowGui) {
        InitData();
        LocalShockHellGui.DrawGUI();
      }
    }
  }
}
