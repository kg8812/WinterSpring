using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class LittleMoon : MagicSkill
    {
        [TitleGroup("스탯값")][LabelText("투사체 설정")] public ProjectileInfo projectileInfo;
        [TitleGroup("스탯값")][LabelText("투사체 크기")] public float scale1;
        [TitleGroup("스탯값")][LabelText("폭발 크기")] public float scale2;
        [TitleGroup("스탯값")][LabelText("폭발 지속시간")] public float duration2;
        [TitleGroup("스탯값")] [LabelText("기절 지속시간")] public float stunDuration;
        

        public override bool TryUse()
        {
            return base.TryUse() && (mover == null || mover.ActorMovement.IsStick);
        }

        public override void Init()
        {
            base.Init();
            actionList.Clear();
            actionList.Add(SpawnWindBall);
        }
        
        void SpawnWindBall()
        {
            LargeWindBall windBall = GameManager.Factory.Get<LargeWindBall>(FactoryManager.FactoryType.AttackObject,
                "LargeWindBall", user.Position + user.TopPivot);

            windBall.explosionScale = scale2;
            windBall.explosionDmg = Atk;
            windBall.explosionDuration = duration2;
            windBall.explosionGroggy = BaseGroggyPower;
            windBall.Init(attacker,new AtkBase(attacker,0));
            windBall.Init(projectileInfo);
            
            windBall.transform.localScale = new Vector3(scale1, scale1, 1);
            windBall.AddEventUntilInitOrDestroy(param =>
            {
                if (param?.target is Actor target)
                {
                    target.SubBuffManager.AddCC(eventUser, SubBuffType.Debuff_Stun,stunDuration);
                }
            });
            windBall.Fire();
        }
        protected override ActiveEnums _activeType => ActiveEnums.Casting;
    }
}