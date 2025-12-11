using Default;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "BeastGrabSkill", menuName = "Scriptable/Skill/BeastGrabSkill")]

    public class BeastGrabSkill : ActiveSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        [TitleGroup("스탯값")] [LabelText("사정거리")]
        public float distance;
        
        public override void Active()
        {
            base.Active();
            GameManager.instance.Player.SpawnGrab(0.2f,distance,Atk);
        }
    }
}