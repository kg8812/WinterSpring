using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class ViichanTree2B : ViichanTree
    {
        [LabelText("돌진 거리")] public float distance;
        [LabelText("돌진 속도")] public float speed;
        [LabelText("돌진 Ease")] public Ease ease;
        [LabelText("충돌 공격설정")] public ProjectileInfo atkInfo;
        [LabelText("충돌 크기")] public Vector2 size;
        [LabelText("충돌 시 방패 소모량 (%)")] public float shieldUse;
        
        private ViichanActiveSkill skill;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as ViichanActiveSkill;

            if (skill == null) return;

            if (skill.IsShield)
            {
                skill.Player.SetDash(new Player.ViichanShieldDash(skill.Player,skill,this));
            }

            skill.Player.RemoveEvent(EventType.OnDash,skill.InvokeUse);
            skill.cancelTypes.Remove(EventType.OnDash);
            skill.OnShield -= SetShieldDash;
            skill.OnShield += SetShieldDash;
            skill.OnShieldEnd -= SetDashToNormal;
            skill.OnShieldEnd += SetDashToNormal;
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill != null)
            {
                skill.OnShield -= SetShieldDash;
                skill.OnShieldEnd -= SetDashToNormal;
                SetDashToNormal();
                if (skill.IsShield)
                {
                    skill.Player.AddEvent(EventType.OnDash,skill.InvokeUse);
                }

                skill.cancelTypes.Add(EventType.OnDash);
            }
        }

        void SetShieldDash()
        {
            skill.Player.SetDash(new Player.ViichanShieldDash(skill.Player,skill,this));
        }

        void SetDashToNormal()
        {
            skill.Player.SetDashToNormal();
        }
    }
}