using ShockHell.Managers;

namespace ShockHell.Extensions {
  public class ConditionModuleTriggers : PlayerConditionModule {
    public override void OnTakeDamage(DamageInfo info) {
      base.OnTakeDamage(info);
      if (info.m_Blocked) return;
      float damage = info.m_Damage;

      if (m_HP > 0) {
        PiShockManager.Get().Shock((int) damage, 1);
      } else {
        PiShockManager.Get().Shock(100, 1);
        PiShockManager.Get().Shock(50, 3);
      }
    }
  }
}
