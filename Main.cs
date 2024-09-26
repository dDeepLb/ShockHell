using UnityEngine;

namespace ShockHell {
  class PlayerExtended : Player {
    protected override void Start() {
      base.Start();

      ModAPI.Log.Write("Initing ShockHell");
      PiShockManager.LoadAuthConfig();
      new GameObject("__ShockHell__").AddComponent<ShockHell>();
    }
  }

  class ShockHell : MonoBehaviour {
    private bool ShowUI = false;

    private void Update() {
      if (Input.GetKeyDown(KeyCode.Backslash)) {
        ShowUI = !ShowUI;

        ShockHellGui.EnableCursor(ShowUI);
      }
    }

    private void OnGUI() {
      if (ShowUI) {
        ShockHellGui.DrawGUI();
      }
    }
  }
}
