using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class BlackChain : Accessory
    {
        [LabelText("사슬 데미지")] public float dmg;
        [LabelText("사슬 반경")] public float radius;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnHit,SpawnChain);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnHit,SpawnChain);
        }

        void SpawnChain(EventParameters param)
        {
            AttackObject chain = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                Define.AccessoryObjects.BlackChainEffect, user.Position);
            chain.transform.localScale = Vector3.one * (radius * 2);
            chain.Init(user,new FixedAmount(dmg),1);
            chain.AddEventUntilInitOrDestroy(ApplyBleed);
        }

        void ApplyBleed(EventParameters param)
        {
            if (param?.target is Actor target)
            {
                target.AddSubBuff(user,SubBuffType.Debuff_Bleed);
            }
        }
    }
}