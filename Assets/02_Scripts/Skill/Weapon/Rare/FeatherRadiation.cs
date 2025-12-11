using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class FeatherRadiation : MagicSkill
    {
        protected override bool UseAtkRatio => false;

        [TitleGroup("스탯값")][LabelText("파편 공격설정")] public ProjectileInfo projInfo;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("기절 지속시간")] public float time;

        protected override ActiveEnums _activeType => ActiveEnums.Instant;
        private AttackObject atk;

        public override void Active()
        {
            base.Active();
            if (mover == null) return;
            
            mover.Stop();
            mover.Rb.gravityScale = 0;
        }

        public void Invoke()
        {
            atk = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect, "StoneFragments",
                user.transform.position + Vector3.right * (direction != null ? (int)direction.Direction : 1));
            
            atk.Init(attacker,new AtkBase(attacker,projInfo.dmg),1);
            atk.Init(projInfo);
            atk.Init((int)BaseGroggyPower);
            Debug.Log(atk._attacker);
        }

        public override void Init()
        {
            base.Init();
            actionList.Clear();
            actionList.Add(Invoke);
        }

        void AddStun(EventParameters parameters)
        {
            if (parameters?.target is Actor target)
            {
                target.StartStun(eventUser,time);
            }
        }
    }
}