using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class DisciplineGloves : Accessory
    {
        [LabelText("폭발 반경")] public float radius;
        [LabelText("영혼 계수")] public float dmg;
        [LabelText("화상 확률")] public float probability;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnKill,SpawnExplosion);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnKill,SpawnExplosion);
        }

        void SpawnExplosion(EventParameters param)
        {
            if (param?.target is Actor target && target.Contains(SubBuffType.Debuff_Burn))
            {
                var explosion = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                    Define.DummyEffects.Explosion, target.Position);
                explosion.transform.localScale = Vector3.one * (0.6f * radius);
                explosion.Init(user,new StatBase(user,ActorStatType.Spirit,0,dmg),1);
                explosion.AddEventUntilInitOrDestroy(ApplyBurn);
            }
        }

        void ApplyBurn(EventParameters param)
        {
            float rand = Random.Range(0, 100f);
            if (rand > probability) return;

            if (param?.target is Actor target)
            {
                target.AddSubBuff(user,SubBuffType.Debuff_Burn);
            }
        }
    }
}