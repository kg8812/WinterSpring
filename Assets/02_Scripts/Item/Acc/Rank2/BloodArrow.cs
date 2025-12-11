using System.Collections;
using System.Collections.Generic;
using PlayerState;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class BloodArrow : Accessory
    {
        [LabelText("쿨타임")] public float cd;
        [LabelText("화살 투사체 설정")] public ProjectileInfo info;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnAttack,SpawnArrow);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnAttack,SpawnArrow);
        }

        void SpawnArrow(EventParameters param)
        {
            if (isCd) return;
            GameManager.instance.StartCoroutine(CDCoroutine(cd));
            Projectile arrow = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject,
                Define.AccessoryObjects.BloodArrowEffect, user.Position);
            
            arrow.Init(user,new FixedAmount(info.dmg));
            arrow.Init(info);
            arrow.AddEventUntilInitOrDestroy(ApplyBleed);
            arrow.Fire(true);
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