using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "MechaCannonSkill", menuName = "Scriptable/Skill/MechaCannonSkill")]
    public class SeguMechaCannonSkill : ActiveSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;
        
        private SeguMecha mecha;

        protected override bool UseAtkRatio => false;
        protected override bool UseGroggyRatio => false;

        [TitleGroup("스탯값")] [LabelText("대포 정보")] public ProjectileInfo cannonInfo;

        [TitleGroup("스탯값")] [LabelText("대포 반경")]
        public float cannonRadius;
        [TitleGroup("스탯값")] [LabelText("그로기 수치")] public int groggy;
        [TitleGroup("스탯값")] [LabelText("폭발 반경")] public float radius;
        
        public void Init(SeguMecha mecha)
        {
            this.mecha = mecha;
            actionList.Remove(Action);
            actionList.Add(Action);
        }

        void Action()
        {
            SpawnEffect(Define.PlayerEffect.GoseguDroneIcicleShoot, 0.5f, mecha.firePos.position, false);
            
            Projectile cannon = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.Effect,
                Define.PlayerSkillObjects.SeguMechaCannon,
                mecha.firePos.position);
            cannon.transform.localScale = Vector3.one * (cannonRadius * 2);
            cannon.Init(mecha,new FixedAmount(0),10);
            cannon.Init(cannonInfo);
            cannon.Init(groggy);
            cannon.AddEventUntilInitOrDestroy(SpawnExplosion,EventType.OnDestroy);
            cannon.Fire();
        }
        public override void Active()
        {
            base.Active();

            mecha.animator.SetInteger("AttackType",3);
            mecha.animator.SetTrigger("IsAttack");
        }

        void SpawnExplosion(EventParameters parameters)
        {
            if ((object)parameters?.user == null) return;
            var cannon = parameters.user;
            var explosion = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                Define.PlayerEffect.GoseguMechaCannonHit, cannon.transform.position);

            explosion.SetRadius(radius);
            
            explosion.Init(mecha,new AtkBase(mecha,cannonInfo.dmg));
            explosion.Init(GameManager.instance.Player.atkInfo);
            explosion.Init(groggy);
        }
    }
}