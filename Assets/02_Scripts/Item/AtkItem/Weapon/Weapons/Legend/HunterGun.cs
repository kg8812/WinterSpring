using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class HunterGun : ProjectileWeapon
    {
        private IWeaponAttack iAttack;

        public override IWeaponAttack IAttack => iAttack ??= new ProjectileAttack(this);

        [HideInInspector]public HunterGunSummon eagle;
        [Title("맹금류 설정")]
        [LabelText("데미지")] public float dmg;
        [LabelText("공격주기")] public float atkTime;
        [LabelText("공격횟수")] public int atkCount;
        protected override void OnEquip(IMonoBehaviour user)
        {
            base.OnEquip(user);
            eagle = GameManager.Factory.Get<HunterGunSummon>(FactoryManager.FactoryType.Normal, "HunterGunSummon",
                GameManager.instance.ControllingEntity.Position + new Vector3(-(int)GameManager.instance.ControllingEntity.Direction, 1));
            
            eagle.SetMaster(Player);
            eagle.dmg = dmg;
            eagle.atkTime = atkTime;
            eagle.atkCount = atkCount;
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            GameManager.Factory.Return(eagle.gameObject);
        }
    }
}