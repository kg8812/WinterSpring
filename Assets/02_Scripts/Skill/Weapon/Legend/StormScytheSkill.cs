using System;
using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class StormScytheSkill : MagicSkill
    {
        protected override bool UseAtkRatio => false;

        [TitleGroup("스탯값")][LabelText("돌풍 공격설정")] public ProjectileInfo info;
        [TitleGroup("스탯값")][LabelText("돌풍 공격횟수")] public int count;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("돌진 거리")] public float distance;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("돌풍 반경")] public float radius;


        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        protected override CDEnums _cdType => CDEnums.Stack;

        private ICdActive _cdActive;
        public override ICdActive CDActive => _cdActive ??= new StackCd(this);

        public override void Active()
        {
            base.Active();
            
            mover?.ActorMovement.SetGravityToZero();
            GameManager.instance.Player.DashLandingOff();
            Tween tweener = mover?.ActorMovement.DashTemp(Duration, distance, false).SetAutoKill(true);
            if (tweener == null) return;
            
            Guid guid = hit.AddInvincibility(); 
            GameManager.instance.Player.StopJumpCoroutine();

            tweener.onKill += () =>
            {
                hit.RemoveInvincibility(guid);
                EndMotion();
                Cancel();
            };
            
            
            AttackObject wind = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                "StormScytheWind", user.Position);
            wind.Init(attacker,new AtkBase(attacker,info.dmg),Duration);
            wind.Init(info);
            wind.Frequency = Duration / count;
            wind.Init(_weapon.CalculateGroggy(BaseGroggyPower));
            SpineUtils.AddBoneFollower(skeleton?.Mecanim, "center", wind.gameObject);
            wind.transform.localScale = Vector2.one * (radius * 2);
        }
        
    }
}