using System;
using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class IneTree2A : SkillTree
    {
        private IneActiveSkill skill;
        [Serializable]
        public struct Data
        {
            [LabelText("마나 회복치(%)")] public float manaGain;
        }

        public Data[] datas;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as IneActiveSkill;
            if (skill == null) return;
            
            skill.OnCircleUse.AddListener(HealMana);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            skill?.OnCircleUse.RemoveListener(HealMana);
        }

        void HealMana(int circle)
        {
            switch (circle)
            {
                case 2 :
                    skill.mana += skill.Circle2Mana / 100f * datas[level-1].manaGain;
                    break;
                case 3 :
                    skill.mana += skill.Circle3Mana / 100f * datas[level-1].manaGain;
                    break;
            }
        }
    }
}