public partial class FoxSoldier
{
    public class IdleState : IState<FoxSoldier>
    {
        private FoxSoldier soldier;
        
        public void OnEnter(FoxSoldier t)
        {
            soldier = t;
            soldier.idlePattern.OnEnter();
        }

        public void Update()
        {
            soldier.idlePattern.Update();
        }

        public void FixedUpdate()
        {
        }

        public void OnExit()
        {
            soldier.idlePattern.OnExit();
        }
    }

    public class AttackState : IState<FoxSoldier>
    {
        private FoxSoldier soldier;
        public void OnEnter(FoxSoldier t)
        {
            soldier = t;
            soldier.isAttackable = false;
        }

        public void Update()
        {
            soldier?.atkPattern?.Update();
        }

        public void FixedUpdate()
        {
        }

        public void OnExit()
        {
            soldier.EffectSpawner.Remove(soldier.GetEffectAddress(EffectType.MagicianReady));
        }
    }

    public class DeadState : IState<FoxSoldier>
    {
        public void OnEnter(FoxSoldier t)
        {
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        public void OnExit()
        {
        }
    }
}
