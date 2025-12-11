using System.Collections.Generic;
using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class BlackRoseBlow : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Charge;

        [TabGroup("사용 관련/Charge/설정","차징 설정")]
        [ValidateInput("ValidateSize", "차징설정과 개수를 맞춰주세요")]
        [LabelText("차징별 풍압 크기")]
        public List<Vector2> windInfos;

        [TitleGroup("스탯값")] [LabelText("데미지 감소율")] public float dmgReduce;

        private BonusStat _stat;
        
        BonusStat StatEvent()
        {
            _stat ??= new();
            _stat.Stats[ActorStatType.DmgReduce].Value = dmgReduce;
            return _stat;
        }
        bool ValidateSize(List<Vector2> _sizes)
        {
            return _sizes.Count == chargeInfos.Count;
        }

        private Vector2 size;
        protected override void ChargeInvoke(int idx)
        {
            base.ChargeInvoke(idx);
            size = windInfos[idx];
        }

        public override void Init()
        {
            base.Init();
            actionList.Clear();
            actionList.Add(Attack);
        }

        void Attack()
        {
            AttackObject wind = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                "Prefabs/AttackObjects/WindWave",
                user.Position + Vector3.right * (direction != null ? (int)direction.Direction : 1 * size.x) / 2);

            wind.transform.localScale = size;
            wind.Init(attacker, new AtkBase(attacker, Atk),1);
            wind.Init(Mathf.RoundToInt((int)BaseGroggyPower));
        }

        public override void Active()
        {
            base.Active();
            statUser.StatManager.BonusStatEvent -= StatEvent;
        }

        public override void Cancel()
        {
            base.Cancel();
            statUser.StatManager.BonusStatEvent -= StatEvent;
        }

        public override void StartCharge()
        {
            base.StartCharge();
            statUser.StatManager.BonusStatEvent += StatEvent; 
            size = windInfos[0];
        }
    }
}