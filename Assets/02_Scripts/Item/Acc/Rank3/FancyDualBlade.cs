using System.Collections;
using System.Collections.Generic;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class FancyDualBlade : Accessory
    {
        [LabelText("쿨타임")] public float cd;
        [LabelText("공격 설정")] public ProjectileInfo projectileInfo;
        [LabelText("공격 반경")] public float radius;
        [LabelText("기량 계수 (%)")] public float finesseAmount;
        [LabelText("공격속도 적용값 (%)")] public float atkSpeed;
        [LabelText("공격속도 적용값당 쿨타임 감소량 (고정)")] public float cdReduce;

        private float CD => cd - user.AtkSpeed * atkSpeed / 100f * cdReduce;
        public void Attack()
        {
            AttackObject effect = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                Define.AccessoryObjects.FancyDualBladeEffect, user.Position);
            effect.Init(user,new StatBase(user,ActorStatType.Finesse,projectileInfo.dmg,finesseAmount),projectileInfo.duration);
            effect.Init(projectileInfo);
            effect.gameObject.SetRadius(radius);
        }

        void CheckCd(EventParameters _)
        {
            if (isCd) return;
            StartCoroutine(CDCoroutine(CD));
            Attack();
        }
        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnUpdate,CheckCd);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnUpdate,CheckCd);
        }
    }
}