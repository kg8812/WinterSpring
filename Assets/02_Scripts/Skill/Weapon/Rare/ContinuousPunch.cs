using Sirenix.OdinInspector;
using Sirenix.Utilities;

namespace Apis
{
    public class ContinuousPunch : MagicSkill
    {
        [Title("용과같이 스킬")] 
        protected override ActiveEnums _activeType => ActiveEnums.Continuous;

        public override void Active()
        {
            base.Active();
            
            GameManager.instance.Player.attackColliders.ForEach(x =>
            {
                x.Init(attacker,new AtkBase(attacker,Atk));
                x.Init((int)BaseGroggyPower);
            });
        }

        public override void Cancel()
        {
            base.Cancel();
            EndMotion();
        }
    }
}