using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class Flaming : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        [PropertyOrder(2)] [TitleGroup("스탯값")] [LabelText("불 투사체정보")] public ProjectileInfo projInfo;
        [PropertyOrder(2)] [TitleGroup("스탯값")] [LabelText("불 발사정보")] public BeamEffect.BeamInfo beamInfo;

        private BeamEffect fire;
        
        public override void Active()
        {
            base.Active();
            fire = GameManager.Factory.Get<BeamEffect>(FactoryManager.FactoryType.AttackObject, "NyongpaFire",
                user.Position);
           fire.Init(beamInfo);
           fire.Init(attacker,new AtkItemCalculation(attacker as Actor, this, projInfo.dmg));
           fire.Init(projInfo);
           fire.Fire();
        }

        public override void AfterDuration()
        {
            base.AfterDuration();
            fire?.Destroy();
            fire = null;
        }
    }
}