
using chamwhy;
using Sirenix.OdinInspector;

namespace Apis
{
    public class SlapShield : Weapon
    {
        [Title("반사 변수")] [LabelText("반사 투사체 정보")]
        public ProjectileInfo projInfo;

        [LabelText("반사 그로기 계수")] public float reflectGroggy;
        
        [LabelText("피해 감소량(%)")] public float dmgReduce;
        
        protected override void OnEquip(IMonoBehaviour user)
        {
            base.OnEquip(user);
            foreach (var x in Player.attackColliders)
            {
                x.AddEvent(EventType.OnTriggerEnter,Reflect);
            }
            Player.AddEvent(EventType.OnAttackStateEnter,AddStat);
            Player.AddEvent(EventType.OnAttackStateExit,RemoveStat);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            foreach (var x in Player.attackColliders)
            {
                x.RemoveEvent(EventType.OnTriggerEnter,Reflect);
            }
            Player.RemoveEvent(EventType.OnAttackStateEnter,AddStat);
            Player.RemoveEvent(EventType.OnAttackStateExit,RemoveStat);
        }

        void AddStat(EventParameters parameters)
        {
            Player.AddStat(ActorStatType.DmgReduce,dmgReduce,ValueType.Value);
        }

        void RemoveStat(EventParameters parameters)
        {
            Player.AddStat(ActorStatType.DmgReduce,-dmgReduce,ValueType.Value);
        }
        void Reflect(EventParameters parameters)
        {
            if (parameters.collideData.collider.gameObject.CompareTag("EnemyEffect") && parameters.collideData.collider.TryGetComponent(out Projectile projectile))
            {
                projectile.Init(Player,new AtkBase(Player,projInfo.dmg));
                projectile.Init(projInfo);
                projectile.Init(CalculateGroggy(reflectGroggy));
                projectile.Fire();
            }
        }
    }
}