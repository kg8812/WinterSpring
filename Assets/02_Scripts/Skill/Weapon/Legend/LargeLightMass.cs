using chamwhy.DataType;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class LargeLightMass : MagicSkill
    {
        protected override bool UseAtkRatio => false;
        protected override bool UseGroggyRatio => false;
        protected override ActiveEnums _activeType => ActiveEnums.Instant;
        
        [TitleGroup("스탯값")][LabelText("투사체 설정")] public ProjectileInfo info;
        [TitleGroup("스탯값")][LabelText("성운 지속시간")] public float d;
        [TitleGroup("스탯값")][LabelText("성운 공격력 증가량")] public float dmg2;

        private Buff _buff;

        Buff buff
        {
            get
            {
                if (_buff == null)
                {
                    BuffDataType data = new BuffDataType(SubBuffType.Buff_BasicAtkEnhance)
                    {
                        buffPower = new[]{dmg2}, buffCategory = 1, buffDuration = d,
                        buffDispellType = 1, buffMaxStack = 1, showIcon = true,valueType = ValueType.Ratio,
                    };
                    _buff = new(data, eventUser);
                }

                return _buff;
            }
        }

        public override void Init()
        {
            base.Init();
            actionList.Clear();
            actionList.Add(Fire);
        }
        public override void Active()
        {
            base.Active();
            buff.AddSubBuff(GameManager.instance.Player,null);
        }

        void Fire()
        {
            Debug.Log("Fire");
            Projectile proj = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject,
                "bullet", user.Position);

            proj.Init(attacker, new AtkBase(attacker, info.dmg));
            proj.Init(info);
            proj.Init((int)BaseGroggyPower);
            proj.Fire();
        }
    }
}