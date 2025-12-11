using System.Collections;
using System.Collections.Generic;
using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class StiffLetter : Accessory
    {
        [LabelText("원념 획득 증가량(%)")] public float goldRate;

        private BonusStat stat;

        BonusStat StatEvent()
        {
            stat ??= new();
            stat.SetValue(ActorStatType.GoldRate,goldRate);
            return stat;
        }

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.BonusStatEvent += StatEvent;
            user.AddEvent(EventType.OnHitReaction,DropGold);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.BonusStatEvent -= StatEvent;
            user.RemoveEvent(EventType.OnHitReaction,DropGold);
        }

        void DropGold(EventParameters param)
        {
            var goldDrop = GameManager.Item.Gold.CreateNew(Gold_Drop.LobbyGold5Id);
            goldDrop.transform.position = user.Position;
            goldDrop.Dropping();
        }
    }
}