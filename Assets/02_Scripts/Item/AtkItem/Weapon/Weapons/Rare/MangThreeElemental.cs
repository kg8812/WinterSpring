using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Apis
{
    class MangThreeElemental : Orb
    {
        [Title("정령 변수")]
        [LabelText("화상 확률")] public float burnProb;
        [LabelText("냉기 확률")] public float chillProb;
        [LabelText("냉기 지속시간")] public float chillDuration;
        [LabelText("중독 확률")] public float poisonProb;

        public enum SpiritType
        {
            Pink,Blue,Green
        }

        [HideInInspector] public SpiritType spiritType;
        [HideInInspector] public GameObject curSpirit;
        
        protected override void OnEquip(IMonoBehaviour user)
        {
            base.OnEquip(user);

            if (curSpirit != null)
            {
                GameManager.Factory.Return(curSpirit);
            }
            
            Player.AddEvent(EventType.OnBasicAttack,Invoke);
            SetSpirit(SpiritType.Pink);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            Player.RemoveEvent(EventType.OnBasicAttack,Invoke);
            if (curSpirit != null)
            {
                GameManager.Factory.Return(curSpirit);
                curSpirit = null;
            }
        }

        void Invoke(EventParameters parameters)
        {
            SubBuffType subType;
            float prob;
            
            switch (spiritType)
            {
                case SpiritType.Pink:
                    subType = SubBuffType.Debuff_Burn;
                    prob = burnProb;
                    break;
                case SpiritType.Blue:
                    subType = SubBuffType.Debuff_Chill;
                    prob = chillProb;
                    break;
                case SpiritType.Green:
                    subType = SubBuffType.Debuff_Poison;
                    prob = poisonProb;
                    break;
                default:
                    return;
            }

            float rand = Random.Range(0f, 100f);
            if (parameters?.target is Actor target && prob >= rand)
            {
                target.AddSubBuff(Player,subType);
            }
        }
        public void SetSpirit(SpiritType type)
        {
            spiritType = type;

            if (curSpirit != null)
            {
                GameManager.Factory.Return(curSpirit);
            }

            curSpirit = type switch
            {
                SpiritType.Pink => GameManager.Factory.Get(FactoryManager.FactoryType.Normal, "PinkSpirit",
                    GameManager.instance.ControllingEntity.Position),
                SpiritType.Blue => GameManager.Factory.Get(FactoryManager.FactoryType.Normal, "BlueSpirit",
                    GameManager.instance.ControllingEntity.Position),
                SpiritType.Green => GameManager.Factory.Get(FactoryManager.FactoryType.Normal, "GreenSpirit",
                    GameManager.instance.ControllingEntity.Position),
                _ => curSpirit
            };

            CustomBoneFollower follower = SpineUtils.AddCustomBoneFollower(Player.Mecanim,"center",curSpirit);
            follower.offset = Player.topPivot;
        }
    }
}
