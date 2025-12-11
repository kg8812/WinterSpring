using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class NobilityParasol : Accessory
    {
        [LabelText("쿨타임")] public float cd;
        [LabelText("발동 체력 %")] public float applyHp;
        [LabelText("체력 회복 %(최대체력 기반 %)")][Tooltip("입력한 수치까지 회복됩니다")] public float healAmount;
        [LabelText("신체 증가량")] public float body;
        [LabelText("영혼 증가량")] public float spirit;
        [LabelText("기량 증가량")] public float finesse;

        private BonusStat stat;

        BonusStat StatEvent()
        {
            stat ??= new();
            stat.SetValue(ActorStatType.Body,isCd ? 0 : body);
            stat.SetValue(ActorStatType.Spirit,isCd ? 0 : spirit);
            stat.SetValue(ActorStatType.Finesse,isCd ? 0 : finesse);

            return stat;
        }
        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.BonusStatEvent += StatEvent;
            user.AddEvent(EventType.OnUpdate,Heal);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.BonusStatEvent -= StatEvent;
            user.RemoveEvent(EventType.OnUpdate,Heal);
        }

        void Heal(EventParameters param)
        {
            if (isCd || user.CurHp / user.MaxHp > applyHp / 100f) return;
            GameManager.instance.StartCoroutine(CDCoroutine(cd));
            
            user.CurHp = user.MaxHp / 100f * healAmount;
        }
    }
}