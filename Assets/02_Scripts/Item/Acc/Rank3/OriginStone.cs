using System.Collections;
using System.Collections.Generic;
using System.Linq;
using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class OriginStone : Accessory
    {
        [LabelText("화상 탐색 반경")] public float radius;
        [LabelText("1명당 영혼 증가량")] public float amount;
        [LabelText("최대 영혼 증가량")] public float maxAmount;

        private int count;
        
        private BonusStat _bonus;
        BonusStat Bonus => _bonus ??= new();
        BonusStat StatEvent()
        {
            Bonus.SetValue(ActorStatType.Spirit, Mathf.Clamp(amount * count,0,maxAmount));
            return Bonus;
        }

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.BonusStatEvent += StatEvent;
            user.AddEvent(EventType.OnUpdate,SearchTargets);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.BonusStatEvent -= StatEvent;
            user.RemoveEvent(EventType.OnUpdate,SearchTargets);
        }

        void SearchTargets(EventParameters param)
        {
            var targets = user.gameObject.GetTargetsInCircle(radius, LayerMasks.Enemy).OfType<IBuffUser>().Where(
                x => x.SubBuffManager.Contains(SubBuffType.Debuff_Burn));
            count = targets.Count();
        }
    }
}