using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class GoseguTree1A : SkillTree
    {
        [Serializable]
        public struct Data
        {
            [LabelText("발사 개수")] public int atkCount;
        }
        private GoseguPassive skill;

        public Data[] datas;
        private GoseguDrone drone;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as GoseguPassive;

            if (skill == null) return;
            drone = skill.GetDrone(GoseguPassive.DroneType.Main);
            
            Debug.Log(drone);
            drone?.ChangeAtkStrategy(new GoseguDrone.FireProjectile(drone,
                drone._DroneInfo as GoseguMainDrone.MainDroneInfo, datas[level-1].atkCount));
        }

        public override void DeActivate()
        {
            base.DeActivate();

            drone?.ReturnToOriginalAtkStrategy();
        }
    }
}