using UnityEngine;

namespace ShockHell
{
  class PlayerExtended : Player
  {
    protected override void Start()
    {
      base.Start();

      ModAPI.Log.Write("Initing ShockHell");
      new GameObject($"__{nameof(PiShockManager)}__").AddComponent<PiShockManager>();      
      new GameObject($"__{nameof(ShockHell)}__").AddComponent<ShockHell>();
      new GameObject($"__{nameof(ShockHellGui)}__").AddComponent<ShockHellGui>();
    }
  }
}
