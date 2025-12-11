using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class ThirtySixToken : Accessory
    {
        [LabelText("기본 데미지")] public float dmg;
        [LabelText("단검 투사체 설정")] public ProjectileInfo info;
        
        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnAttack,SpawnKnife);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnAttack,SpawnKnife);
        }

        void SpawnKnife(EventParameters param)
        {
            Projectile knife = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject,
                Define.PlayerSkillObjects.LilpaDagger, user.Position);
            knife.Init(user,new StatBase(user,ActorStatType.Finesse,dmg,info.dmg));
            knife.Init(info);
            knife.Fire();
        }
    }
}