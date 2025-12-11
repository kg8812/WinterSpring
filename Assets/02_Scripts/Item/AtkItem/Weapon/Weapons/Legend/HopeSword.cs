
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis
{
    public class HopeSword : Weapon
    {
        [LabelText("달빛수정 투사체정보")] public ProjectileInfo info;
        [LabelText("발사 개수")] public int projCount;
        [LabelText("달빛수정 반경")] public float radius;
        protected override void OnEquip(IMonoBehaviour user)
        {
            base.OnEquip(user);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
        }

        void SpawnMoonLight(EventParameters param)
        {
            for (int i = 0; i < projCount; i++)
            {
                Projectile moon = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.Effect,
                    Define.PlayerEffect.Ine_Magic_1Circle_Shard, GameManager.instance.ControllingEntity.Position);
                moon.Init(Player,new FixedAmount(info.dmg),10);
                moon.Init(info);
                moon.Fire();
                float randAngle = Random.Range(-45f, 45f);
                moon.Rotate(180 + randAngle);
                moon.transform.localScale = Vector3.one * (2 * radius);
                
            }
        }

        public override void SetGroundCollider(int idx, AttackObject[] attackObjs)
        {
            base.SetGroundCollider(idx, attackObjs);
            Invoke(idx);
        }

        public override void SetAirCollider(int idx, AttackObject[] attackObjs)
        {
            base.SetAirCollider(idx, attackObjs);
            Invoke(idx);
        }

        void Invoke(int combo)
        {
            Player.attackColliders.ForEach(x =>
            {
               x.AddAtkEventOnce(SpawnMoonLight);
            });
        }
    }
}