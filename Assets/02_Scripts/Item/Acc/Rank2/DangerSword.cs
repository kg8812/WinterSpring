using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class DangerSword : Accessory
    {
        [LabelText("공격속도 증가량")] public float atkSpeed;
        [LabelText("검기 투사체 설정")] public ProjectileInfo info;
        [LabelText("필요 공격 횟수")] public int needCount;

        private BonusStat stat;
        private int count = 0;
        
        BonusStat StatEvent()
        {
            stat ??= new();
            stat.SetValue(ActorStatType.AtkSpeed,atkSpeed);
            return stat;
        }

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnAttack,SpawnSlash);
            user.BonusStatEvent += StatEvent;
            count = 0;
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnAttack,SpawnSlash);
            user.BonusStatEvent -= StatEvent;
        }

        void SpawnSlash(EventParameters param)
        {
            count++;
            if (count < needCount) return;
            count = 0;
            Projectile proj = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject, "SwordAura",
                user.Position);
            proj.Init(user,new AtkBase(user,info.dmg));
            proj.Init(info);
            proj.Fire();
        }
    }
}