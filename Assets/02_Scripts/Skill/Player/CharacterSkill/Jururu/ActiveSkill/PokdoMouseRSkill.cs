using Apis.SkillTree;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "PokdoGrabSkill",menuName = "Scriptable/Skill/PokdoGrabSkill")]
    public class PokdoMouseRSkill : ActiveSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        protected override bool UseAtkRatio => false;

        protected override bool UseGroggyRatio => false;

        [TitleGroup("스탯값")] [LabelText("그랩 공격설정")]
        public ProjectileInfo atkInfo;

        [TitleGroup("스탯값")] [LabelText("그로기 수치")]
        public int groggy;

        [TitleGroup("스탯값")] [LabelText("소환 거리")]
        public float distance;

        [TitleGroup("스탯값")] [LabelText("그랩 종료 거리")] [Tooltip("플레이어 위치 기준으로")]
        public float endDistance;
        [TitleGroup("스탯값")] [LabelText("스턴 지속시간")]
        public float stunDuration;

        [HideInInspector] public PokdoStand pokdo;

        private JururuTree2E tree;
        public void Init(JururuTree2E tree)
        {
            this.tree = tree;
        }
        public override void Active()
        {
            base.Active();

            AttackObject grab = pokdo.SpawnGrab(this);
            pokdo.animator.SetTrigger("SkillGrab");
            if (grab != null && tree.Level >= 2)
            {
                grab.AddEventUntilInitOrDestroy(x =>
                {
                    if (x?.target is Actor target)
                    {
                        target.SubBuffManager.AddCC(eventUser,SubBuffType.Debuff_Stun,stunDuration);
                    }
                });
            }
        }
    }
}