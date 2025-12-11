using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class GoseguTree1D : SkillTree
    {
        private GoseguPassive skill;
        [InfoBox("드론을 클릭하여 선택한 후 드론 프리팹 인스펙터창에서 스탯값을 수정해주세요")]
        [LabelText("치유 드론")] [SerializeField] private GoseguHealingDrone drone;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as GoseguPassive;

            if (skill == null) return;
            
            skill.CreateDrone(GoseguPassive.DroneType.Heal);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.RemoveDrone(GoseguPassive.DroneType.Heal);
        }
    }
}