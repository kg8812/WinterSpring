using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class GoseguTree1B : SkillTree
    {
        [InfoBox("드론을 클릭하여 선택한 후 드론 프리팹 인스펙터창에서 스탯값을 수정해주세요")] [LabelText("미사일 드론")]
        public GoseguMissileDrone dronePrefab;

        private GoseguMissileDrone drone;

        private GoseguPassive skill;

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            
            skill = passive as GoseguPassive;

            if (skill == null) return;

            skill.CreateDrone(GoseguPassive.DroneType.Missile);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            skill?.RemoveDrone(GoseguPassive.DroneType.Missile);
        }
    }
}