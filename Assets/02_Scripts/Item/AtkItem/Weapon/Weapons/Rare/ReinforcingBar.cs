using System;
using System.Collections.Generic;
using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class ReinforcingBar : Weapon
    {
        [Serializable]
        public struct FragmentInfo
        {
            [LabelText("투사체 설정")] public ProjectileInfo info;
            [LabelText("그로기 계수")] public float groggy;
        }
        
        bool ValidateSize(List<FragmentInfo> list)
        {
            return list.Count == groundAtkDmgs.Count;
        }
        bool ValidateSize2(List<FragmentInfo> list)
        {
            return list.Count == airAtkDmgs.Count;
        }
        [ValidateInput("ValidateSize", "타수를 맞춰주세요")]
        [TabGroup("공격설정/설정","지상")]
        [LabelText("파편 설정")] public List<FragmentInfo> info;
        [ValidateInput("ValidateSize2", "타수를 맞춰주세요")]
        [TabGroup("공격설정/설정","공중")]
        [LabelText("파편 설정")] public List<FragmentInfo> airInfo;

        private IWeaponAttack iatk;

        public override IWeaponAttack IAttack => iatk??=new ReinforcingAttack(this);

        class ReinforcingAttack : Weapon_BasicAttack
        {
            private new ReinforcingBar weapon;
            
            public ReinforcingAttack(ReinforcingBar weapon) : base(weapon)
            {
                this.weapon = weapon;
            }

            public override void GroundAttack(int index)
            {
                base.GroundAttack(index);
                AttackObject atk =
                    GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect, "StoneFragments",player.transform.position + Vector3.right * (int)player.Direction);
                atk.Init(player,new AtkItemCalculation(player,weapon,weapon.info[index].info.dmg),1);
                atk.Init(weapon.info[index].info);
                atk.Init(weapon.CalculateGroggy(weapon.info[index].groggy));
            }

            public override void AirAttack(int index)
            {
                base.AirAttack(index);
                AttackObject atk =
                    GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect, "StoneFragments",player.transform.position + Vector3.right * (int)player.Direction);
                atk.Init(player,new AtkItemCalculation(player,weapon,weapon.airInfo[index].info.dmg),1);
                atk.Init(weapon.airInfo[index].info);
                atk.Init(weapon.CalculateGroggy(weapon.airInfo[index].groggy));
            }
        }
    }
}