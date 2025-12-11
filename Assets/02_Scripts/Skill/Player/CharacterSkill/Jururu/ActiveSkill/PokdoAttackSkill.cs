using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "PokdoAttack", menuName = "Scriptable/Skill/Jururu/PokdoAttack")]
    public class PokdoAttackSkill : ActiveSkill
    {
        private PokdoStand stand;
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        protected override void TurnOnActiveMotion()
        {
            base.TurnOnActiveMotion();
            Player player = GameManager.instance.Player;
            animator?.animator.SetInteger("ActiveSkillType", index);

            if (player.GetState() == EPlayerState.Skill)
            {
                animator?.animator.ResetTrigger("PlayerSkillEnd");
                animator?.animator.ResetTrigger("PlayerSkill");
                animator?.animator.SetTrigger("PlayerSkillInit");
            }
            else
            {
                animator?.animator.SetTrigger("PlayerSkill");
            }
        }

        public override void Active()
        {
            base.Active();
        }
        
        public override bool TryUse()
        {
            return base.TryUse() && (stand.GetState() == PokdoStand.PokdoState.Attack ||
                   stand.GetState() == PokdoStand.PokdoState.Idle);
        }

        public void Init(PokdoStand stand)
        {
            this.stand = stand;
            
        }

        public override void Init()
        {
            base.Init();
            actionList.Clear();
            actionList.Add(DoAttackCommand);
        }

        void DoAttackCommand()
        {
            stand.AttackCommand();
        }
    }
}