using chamwhy;
using chamwhy.DataType;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class ApprenticeProtection : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        protected override bool UseAtkRatio => false;

        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("공격 설정")] public ProjectileInfo atkInfo;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("폭발 크기")] public float size;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("콜라이더 크기")] public Vector2 colSize;

        [PropertyOrder(2)][TitleGroup("스탯값")][SerializeField] [LabelText("보호막 양 (체력%)")] private float amount1;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("배리어 지속시간")] public float duration2;
        
        private Buff barrier;

        public override void Active()
        {
            base.Active();

            GameObject effect = GameManager.Factory.Get(FactoryManager.FactoryType.Effect,
                "PaladinLight", user.Position);
            effect.transform.localScale = Vector3.one * size;
            GameManager.Factory.Return(effect, 1);
            AttackObject atk = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                "SquareAttackObject", user.Position);
            atk.transform.localScale = colSize;
            atk.Init(attacker,new AtkBase(attacker,atkInfo.dmg),1);
            atk.Init((int)BaseGroggyPower);
            atk.Init(atkInfo);

            if (barrier == null)
            {
                BuffDataType barrierData = new(SubBuffType.Buff_Barrier)
                {
                    buffPower = new[]{ amount1}, buffCategory = 1,applyStrategy = 0,
                    buffDuration = duration2, buffDispellType = 1, buffMaxStack = 1, stackDecrease = 0,
                    valueType = ValueType.Ratio, showIcon = false
                };
                barrier = new(barrierData,eventUser);
            }

            barrier.AddSubBuff(GameManager.instance.Player,null);
        }
    }
}