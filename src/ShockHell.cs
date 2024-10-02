using UnityEngine;

namespace ShockHell
{
  /// <summary>
  /// The main mod class.
  /// </summary>
  public class ShockHell : MonoBehaviour
  {
    /// <summary>
    /// The main singleton instance.
    /// </summary>
    private static ShockHell Instance;
    private bool ShowUI { get; set; } = false;

    private static ShockHellGui LocalShockHellGui;

    /// <summary>
    /// Get the singleton reference to this <see cref="ShockHell"/> instance.
    /// </summary>
    /// <returns></returns>
    public static ShockHell Get()
    {
      return Instance;
    }

    public ShockHell()
    {
      ///Flag to enable using GUILayout and GUI related functionality in Unity like OnGUI()
      useGUILayout = true;
      Instance = this;
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
      ///Initialize any locally used instance types in here, like your custom type, CursorManager, Player...
      ///to assure their existance and availability
      InitData();
    }

    private void InitData()
    {
      LocalShockHellGui = ShockHellGui.Get();
    }

    protected virtual void Update()
    {
      if (Input.GetKeyDown(KeyCode.Backslash))
      {
        ShowUI = !ShowUI;

        LocalShockHellGui.EnableCursor(ShowUI);
      }
    }

    protected virtual void OnGUI()
    {
      if (ShowUI)
      {
        InitData();
        LocalShockHellGui.DrawGUI();
      }
    }
  }
}
