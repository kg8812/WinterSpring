using chamwhy;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis.SkillTree
{
    public class LilpaTree2A : SkillTree
    {
        private LilpaActiveSkill skill;

        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            
            skill = active as LilpaActiveSkill;
            if (skill == null) return;
            skill.OnWeaponEquip.RemoveListener(AddCritEvent);
            skill.OnWeaponUnEquip.RemoveListener(RemoveCritEvent);
            skill.OnWeaponEquip.AddListener(AddCritEvent);
            skill.OnWeaponUnEquip.AddListener(RemoveCritEvent);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill.OnWeaponEquip.RemoveListener(AddCritEvent);
            skill.OnWeaponUnEquip.RemoveListener(RemoveCritEvent);
        }

        void AddCritEvent()
        {
            skill.lilpaWeapons.Values.ForEach(x =>
            {
                x.OnAtkObjectInit.RemoveListener(MakeCrit);
                x.OnBeforeAttack -= ChangeEffect;
                if (x.Skill.CurStack == x.Skill.MaxStack)
                {
                    x.OnBeforeAttack += ChangeEffect;
                    x.OnAtkObjectInit.AddListener(MakeCrit);
                }
            });
        }

        void ChangeEffect(Weapon weapon)
        {
            LilpaShotgun shotgun = weapon as LilpaShotgun;
            if (shotgun != null)
            {
                shotgun.isEnhanced = true;
            }
        }
        void RemoveCritEvent()
        {
            skill.lilpaWeapons.Values.ForEach(x =>
            { 
                x.OnBeforeAttack -= ChangeEffect;
               x.OnAtkObjectInit.RemoveListener(MakeCrit);
            });
        }
        void MakeCrit(EventParameters parameters)
        {
            
            if (parameters?.user is AttackObject atk)
            {
                atk.isCrit = true;
            }
            RemoveCritEvent();
        }
    }
}