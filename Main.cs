using UnityEngine;

namespace ShockHell {
  class AddMyGameObject : Player {
    protected override void Start() {
      base.Start();
      new GameObject("__ShockHell__").AddComponent<ShockHell>();
    }
  }

  class ShockHell : MonoBehaviour {
    private bool ShowUI = false;
    private ShockHellGUI GUI = null;

    private void Start() {
      GUI = new ShockHellGUI();
    }

    private void Update() {
      if (Input.GetKeyDown(KeyCode.Backslash)) {
        ShowUI = !ShowUI;

        GUI.EnableCursor(ShowUI);
      }
    }

    private void OnGUI() {
      if (ShowUI) {
        GUI.DrawGUI();
      }
    }
  }
}
