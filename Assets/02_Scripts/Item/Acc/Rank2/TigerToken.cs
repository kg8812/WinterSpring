using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class TigerToken : Accessory
    {
        [LabelText("고드름 발사 갯수")] public int count;
        [LabelText("발사 반경")] public float radius;
        [LabelText("고드름 투사체 설정")] public ProjectileInfo projInfo;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user?.AddEvent(EventType.OnKill,SpawnIcicles);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnKill,SpawnIcicles);
        }

        void SpawnIcicles(EventParameters param)
        {
            if (param?.target is not Actor target || !target.Contains(SubBuffType.Debuff_Frozen)) return;
            
            List<Projectile> icicles = GameManager.Factory.SpawnProjectilesInCircle(Define.AccessoryObjects.Icicle,
                param.target.Position, count, radius);
            
            icicles.ForEach(x =>
            {
                x.Init(user,new FixedAmount(projInfo.dmg));
                x.Init(projInfo);
                x.AddEventUntilInitOrDestroy(ApplyChill);
                x.LookAt(x.Position - target.Position);
                x.Fire(false);
            });
        }

        void ApplyChill(EventParameters param)
        {
            if (param?.target is not Actor target) return;
            target.AddSubBuff(user,SubBuffType.Debuff_Chill);
        }
    }
}