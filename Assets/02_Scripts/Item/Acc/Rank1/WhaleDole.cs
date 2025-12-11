using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class WhaleDole : Accessory
    {
        [LabelText("쿨타임")] public float cd;
        [LabelText("면역 목록")] public List<SubBuffType> types;
        
        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            isCd = false;
            user.AddEvent(EventType.OnSubBuffTaken,Remove);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            
            user.RemoveEvent(EventType.OnSubBuffTaken,Remove);
        }

        void Remove(EventParameters parameters)
        {
            if (isCd) return;
            if (parameters?.buffData.takenSubBuff == null) return;

            SubBuffType type = SubBuffType.None;
            foreach (var x in types)
            {
                if (parameters.buffData.takenSubBuff.Type == x)
                {
                    type = x;
                    break;
                }
            }

            if (type != SubBuffType.None)
            {
                user.RemoveType(type,1);
                GameManager.instance.StartCoroutineWrapper(CDCoroutine(cd));
            }
        }
    }
}