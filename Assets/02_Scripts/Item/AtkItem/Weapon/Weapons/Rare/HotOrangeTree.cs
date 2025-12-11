
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class HotOrangeTree : Weapon
    {
        public struct OrangeInfo
        {
            [LabelText("지속시간")] public float duration;
            [LabelText("방어력 증가량")]public float amount;
            [LabelText("버프 최대스택")] public int maxStack;
            [LabelText("잔재 획득량")] public int goldAmount;
        }
        [LabelText("데운귤 드랍확률")] public float orangeProb;
        [LabelText("데운귤 설정")] public OrangeInfo orangeInfo;
        
        protected override void OnEquip(IMonoBehaviour user)
        {
            base.OnEquip(user);
            Player.AddEvent(EventType.OnBasicAttack,SpawnOrange);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            Player.RemoveEvent(EventType.OnBasicAttack,SpawnOrange);
        }

        void SpawnOrange(EventParameters parameters)
        {
            float rand = Random.Range(0, 100f);
            if (rand >= orangeProb)
            {
                HotOrange orange = GameManager.Factory.Get<HotOrange>(FactoryManager.FactoryType.Normal, "HotOrange",
                    GameManager.instance.ControllingEntity.Position);
                orange.Init(orangeInfo);
                orange.Dropping();
            }
        }
    }
}