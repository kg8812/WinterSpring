using System;
using UnityEngine;

namespace Apis.SkillTree
{
    public class IneTree3D : SkillTree
    {
        private Player player => GameManager.instance.Player;
        private IneActiveSkill skill;

        private Guid guid;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);

            skill = active as IneActiveSkill;
            
            if (skill == null) return;

            skill.OnToggle.AddListener(On);
            skill.WhenChanged();
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.OnToggle.RemoveListener(On);
            On(false);
        }

        void On(bool tf)
        {
            if (tf)
            {
                guid = player.AddHitImmunity();
            }
            else
            {
                
                player.RemoveHitImmunity(guid);
            }
        }
    }
}