using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class PlagueAxe : HitImmuneWeapon
    {
        [LabelText("독성 폭발 데미지")] public float dmg;
        [LabelText("독성 폭발 그로기 계수")] public float poisonGroggy;
        
        private IWeaponAttack iAttack;
        public override IWeaponAttack IAttack => iAttack ??= new PlageAxeAtk(this);

        class PlageAxeAtk : Weapon_BasicAttack
        {
            private PlagueAxe axe;
            public PlageAxeAtk(PlagueAxe weapon) : base(weapon)
            {
                axe = weapon;
            }

            public override void GroundAttack(int index)
            {
                base.GroundAttack(index);

                if (index == 1)
                {
                    var exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                        "PlagueExplosion", weapon.transform.position + Vector3.right * ((int)player.Direction * 2));
                    exp.transform.localScale = Vector3.one * 0.6f;
                    exp.Init(player, new AtkItemCalculation(player, axe, axe.dmg), 1);
                    exp.Init(weapon.CalculateGroggy(axe.poisonGroggy));
                    exp.AddEventUntilInitOrDestroy(AddPoison);
                }
            }

            public override void AirAttack(int index)
            {
                base.AirAttack(index);
                if (index == 1)
                {
                    var exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                        "PlagueExplosion", weapon.transform.position + Vector3.right * ((int)player.Direction * 2));
                    exp.transform.localScale = Vector3.one * 0.6f;
                    exp.Init(player, new AtkBase(player, axe.dmg), 1);
                    exp.Init(weapon.CalculateGroggy(axe.poisonGroggy));
                    exp.AddEventUntilInitOrDestroy(AddPoison);
                }
            }

            void AddPoison(EventParameters parameters)
            {
                if (parameters?.target is Actor t)
                {
                    t.AddSubBuff(player, SubBuffType.Debuff_Poison);
                }
            }
        }
    }
}