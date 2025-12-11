using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class HealTwist : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Casting;

        protected override bool UseAtkRatio => false;

        private HolyArea area;
        
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("공격 설정")] public ProjectileInfo projInfo;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("회복량 (백분률)")] public float amount1;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("영역크기")] public Vector2 size;
        public override void Active()
        {
            base.Active();
        }

        public override void Init()
        {
            base.Init();
            actionList.Clear();
            actionList.Add(SpawnArea);
        }

        void SpawnArea()
        {
            area = GameManager.Factory.Get<HolyArea>(FactoryManager.FactoryType.AttackObject, "HolyArea",
                user.transform.position - Vector3.up * size.y / 2);
            area.transform.localScale = Vector3.one * size / 2;
            area.healAmount = amount1;
            area.Init(attacker,new AtkBase(attacker,projInfo.dmg));
            area.Init((int)(BaseGroggyPower));
            area.Init(projInfo);
        }

        public override void AfterDuration()
        {
            base.AfterDuration();
            area?.Destroy();
            area = null;
        }
    }
}