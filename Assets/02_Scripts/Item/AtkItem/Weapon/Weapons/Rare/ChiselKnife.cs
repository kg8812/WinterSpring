using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class ChiselKnife : ProjectileWeapon
    {
        private IWeaponAttack _iattack;
        public override IWeaponAttack IAttack => _iattack ??= new ChiselKnifeAttack(this);

        public override AttackCategory Category => AttackCategory.Sword;

        [Serializable]
        public struct AtkInfo
        {
            [LabelText("단검사이 각도")] public float angle;
            [LabelText("중복 히트시 데미지 감소량(%)")] public float dmgReduce;
        }
        [TabGroup("공격설정/설정","공중")]
        [SerializeField] List<AtkInfo> atkInfos;
        
        public class ChiselKnifeAttack : ProjectileAttack
        {
            private ChiselKnife chisel;
            public ChiselKnifeAttack(ChiselKnife weapon) : base(weapon)
            {
                chisel = weapon;

                OnAirFire += SetProjectile;
            }
            
            void SetProjectile(Projectile proj, int atkIndex, int order)
            {
                int half = Mathf.FloorToInt(chisel.airProjectileInfos[atkIndex].projCount / 2f);
                float angle = chisel.atkInfos[atkIndex].angle;
                float currAngle = (order - half) * angle;
                proj.Rotate(currAngle);
                proj.AddEventUntilInitOrDestroy(param =>
                {
                    if (param.target is Actor target)
                    {
                        if (target.CheckDuplicationAtk(proj))
                        {
                            proj.DmgRatio -= chisel.atkInfos[atkIndex].dmgReduce;
                        }
                    }
                });
            }
        }
        protected override void OnEquip(IMonoBehaviour user)
        {
            base.OnEquip(user);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
        }
    }
}