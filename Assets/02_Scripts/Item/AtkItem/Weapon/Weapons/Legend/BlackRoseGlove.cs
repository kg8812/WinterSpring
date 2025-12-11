using System.Collections.Generic;
using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class BlackRoseGlove : Orb
    {
        private BlackRoseAtk iatk;

        //[ValidateInput("ValidateSize", "공격설정과 개수를 맞춰주세요")]

        //[LabelText("풍압 정보")] public List<BlackRoseAtk.WindInfo> WindInfos;
        
        bool ValidateSize(List<BlackRoseAtk.WindInfo> _sizes)
        {
            return _sizes.Count == groundAtkDmgs.Count;
        }
        public class BlackRoseAtk : Weapon_BasicAttack
        {
            [System.Serializable]
            public struct WindInfo
            {
                [LabelText("크기")] public Vector2 size;
                [LabelText("데미지")] public float dmg;
                [LabelText("그로기 계수")] public float groggy;
            }
            private BlackRoseGlove glove;
            private List<WindInfo> windInfos;
            public BlackRoseAtk(BlackRoseGlove weapon,List<WindInfo> windInfos) : base(weapon)
            {
                glove = weapon;
                this.windInfos = windInfos;
            }

            public override void GroundAttack(int index)
            {
                base.GroundAttack(index);

                AttackObject wind = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                    "Prefabs/AttackObjects/WindWave",
                    player.Position + Vector3.right * ((int)player.Direction * (windInfos[index].size.x / 2)));

                wind.transform.localScale = windInfos[index].size;
                wind.Init(player, new AtkItemCalculation(player, weapon, windInfos[index].dmg),1);
                wind.Init(weapon.CalculateGroggy(windInfos[index].groggy));
            }
        }
    }
}