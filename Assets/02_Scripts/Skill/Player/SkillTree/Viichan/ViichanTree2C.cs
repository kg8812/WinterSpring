using System.Collections.Generic;
using Command;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Apis.SkillTree
{
    public class ViichanTree2C : ViichanTree
    {
        private ViichanActiveSkill skill;

        [LabelText("반격 지속시간")] public float duration;
        [LabelText("반격 TimeScale")] public float timeScale;
        [LabelText("반격 범위")] public Vector2 atkArea;
        [LabelText("반격 데미지(%)")] public float dmg;
        [LabelText("반격 그로기 수치")] public int groggy;

        [AssetsOnly][Required] public List<ActorCommand> counterAtkCommand;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            
            skill = active as ViichanActiveSkill;

            if (counterAtkCommand[0] is CounterAttack counterAtk)
            {
                counterAtk.Init(this);
            }

            if (skill == null) return;
            skill.OnParrying -= Slower;
            skill.OnParrying += Slower;
            Debug.Log("Activate");
        }

        public override void DeActivate()
        {
            base.DeActivate();
            if (skill != null)
            {
                skill.OnParrying -= Slower;
            }
        }

        private Sequence seq;
        void Slower()
        {
            Time.timeScale = timeScale;
            skill.Player.Controller.Executors[Define.GameKey.Attack1].keyDownCommand.Commands = counterAtkCommand;
            seq = DOTween.Sequence();
            seq.SetAutoKill(false);
            seq.SetDelay(duration);
            seq.SetUpdate(true);
            seq.AppendCallback(() =>
            {
                Time.timeScale = 1;
                skill.Player.Controller.Executors[Define.GameKey.Attack1].keyDownCommand.Commands = skill.Player.Controller
                    .BaseCommands.Commands[Define.GameKey.Attack1].keyDownCommand;
            });
            seq.onKill += () =>
            {
                Time.timeScale = 1;
                skill.Player.Controller.Executors[Define.GameKey.Attack1].keyDownCommand.Commands = skill.Player.Controller
                    .BaseCommands.Commands[Define.GameKey.Attack1].keyDownCommand;
            };
        }

        public void Resume()
        {
            seq?.Kill();
        }
    }
}