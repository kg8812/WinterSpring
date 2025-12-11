using System.Collections.Generic;
using System.Linq;
using chamwhy.DataType;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class GunSword : ProjectileWeapon
    {

        private Weapon_BasicAttack swordAtk;
        private ProjectileAttack gunAtk;

        public override IWeaponAttack IAttack
        {
            get
            {
                switch (currentForm)
                {
                    case WeaponForm.Gun:
                        gunAtk ??= new ProjectileAttack(this);
                        return gunAtk;
                    case WeaponForm.Sword:
                        swordAtk ??= new Weapon_BasicAttack(this);
                        return swordAtk;
                }

                return base.IAttack;
            }
        }

        public override AttackType attackType =>
            currentForm == WeaponForm.Gun ? AttackType.Projectile : AttackType.Collider;

        private List<float> groundSword;
        private List<float> groundGun;
        private List<float> airSword;
        private List<float> airGun;
        public override List<float> GroundCancelTimes
        {
            get
            {
                return currentForm switch
                {
                    WeaponForm.Gun => groundGun ??= groundProjectileInfos.Select(x => x.cancelTime).ToList(),
                    WeaponForm.Sword => groundSword ??= groundAtkDmgs.Select(x => x.cancelTime).ToList(),
                    _ => null
                };
            }
        }

        public override List<float> AirCancelTimes
        {
            get
            {
                return currentForm switch
                {
                    WeaponForm.Gun => airGun ??= airProjectileInfos.Select(x => x.cancelTime).ToList(),
                    WeaponForm.Sword => airSword ??= airAtkDmgs.Select(x => x.cancelTime).ToList(),
                    _ => null
                };
            }
        }

        private Animator _animator;
        private Animator animator => _animator ??= GetComponent<Animator>();

        [SerializeField] [LabelText("근거리 Index")] private int dataId2;

        private WeaponDataType data2;

        public override WeaponDataType Data
        {
            get
            {
                return currentForm switch
                {
                    WeaponForm.Gun => base.Data,
                    WeaponForm.Sword => data2,
                    _ => null
                };
            }
        }

        public enum WeaponForm
        {
            Gun = 0,Sword = 1
        }

        public WeaponForm currentForm = WeaponForm.Gun;

        public override int Index
        {
            get
            {
                return currentForm switch
                {
                    WeaponForm.Gun => dataId,
                    WeaponForm.Sword => dataId2,
                    _ => dataId
                };
            }
        }

        public override int MotionIndex => Index;

        public void ChangeForm()
        {
            currentForm = currentForm == WeaponForm.Sword ? WeaponForm.Gun : WeaponForm.Sword;
            Player._overrider.SetAnimation(MotionIndex, Player);
            atlasRegion[0] = currentForm == WeaponForm.Sword ? "gunsword" : "gunswordMelee";
            animator.SetInteger("Form",(int)currentForm);
        }

        public override void Init()
        {
            currentForm = WeaponForm.Gun;

            base.Init();
            WeaponData.DataLoad.TryGetWeaponData(dataId2, out data2);
        }

        protected override void OnEquip(IMonoBehaviour user)
        {
            base.OnEquip(user);
            currentForm = WeaponForm.Gun;
            animator.Rebind();
            animator.enabled = false;
            animator.enabled = true;
        }
    }
}