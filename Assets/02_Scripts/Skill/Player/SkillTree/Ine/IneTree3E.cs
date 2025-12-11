using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class IneTree3E : SkillTree
    {
        [LabelText("타격당 데미지 증가량 (%)")] public float dmgIncrement;
        [LabelText("최대 증가량 (%)")] public float max;
        [LabelText("마나당 데미지 증가량 (%)")] public float featherDmg;

        private IneActiveSkill active;
        private InePassiveSkill passive;

        private IneActiveAttachment _attachment;
        private InePassiveAttachment _passiveAttachment;
        
        public override void Activate(PlayerActiveSkill _active, int level)
        {
            base.Activate(_active, level);

            active = _active as IneActiveSkill;
            _attachment ??= new (new IneActiveStat());
            _passiveAttachment ??= new(new InePassiveStat());

            if (active == null) return;
            
            active.AfterCircleUse.AddListener(ResetDmg);
            active.OnManaChange.AddListener(AddFeatherDmg);
            active.RemoveAttachment(_attachment);
            active.AddAttachment(_attachment);
        }
        
        public override void Activate(PlayerPassiveSkill _passive, int level)
        {
            base.Activate(_passive, level);
            passive = _passive as InePassiveSkill;
            _passiveAttachment ??= new(new InePassiveStat());
            _attachment ??= new (new IneActiveStat());

            if (passive == null) return;
            
            passive.RemoveAttachment(_passiveAttachment);
            passive.AddAttachment(_passiveAttachment);
            passive.OnFeatherAtk.AddListener(AddDmg);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            active?.RemoveAttachment(_attachment);
            passive?.RemoveAttachment(_passiveAttachment);
        }

        void AddFeatherDmg(float mana)
        {
            _passiveAttachment.IneStat.dmgRatio = mana * featherDmg;
        }
        void AddDmg()
        {
            if (_attachment.IneStat.highCircleDmgIncrement < max)
            {
                _attachment.IneStat.highCircleDmgIncrement += dmgIncrement;
            }
        }

        void ResetDmg(int circle)
        {
            if (circle >= 2)
            {
                _attachment.IneStat.highCircleDmgIncrement = 0;
            }
        }
    }
}