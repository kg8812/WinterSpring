using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class LilpaTree2B : SkillTree
    {
        [HideInInspector] public LilpaActiveSkill skill;
        private LilpaSword weapon;

        [LabelText("돌진 거리")] public float distance;
        [LabelText("돌진 시간")] public float duration;
        [LabelText("베기 데미지")] public float dmg;
        [LabelText("베기 그로기")] public int groggy;
        [LabelText("돌진 이펙트 반경")] public float radius;
        [LabelText("이펙트 offset (중앙 기준)")] public Vector2 offset;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as LilpaActiveSkill;
            
            if (skill == null) return;

            // ItemId - 3053 : 릴파의 장검
            InvenManager.instance.PresetManager.ModifyPresetItem(6,0,1, 3053,weapon);
            if (weapon == null)
            {
                weapon = skill.lilpaWeapons[LilpaActiveSkill.WeaponType.Sword] as LilpaSword;
                weapon?.Init(new LilpaSword.RushInfo
                {
                    distance = distance,
                    duration = duration,
                    dmg = dmg, groggy = groggy,
                    radius = radius,
                    offset = offset
                });
            }
            skill.WhenChanged();
        }

        public override void DeActivate()
        {
            base.DeActivate();
            InvenManager.instance.PresetManager.ModifyPresetItem(6,0,1,0);
            skill.WhenChanged();
        }
    }
}