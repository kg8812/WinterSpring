using chamwhy;
using Default;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "IneCircle4Beam", menuName = "Scriptable/Skill/Ine/IneCircle4Beam")]
    public class IneCircle4Beam : PlayerActiveSkill
    {
        public override UI_AtkItemIcon Icon => Item?.Icon;

        protected override TagManager.SkillTreeTag Tag => TagManager.SkillTreeTag.Spell;

        protected override float TagIncrement => GameManager.Tag.Data.SpellIncrement;
        
        public override float Atk => base.Atk * skill.AdditionalCircleDmg(4) / 100f;

        protected override ActiveEnums _activeType => ActiveEnums.Instant;
        private IneActiveSkill skill;
        public override void BeforeAttack()
        {
            base.BeforeAttack();
            animator.animator.SetInteger("AttackType", 3);
        }
        
        public void Init(IneActiveSkill skill)
        {
            this.skill = skill;
        }
        public override void Active()
        {
            base.Active();
            
            Projectile beam = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject,
                Define.PlayerSkillObjects.IneCircle4Beam, GameManager.instance.ControllingEntity.Position);
            beam.Init(skill.attacker, 
                new AtkItemCalculation(skill.attacker as Actor,this, skill.circle4BeamInfo1.dmg));
            beam.Init(skill.circle4BeamInfo1);
            beam.Init(skill.beam1Groggy);
            beam.AddEventUntilInitOrDestroy(SpawnBeams);
            beam.Fire();
        }
        void SpawnBeams(EventParameters parameters)
        {
            AttackObject atk = parameters?.user as AttackObject;

            if (atk == null) return;

            var enemies = parameters.user.gameObject.GetTargetsInCircle(skill.circle4BeamRadius, LayerMasks.Enemy);

            enemies.ForEach(x =>
            {
                if (atk.firstAttackedTarget == x.gameObject) return;

                Projectile beam = GameManager.Factory.Get<Projectile>(
                    FactoryManager.FactoryType.AttackObject, Define.PlayerSkillObjects.IneCircle4Beam,
                    parameters.user.transform.position);
                beam.Init(skill.attacker, new AtkItemCalculation(skill.attacker as Actor,this, skill.circle4BeamInfo2.dmg));
                beam.Init(skill.circle4BeamInfo2);
                beam.Init(skill.beam2Groggy);
                beam.LookAtTarget(x);
                beam.Fire();
            });
        }
    }
}