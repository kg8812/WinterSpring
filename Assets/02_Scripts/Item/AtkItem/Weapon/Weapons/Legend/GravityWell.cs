using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class GravityWell : ProjectileWeapon
    {
        private IWeaponAttack iattack;
        public override IWeaponAttack IAttack => iattack??=new GravityWellAtk(this);

        [LabelText("산탄 크기")] public Vector2 size;
        public override AttackCategory Category => AttackCategory.Gun;

        class GravityWellAtk : Weapon_BasicAttack
        {
            private new GravityWell weapon; 
            public GravityWellAtk(GravityWell weapon) : base(weapon)
            {
                this.weapon = weapon;
            }

            public override void GroundAttack(int idx)
            {
                base.GroundAttack(idx);
                Sequence seq = DOTween.Sequence();
            
                for (int i = 0; i < weapon.groundProjectileInfos[idx].projCount; i++)
                {
                    AttackObject proj = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                        "StarShot", weapon.FirePos);
                    proj.gameObject.SetActive(false);
                    Vector2 size = weapon.size;
                    size.x *= (int)player.Direction;
                    proj.transform.localScale = size;
                    seq.AppendCallback(() =>
                    {
                        proj.gameObject.SetActive(true);
                        proj.Init(player, new AtkItemCalculation(player,weapon, weapon.groundProjectileInfos[idx].info.dmg),1);
                        proj.Init(weapon.groundProjectileInfos[idx].info);
                        proj.Init(Mathf.RoundToInt(weapon.BaseGroggyPower * weapon.groundProjectileInfos[idx].groggy / 100));
                        weapon.OnAtkObjectInit.Invoke(new EventParameters(proj));
                    });
                    seq.AppendInterval(0.1f);
                }
            }

            public override void AirAttack(int idx)
            {
                base.AirAttack(idx);
                Sequence seq = DOTween.Sequence();
            
                for (int i = 0; i < weapon.airProjectileInfos[idx].projCount; i++)
                {
                    AttackObject proj = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                        "StarShot", weapon.FirePos);
                    proj.gameObject.SetActive(false);
                    Vector2 size = weapon.size;
                    size.x *= (int)player.Direction;
                    proj.transform.localScale = size;
                    seq.AppendCallback(() =>
                    {
                        proj.gameObject.SetActive(true);
                        proj.Init(player, new AtkItemCalculation(player, weapon,weapon.airProjectileInfos[idx].info.dmg),0.5f);
                        proj.Init(weapon.airProjectileInfos[idx].info);
                        proj.Init(Mathf.RoundToInt(weapon.BaseGroggyPower * weapon.airProjectileInfos[idx].groggy / 100));
                    });
                    seq.AppendInterval(0.1f);
                }
            }
        }
       
    }
}