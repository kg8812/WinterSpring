using Sirenix.OdinInspector;
using Sirenix.Utilities;

namespace Apis.SkillTree
{
    public class LilpaTree3C : SkillTree
    {
        private Player player;

        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("추가 부여량")] public int count;
        }

        public DataStruct[] datas;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            player = GameManager.instance.Player;
            player.AddEvent(EventType.OnColliderAttack,AddHunterStack);
           
        }

        public override void DeActivate()
        {
            base.DeActivate();
            player.RemoveEvent(EventType.OnColliderAttack,AddHunterStack);
        }

        void AddHunterStack(EventParameters parameters)
        {
            if (parameters is { target: Actor target, hitData: { isCritApplied: true } })
            {
                for (int i = 0; i < datas[level - 1].count; i++)
                {
                    target.AddSubBuff(player, SubBuffType.HunterStack);
                }
            }
        }
    }
}