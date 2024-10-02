using ShockHell.Managers;
using UnityEngine;

namespace ShockHell.Extensions
{
  class PlayerExtended : Player
  {
    protected override void Start()
    {
      base.Start();

      new GameObject($"__{nameof(PiShockManager)}__").AddComponent<PiShockManager>();      
      new GameObject($"__{nameof(ShockHell)}__").AddComponent<ShockHell>();
      new GameObject($"__{nameof(ShockHellGui)}__").AddComponent<ShockHellGui>();
    }
  }
}
