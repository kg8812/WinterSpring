using System;
using chamwhy;
using Default;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Apis.SkillTree
{
    public class LilpaTree1A : SkillTree
    {
        [FormerlySerializedAs("distance")] [LabelText("상호작용 반경")] public int radius;
        [LabelText("데미지(%)")] public float dmg;
        [LabelText("지속시간")] public float duration;
        [LabelText("이펙트 반경")] public float effectRadius;
        [LabelText("접근 거리")] public float distance;
        
        private LilpaPassiveSkill skill;
        private Player player;
        [HideInInspector] public IOnHit target;
        private LilpaQuietusComponent component;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as LilpaPassiveSkill;

            if (skill == null) return;
            
            Monster.MakeInteractable(radius);
            player = skill.Player;
            Monster.CheckInteractable -= CheckInteractable;
            Monster.CheckInteractable += CheckInteractable;
            Monster.InteractEvent.RemoveListener(Execution);
            Monster.InteractEvent.AddListener(Execution);
            component = player.Mecanim.gameObject.GetOrAddComponent<LilpaQuietusComponent>();
            component.Init(this,player);
            target = null;
        }

        public override void DeActivate()
        {
            base.DeActivate();
            Monster.RemoveInteractable();
            Monster.CheckInteractable -= CheckInteractable;
            Monster.InteractEvent.RemoveListener(Execution);
            Destroy(component);
            target = null;
        }

        bool CheckInteractable(Monster monster)
        {
            return monster.SubBuffCount(SubBuffType.HunterStack) >= 5;
        }

        void Execution(Monster monster)
        {
            if (!player.AbleAttack)
            {
                return;
            }

            target = monster;
            Guid guid = player.AddInvincibility();
            player.ControlOff();
            EActorDirection dir = player.transform.position.x > monster.transform.position.x
                ? EActorDirection.Left
                : EActorDirection.Right;
            player.SetDirection(dir);
            player.transform.position = monster.transform.position + Vector3.left * (distance * (int)dir);
            player.Rb.gravityScale = 0;
            Sequence seq = DOTween.Sequence();
            Guid pauseGuid = GameManager.instance.RegisterPause();
            player.animator.SetInteger("PlayerType",(int)player.playerType);
            player.animator.SetInteger("ActiveSkillType",0);
            player.animator.ResetTrigger("PlayerSkillEnd");
            player.animator.ResetTrigger("PlayerSkill");
            player.animator.SetTrigger("PlayerSkillInit");
            
            player.animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            
            seq.SetUpdate(true);
            seq.AppendInterval(duration);
            seq.onUpdate += () =>
            {
                player.Mecanim.UpdateAnimation();
            };
            seq.AppendCallback(() =>
            {
                player.ControlOn();
                player.ActorMovement.ResetGravity();
                player.animator.SetTrigger("PlayerSkillEnd");
                
                EventParameters temp = new EventParameters(player, monster)
                {
                    atkData = new(){
                    atkStrategy =  new AtkItemCalculation(skill.user as Actor, skill,dmg + player.CritDmg),
                    attackType = Define.AttackType.Extra
                    }
                };
                player.Attack(temp);
                player.RemoveInvincibility(guid);
                player.animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
                GameManager.instance.RemovePause(pauseGuid);
                
                skill.Heal(new EventParameters(player,monster));
                monster.RemoveType(SubBuffType.HunterStack);
                target = null;
            });
        }
    }
}