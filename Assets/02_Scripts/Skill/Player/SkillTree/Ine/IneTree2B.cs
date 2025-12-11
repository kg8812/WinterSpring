using chamwhy;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

namespace Apis.SkillTree
{
    public class IneTree2B : SkillTree
    {
        private IneActiveSkill skill;
        private IneActiveAttachment _attachment;

        [LabelText("서클1 마나 증가량")] public int amount1;
        [LabelText("서클2 추적범위")] public float radius2;
        [LabelText("서클2 이동속도")] public float speed2;
        [LabelText("서클3 반경 증가량(%)")] public float radius3;
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as IneActiveSkill;

            if (skill != null)
            {
                _attachment ??= new IneActiveAttachment(new IneActiveStat()
                {
                    manaGain = amount1, circle3ExpRadiusRatio = radius3
                });
                
                skill.AddAttachment(_attachment);
                skill.OnCircle2Spawn += AddFollower;
            }
        }

        public override void DeActivate()
        {
            base.DeActivate();
            if (skill == null) return;
            
            skill.RemoveAttachment(_attachment);
            skill.OnCircle2Spawn -= AddFollower;
        }

        void AddFollower(AttackObject obj)
        {
            Circle2Follower follower = obj.GetOrAddComponent<Circle2Follower>();
            follower.Init(speed2,radius2);
            obj.AddEventUntilInitOrDestroy(RemoveFollower,EventType.OnDestroy);
        }

        void RemoveFollower(EventParameters parameters)
        {
            if (parameters?.user != null && !parameters.user.gameObject.activeSelf)
            {
                DestroyImmediate(parameters.user.gameObject.GetComponent<Circle2Follower>());
            }
        }
    }
}