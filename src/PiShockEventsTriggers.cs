namespace ShockHell {
  class ConditionModuleTriggers : PlayerConditionModule {
    public override void OnTakeDamage(DamageInfo info) {
      base.OnTakeDamage(info);
      if (info.m_Blocked) return;
      float damage = info.m_Damage;

      if (m_HP > 0) {
        PiShockManager.Instance.Shock((int) damage, 1);
      } else {
        PiShockManager.Instance.Shock(100, 1);
        PiShockManager.Instance.Shock(50, 3);
      }
    }
  }
}
