using chamwhy;
using chamwhy.DataType;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "MechaMissileSkill", menuName = "Scriptable/Skill/MechaMissileSkill")]
    public class SeguMechaMissileSkill : ActiveSkill 
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        protected override bool UseAtkRatio => false;
        protected override bool UseGroggyRatio => false;

        [TitleGroup("스탯값")] [LabelText("미사일 투사체 설정")] public ProjectileInfo info;
        [TitleGroup("스탯값")] [LabelText("미사일 발사 개수")] public int count;
        [TitleGroup("스탯값")] [LabelText("미사일 그로기 수치")] public int groggy;
        [TitleGroup("스탯값")] [LabelText("둔화량 (%)")] public float debuffAmount;
        [TitleGroup("스탯값")] [LabelText("둔화 지속시간")] public float debuffDuration;

        private Buff _buff;
        
        Buff buff
        {
            get
            {
                if (_buff == null)
                {
                    
                        BuffDataType data = new BuffDataType(SubBuffType.Debuff_MoveSpeed)
                        {
                            buffPower = new[] { debuffAmount },
                            buffDuration = debuffDuration,
                            buffCategory = 1,
                            buffDispellType = 1, buffMaxStack = 1, valueType = ValueType.Ratio, showIcon = false
                        };
                        _buff = new(data, eventUser);
                }

                return _buff;
            }
        }
        public override void Active()
        {
            base.Active();
            
            var list = GameManager.Factory.SpawnWithPadding<Projectile>(FactoryManager.FactoryType.AttackObject,
                Define.PlayerSkillObjects.GoseguMissile, user.Position,
                count, FactoryManager.Direction.Vertical);

            foreach (var missile in list)
            {
                missile.Init(attacker,new AtkBase(attacker,info.dmg),10);
                missile.Init(info);
                missile.Init(groggy);
                missile.AddEventUntilInitOrDestroy(AddDebuff);
                missile.Fire();
            }
        }

        void AddDebuff(EventParameters eventParameters)
        {
            if (eventParameters?.target is Actor target)
            {
                buff.AddSubBuff(target,null);
            }
        }
    }
}