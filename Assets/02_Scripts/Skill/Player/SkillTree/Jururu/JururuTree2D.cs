using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class JururuTree2D : SkillTree
    {
        private JururuActiveSkill skill;

        [LabelText("점프 설정")] public Player.PokdoDashJump.PokdoDashInfo dashInfo;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);

            skill = active as JururuActiveSkill;
            if (skill == null) return;

            skill.OnPokdoSpawn -= SetDash;
            skill.OnPokdoSpawn += SetDash;
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill != null)
            {
                skill.OnPokdoSpawn -= SetDash;
            }
        }

        void SetDash(PokdoStand pokdo)
        {
            skill.Player.SetDash(new Player.PokdoDashJump(skill.Player,skill,dashInfo));
        }
    }
}