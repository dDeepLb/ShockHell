namespace ShockHell {
  class ConditionModuleTriggers : PlayerConditionModule {
    public override void OnTakeDamage(DamageInfo info) {
      base.OnTakeDamage(info);
      if (info.m_Blocked) return;
      float damage = info.m_Damage;

      if (m_HP > 0) {
        PiShockManager.Shock((int) damage, 1);
      } else {
        PiShockManager.Shock(100, 1);
        PiShockManager.Shock(50, 3);
      }
    }
  }
}
