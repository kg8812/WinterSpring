namespace Apis
{
    public class FantasticDawnSkill : MagicSkill
    {
        private FantasticDawn dawn;
        
        public override void Init(Weapon weapon)
        {
            base.Init(weapon);
            dawn = weapon as FantasticDawn;
        }

        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        public override void Active()
        {
            base.Active();
            if (dawn != null)
            {
                dawn.TurnOnBoth();
            }
        }

        public override void AfterDuration()
        {
            base.AfterDuration();
            dawn.TurnOffBoth();
        }
    }
}