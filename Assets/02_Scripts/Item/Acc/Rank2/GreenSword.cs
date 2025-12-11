using System.Collections;
using System.Collections.Generic;
using chamwhy;
using Default;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class GreenSword : Accessory
    {
        [LabelText("쿨타임")] public float cd;
        [LabelText("데미지")] public float dmg;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnUpdate,UpdateEvent);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnUpdate,UpdateEvent);
        }

        void UpdateEvent(EventParameters _)
        {
            if (!isCd)
            {
                Check();
                GameManager.instance.StartCoroutine(CDCoroutine(cd));
            }
        }
        void Check()
        {
            var x = Utils.GetTargetsInDisplay(LayerMasks.Enemy);
            x.ForEach(y =>
            {
                if (y is not Actor t) return;
                if (!t.Contains(SubBuffType.Debuff_Bleed)) return;
                
                GameObject sword = GameManager.Factory.Get(FactoryManager.FactoryType.Normal,
                    Define.PlayerEffect.ViichanSwordEffect, y.Position + Vector3.up);
                
                sword.transform.DOMoveY(-1, 1f).SetRelative().SetEase(Ease.OutQuad).onComplete += () =>
                {
                    user.Attack(new EventParameters(user, y)
                    {
                        atkData = new()
                        {
                            dmg = dmg, atkStrategy = new FixedAmount(dmg), attackType = Define.AttackType.Extra
                        }
                    });
                    t.RemoveType(SubBuffType.Debuff_Bleed);
                    GameManager.Factory.Return(sword);
                };
            });
        }
    }
}