using Default;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class SwordOfFruit : MagicSkill
    {
        
        [PropertyOrder(2)] [TitleGroup("스탯값")] [LabelText("검 투사체설정")] public ProjectileInfo projInfo;
        [PropertyOrder(2)] [TitleGroup("스탯값")] [LabelText("검 반경")] public float projRadius;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("검 회전주기")] public float frequency;
        [PropertyOrder(2)] [TitleGroup("스탯값")] [LabelText("검 회전횟수")] public int count;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("과즙덩어리 크기")] public float size1;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("발사 x 반경")] public float xSize;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("발사 y 속도")] public float height;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("폭발 데미지")] public float juiceDmg;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("폭발 그로기 계수")] public float juiceGroggy;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("폭발 크기")] public float size2;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("폭발 지속시간")] public float duration2;
        

        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        private Sequence seq;
        private Projectile blade;
        public override void Active()
        {
            base.Active();
            SpawnProjectile();

            seq = DOTween.Sequence();
            for (int i = 0; i < count; i++)
            {
                seq.AppendCallback(SpawnJuice);
                seq.AppendInterval(frequency);
            }

            seq.onKill += () =>
            {
                blade?.Destroy();
                blade = null;
            };
        }

        void SpawnProjectile()
        {
            blade = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject,
                Define.DummyEffects.MagicSwordProjectile, user.Position);
            blade.transform.localScale = Vector3.one * (projRadius * 2);

            var rotate = blade.gameObject.GetOrAddComponent<RotatingEffect>();
            rotate.speed = 360 / frequency;
            rotate.decreaseSpeed = 180 / frequency;
            blade.Init(attacker,new AtkItemCalculation(attacker as Actor, this, projInfo.dmg ));
            blade.Init(projInfo);
            if (!mover.ActorMovement.IsStick)
            {
                blade.RotateBeforeFire(30);
            }
            blade.Fire();
            blade.AddEventUntilInitOrDestroy(_ =>
            {
                Destroy(rotate);
            },EventType.OnDestroy);
        }
        void SpawnJuice()
        {
            HotOrangeJuice juice = GameManager.Factory.Get<HotOrangeJuice>(FactoryManager.FactoryType.AttackObject,
                "HotOrangeJuice", blade.Position);
            float vx = Random.Range(-xSize / 2, xSize/2);
            float vy = height;
            juice.explosionDuration = duration2;
            juice.explosionDmg = juiceDmg;
            juice.explosionSize = size2;
            juice.explosionGroggy = juiceGroggy;
            juice.transform.localScale = Vector3.one * size1;
            juice.Init(attacker,new AtkBase(attacker,0));
            juice.firstVelocity = new Vector2(vx, vy);
            juice.Fire();
        }
    }
}