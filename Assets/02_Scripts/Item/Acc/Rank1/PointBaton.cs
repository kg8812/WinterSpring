using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class PointBaton : Accessory
    {
        [LabelText("참격 설정")] public ProjectileInfo atkInfo;
        
        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user?.AddEvent(EventType.OnAttack,SpawnSlash);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnAttack,SpawnSlash);
        }

        void SpawnSlash(EventParameters param)
        {
            AttackObject slash = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                Define.WeaponEffects.MagicSlash,user.Position);
            slash.transform.localScale = new Vector3((int)user.Direction, 1, 1);
            slash.Init(user,new AtkBase(user,atkInfo.dmg),1);
            slash.Init(atkInfo);
        }
        
    }
}