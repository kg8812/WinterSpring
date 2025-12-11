using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class ViichanTree1D : ViichanTree
    {
        private ViichanPassiveSkill skill;
        [LabelText("1타 원념발톱 공격설정")] public ProjectileInfo atkInfo1;
        [LabelText("2타 원념발톱 공격설정")] public ProjectileInfo atkInfo2;
        [LabelText("원념발톱 반경")] public float radius;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);

            skill = passive as ViichanPassiveSkill;
            if (skill == null) return;
            skill.OnBeastAtk -= SpawnEffect;
            skill.OnBeastAtk += SpawnEffect;
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill != null)
            {
                skill.OnBeastAtk -= SpawnEffect;
            }
        }

        void SpawnEffect(int index)
        {
            var atkInfo = index == 1 ? atkInfo1 : atkInfo2;
            
            AttackObject effect = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                Define.PlayerEffect.ViichanBeastClaw, skill.Player.Position);
            
            effect.Init(skill.Player,new AtkItemCalculation(skill.Player,skill,atkInfo.dmg),1);
            effect.Init(atkInfo);
            
            effect.gameObject.SetRadius(radius,skill.Player.Direction);
        }
        
    }
}