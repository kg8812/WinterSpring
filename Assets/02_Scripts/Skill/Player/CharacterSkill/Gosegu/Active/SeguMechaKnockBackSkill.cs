using chamwhy;
using Default;
using EventData;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "MechaKnockBackSkill", menuName = "Scriptable/Skill/MechaKnockBackSkill")]
    public class SeguMechaKnockBackSkill : ActiveSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        [TitleGroup("스탯값")] [LabelText("펄스탄 공격설정")] public ProjectileInfo atkInfo;
        [TitleGroup("스탯값")] [LabelText("펄스탄 반경")] public float radius;

        private GoseguActiveSkill skill;
        
        public void Init(GoseguActiveSkill skill)
        {
            this.skill = skill;
            actionList.Remove(Action);
            actionList.Add(Action);
        }

        void Action()
        {
            AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                Define.DummyEffects.Explosion, user.Position + Vector3.right * (radius * direction.DirectionScale + 1));
            exp.Init(attacker,new AtkItemCalculation(skill.user as Actor , skill ,atkInfo.dmg),1);
            exp.Init(atkInfo);
            exp.Init((int)BaseGroggyPower);
            exp.transform.localScale = Vector3.one * (radius * 0.6f);
        }
        public override void Active()
        {
            base.Active();
            skill.Mecha?.animator.SetInteger("AttackType",5);
            skill.Mecha?.animator.SetTrigger("IsAttack");
            
        }
    }
}