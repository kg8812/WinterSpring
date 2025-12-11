using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Apis
{
    public abstract class IneCircleMagic : PlayerActiveSkill
    {
        protected IneActiveSkill playerSkill;
        protected abstract float Mana { get; }
        protected abstract int circle { get; }

        public override UI_AtkItemIcon Icon => Item?.Icon;

        protected override TagManager.SkillTreeTag Tag => TagManager.SkillTreeTag.Spell;

        protected override float TagIncrement => GameManager.Tag.Data.SpellIncrement;

        public override float Atk => base.Atk * (1 + playerSkill.AdditionalCircleDmg(circle) / 100f);

        public void Init(IneActiveSkill playerSkill)
        {
            this.playerSkill = playerSkill;
        }

        public override bool TryUse()
        {
            return base.TryUse() && playerSkill.mana >= Mana;
        }

        public override void Init()
        {
            base.Init();
            actionList.Clear();
            actionList.Add(ActivateCircle);
        }

        void ActivateCircle()
        {
            Magic();
            playerSkill.OnCircleUse.Invoke(circle);
        }
        protected abstract void Magic();
    }
}