using System.Collections;
using System.Collections.Generic;
using Apis;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "MangBlueElementalSkill", menuName = "Scriptable/Skill/Weapon/MangBlueElementalSkill")]
    public class MangBlueElementSkill : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        [PropertyOrder(2)] [TitleGroup("스탯값")] [LabelText("스탭 거리")]
        public float distance;

        [PropertyOrder(2)] [TitleGroup("스탯값")] [LabelText("스탭 시간")]
        public float time;

        [TabGroup("스탯값/정령", "파랑 정령")] [LabelText("검기 설정")]
        public ProjectileInfo blueInfo;

        [TabGroup("스탯값/정령", "파랑 정령")] [LabelText("그로기 계수")]
        public float blueGroggy;

        private Tween tween;

        void Blue()
        {
            BlueElement blue = GameManager.Factory.Get<BlueElement>(FactoryManager.FactoryType.AttackObject,
                "BlueProjectile", user.Position);
            blue.Init(attacker, new AtkBase(attacker, blueInfo.dmg));
            blue.Init(blueInfo);
            blue.Init((int)blueGroggy);
            blue.Fire();
        }

        public override void Active()
        {
            base.Active();
            if (mover != null)
            {
                mover.ActorMovement.SetGravityToZero();
                tween = mover.ActorMovement.DashTemp(time, distance, true).SetAutoKill(true);
                tween.onKill += EndMotion;
            }
            Blue();
        }

        public override void Cancel()
        {
            base.Cancel();
            tween?.Kill();
        }
    }
}