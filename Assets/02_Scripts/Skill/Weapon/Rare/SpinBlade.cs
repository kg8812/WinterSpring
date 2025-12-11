using chamwhy;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace Apis
{
    class SpinBlade : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        protected override bool UseAtkRatio => false;

        [TitleGroup("스탯값")][LabelText("공격설정")] public ProjectileInfo atkInfo;
        [TitleGroup("스탯값")][LabelText("바벨 반경")] public float radius;

        private AttackObject effect;
        public override void Active()
        {
            base.Active();
            effect = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                "barbellSkillEffect", user.Position);
            SpineUtils.AddBoneFollower(skeleton?.Mecanim, "center", effect.gameObject);
            effect.AddEventUntilInitOrDestroy(x =>
            {
                Destroy(effect.GetComponent<BoneFollower>());
            },EventType.OnDestroy);

            effect.transform.localScale = Vector3.one * (radius * 2);
            effect.Init(attacker,new AtkBase(attacker,atkInfo.dmg));
            effect.Init((int)BaseGroggyPower);
            effect.AddAtkEventOnce(param =>
            {
                if (param.target is Actor target)
                {
                    target.AddSubBuff(eventUser,SubBuffType.Debuff_Bleed);
                }
            });
        }

        public override void AfterDuration()
        {
            base.AfterDuration();
            effect?.Destroy();
            effect = null;
        }
    }
}