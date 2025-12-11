using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class IneTree1E : SkillTree
    {
        private InePassiveSkill skill;

        [LabelText("초승달 베기 공격 설정")] public ProjectileInfo info;
        [LabelText("초승달 베기 그로기 수치")] public int groggy;
        [LabelText("이펙트 반경")] public float radius;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as InePassiveSkill;

            if (skill != null)
            {
                skill.ChangeFireStrategy(new MoonSlash(skill,info,groggy,radius));
            }
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.ChangeFireStrategy(new FireFeather(skill));
        }
    }
}