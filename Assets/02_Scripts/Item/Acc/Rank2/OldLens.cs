using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class OldLens : Accessory
    {
        [LabelText("쿨타임")] public float cd;
        [LabelText("데미지")] public float dmg;
        [LabelText("폭발 반경")] public float radius;
        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnCrit,Apply);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnCrit,Apply);
        }

        void Apply(EventParameters param)
        {
            if (isCd || param?.target == null) return;
            AttackObject obj = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                Define.DummyEffects.Explosion, param.target.Position);

            obj.transform.localScale = Vector3.one * (radius * 0.6f);
            obj.Init(user,new FixedAmount(dmg),1);
            GameManager.instance.StartCoroutine(CDCoroutine(cd));
        }
    }
}