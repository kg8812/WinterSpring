using System.Collections.Generic;
using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class LilpaTree2D : SkillTree
    {
        private LilpaActiveSkill skill;

        [LabelText("별조각 최대 스택")] public int maxStack;
        [LabelText("별조각 발사 각도")] public float angle;
        [LabelText("별조각 투사체 설정")] public ProjectileInfo info;
        [LabelText("별조각 그로기 수치")] public int groggy;
        [LabelText("별조각 투사체 반경")] public float radius;

        private int curStack;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as LilpaActiveSkill;
            curStack = 0;
            if (skill == null) return;
            skill.OnWeaponEquip.RemoveListener(SpawnStarProjectiles);
            skill.OnWeaponEquip.AddListener(SpawnStarProjectiles);
            skill.Player.AddEvent(EventType.OnBasicAttack,AddStack);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.OnWeaponEquip.RemoveListener(SpawnStarProjectiles);
            skill?.Player.RemoveEvent(EventType.OnBasicAttack,AddStack);
        }

        void AddStack(EventParameters _)
        {
            if (skill.IsToggleOn) return;
            
            if (curStack <= maxStack)
            {
                curStack++;
            }
        }
        void SpawnStarProjectiles()
        {
            List<Projectile> temp = new();
            for (int i = 0; i < curStack; i++)
            {
                Projectile star = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.Effect,
                    Define.PlayerEffect.LilpaStar,skill.Player.Position);
                star.transform.localScale = Vector3.one * (radius * 2);
                
                star.Init(skill.Player,new AtkItemCalculation(skill.user as Actor , skill, info.dmg),10);
                star.Init(info);
                star.Init(groggy);
                
                star.Fire();
                temp.Add(star);
            }
            Utils.SetProjectilesAngle(temp,angle);
            curStack = 0;
        }
    }
}