using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class GoseguTree1E : SkillTree
    {
        private GoseguPassive skill;
        
        private GoseguMainDrone drone;

        [LabelText("공격 설정")] public ProjectileInfo atkInfo;
        [LabelText("폭발 반경")] public float radius;

        private GoseguMainDrone.MainDroneInfo info;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as GoseguPassive;

            if (skill == null) return;
            drone = skill.GetDrone(GoseguPassive.DroneType.Main) as GoseguMainDrone;
            if (drone != null)
            {
                info = drone._DroneInfo as GoseguMainDrone.MainDroneInfo;
                drone.OnAttack -= AddExplosion;
                drone.OnAttack += AddExplosion;
            }
        }

        public override void DeActivate()
        {
            base.DeActivate();
            if (drone != null)
            {
                drone.OnAttack -= AddExplosion;
            }
        }

        void AddExplosion(AttackObject obj)
        {
            obj.isAtk = false;
            obj.AddEventUntilInitOrDestroy(SpawnExplosion);
        }
        void SpawnExplosion(EventParameters eventParameters)
        {
            if (eventParameters?.user is not AttackObject proj || drone == null || info == null) return;
            
            AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                Define.PlayerEffect.GoseguDroneIcicleExplosion, eventParameters.user.transform.position);

            exp.transform.localScale = Vector3.one * (radius * 2);
            exp.Init(drone, new AtkBase(drone, atkInfo.dmg),1);
            exp.Init(atkInfo);
            exp.Init(info.groggy);
        }
    }
}