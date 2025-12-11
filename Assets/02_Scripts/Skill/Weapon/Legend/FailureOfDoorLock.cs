using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class FailureOfDoorLock : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        protected override bool UseAtkRatio => false;
        protected override bool UseGroggyRatio => false;

        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("문 폭발 데미지")] public float doorDmg;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("문 폭발 그로기계수")] public float doorGroggy;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("폭발 반경")] public float radius;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("문 초기 생성위치")] [InfoBox("플레이어 발밑기준")] public Vector2 pos;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("문 떨어지는 Ease")] public Ease dropEase;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("문 떨어지는 시간")] public float doorSpeed;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("문 열리는 시간")] public float time1;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("대쉬 거리")] public float dashDistance;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("대쉬 시간")] public float dashTime;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("무기 사정거리")] public float distance;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("무기 공격설정")] public ProjectileInfo atkInfo;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("무기 그로기계수")] public float weaponGroggy;

        private GameObject door;
        private AttackObject weapons;

        private Tween tween;
        private Sequence seq;

        public override void Active()
        {
            base.Active();
            
            if (mover.ActorMovement.IsStick)
            {
                Vector3 temp = pos;
                temp.x *= direction != null ? (int)direction.Direction : 1;
                door = GameManager.Factory.Get(FactoryManager.FactoryType.Normal, "BabylonDoor",
                    user.transform.position + temp);
                dashUser?.DashOff();
                
                seq = DOTween.Sequence();
                seq.Append(door.transform.DOMoveY(user.transform.position.y, doorSpeed)
                    .SetUpdate(UpdateType.Fixed)).SetEase(dropEase);
                seq.AppendCallback(SpawnExplosion);
                seq.AppendCallback(() =>
                {
                    tween = mover?.ActorMovement.DashTemp(dashTime, dashDistance, true).SetAutoKill(true);
                    if (tween == null) return;
                    tween.onKill += EndMotion;
                    tween.onKill += () =>
                    {
                        dashUser?.DashOn();
                    };
                });
                seq.AppendInterval(time1);

                curDuration += time1 * doorSpeed;
                seq.AppendCallback(SpawnWeapons);
                seq.AppendCallback(() =>
                {
                    dashUser?.DashOn();
                    if (mover != null)
                    {
                        mover.ActorMovement.SetGravityToZero();
                    }
                });
               
            }
            else
            {
                door = GameManager.Factory.Get(FactoryManager.FactoryType.Normal, "BabylonDoor",
                    user.transform.position + Vector3.right * (pos.x * (direction != null ?(int)direction.Direction : 1)));
                SpawnExplosion();
                SpawnWeapons();

                mover?.ActorMovement.SetGravityToZero();
                tween = mover?.ActorMovement.DashTemp(dashTime, dashDistance, true).SetAutoKill(true);

                if (tween != null)
                {
                    tween.onKill += EndMotion;
                }
            }
        }

        void SpawnWeapons()
        {
            weapons = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                "BabylonCollider",
                door.transform.position + new Vector3(distance / 2 * (direction != null ? (int)direction.Direction : 1),
                    door.transform.localScale.y / 2, 0));
            weapons.transform.localScale = new Vector3(distance, door.transform.localScale.y, 1);
            weapons.Init(attacker, new AtkBase(attacker, atkInfo.dmg));
            weapons.Init(atkInfo);
            weapons.Init(Mathf.RoundToInt(weaponGroggy / 100 * BaseGroggyPower));
        }

        public override void AfterDuration()
        {
            base.AfterDuration();
            weapons?.Destroy();
            GameManager.Factory.Return(door);
        }

        void SpawnExplosion()
        {
            AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                "BaseExplosion", door.transform.position);
            exp.transform.localScale = Vector3.one * (0.6f * radius);
            exp.Init(attacker, new AtkBase(attacker, doorDmg), 1);
            exp.Init(Mathf.RoundToInt(doorGroggy / 100 * BaseGroggyPower));
        }
        public override void Cancel()
        {
            base.Cancel();
            tween?.Kill();
        }
    }
}