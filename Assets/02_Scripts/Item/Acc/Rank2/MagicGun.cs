using System.Collections;
using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class MagicGun : Accessory
    {
        [LabelText("투사체 설정")] public ProjectileInfo info;
        [LabelText("쿨타임")] public float cd;

        void FireBullet(EventParameters _)
        {
            if (isCd) return;
            GameManager.instance.StartCoroutineWrapper(CDCoroutine(cd));
            Projectile bullet =
                GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject, "bullet", user.Position);
            
            bullet.Init(user,new FixedAmount(info.dmg));
            bullet.Init(info);
            bullet.Fire();
        }

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnSkill,FireBullet);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnSkill,FireBullet);
        }
    }
}