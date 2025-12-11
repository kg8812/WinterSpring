using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class Jingburger2B : SkillTree
    {
        [LabelText("차징시간 및 데미지")] public List<ActiveSkill.ChargeInfo> chargeInfos = new();
        [LabelText("차징별 크기(%)")] public List<float> chargeRadius = new();
        
        private JingburgerActiveSkill skill;
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);

            if (active == null) return;
            
            skill = active as JingburgerActiveSkill;
            if (skill == null) return;
            skill.chargeRadius = chargeRadius;
            active.ChangeToChargeSkill(chargeInfos);
            active.chargeType = 2;
            active.OnChargeStart.RemoveListener(SpawnEffect);
            active.OnChargeStart.AddListener(SpawnEffect);
            active.OnChargeEnd.RemoveListener(RemoveEffect);
            active.OnChargeEnd.AddListener(RemoveEffect);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            if (skill != null)
            {
                skill.ChangeActiveType(ActiveSkill.ActiveEnums.Instant);
                skill.OnChargeStart.RemoveListener(SpawnEffect);
                skill.OnChargeEnd.RemoveListener(RemoveEffect);
            }
        }

        void SpawnEffect()
        {
            skill.SpawnEffect(Define.PlayerEffect.JingRasenganCharge, 0.5f, false);
        }

        void RemoveEffect()
        {
            skill.RemoveEffect(Define.PlayerEffect.JingRasenganCharge);
        }
    }
}