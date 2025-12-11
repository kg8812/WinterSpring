using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class OriginFlame : Accessory
    {
        [LabelText("쿨타임")] public float cd;
        [LabelText("폭발 반경")] public float radius;
        [LabelText("기본 데미지")] public float baseDmg;
        [LabelText("영혼 계수")] public float dmg;
        [LabelText("화상 확률")] public float probability;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnHit, SpawnExplosion);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnHit, SpawnExplosion);
        }

        void SpawnExplosion(EventParameters param)
        {
            if (isCd) return;
            GameManager.instance.StartCoroutine(CDCoroutine(cd));
            var explosion = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                Define.DummyEffects.Explosion, user.Position);
            explosion.transform.localScale = Vector3.one * (0.6f * radius);
            explosion.Init(user, new StatBase(user, ActorStatType.Spirit, baseDmg, dmg), 1);
            explosion.AddEventUntilInitOrDestroy(ApplyBurn);
        }

        void ApplyBurn(EventParameters param)
        {
            float rand = Random.Range(0, 100f);
            if (rand > probability) return;

            if (param?.target is Actor target)
            {
                target.AddSubBuff(user, SubBuffType.Debuff_Burn);
            }
        }
    }
}