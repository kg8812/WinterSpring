using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class FrozenFlag : HitImmuneWeapon
    {
        [System.Serializable]
        public struct ThornInfo
        {
            [LabelText("데미지")] public float dmg;
            [LabelText("크기")] public Vector2 size;
            [LabelText("소환 개수")] public int count;
            [LabelText("그로기 계수")] public float groggy;
        }

        [TitleGroup("얼어붙은 깃발 설정")] [TabGroup("얼어붙은 깃발 설정/공격설정", "지상공격")] [LabelText("가시 설정")]
        public List<ThornInfo> list1 = new();

        [TabGroup("얼어붙은 깃발 설정/공격설정", "지상공격")] [LabelText("첫 소환 거리")]
        public float distance1;

        [TabGroup("얼어붙은 깃발 설정/공격설정", "지상공격")] [LabelText("소환 거리간격")]
        public float padding;

        [TabGroup("얼어붙은 깃발 설정/공격설정", "지상공격")] [LabelText("소환 시간간격")]
        public float timePadding;
        [TabGroup("얼어붙은 깃발 설정/공격설정","공중공격")] [LabelText("가시 설정")]
        public List<ThornInfo> list2 = new();

        private FrozenFlagAtk iatk;
        public override AttackCategory Category => AttackCategory.GreatSword;

        public override IWeaponAttack IAttack => iatk ??= new(this, list1, list2);

        public class FrozenFlagAtk : Weapon_BasicAttack
        {
            private readonly List<ThornInfo> ground;
            private readonly List<ThornInfo> air;

            private FrozenFlag flag;

            public FrozenFlagAtk(Weapon weapon, List<ThornInfo> ground, List<ThornInfo> air) : base(weapon)
            {
                this.ground = ground;
                this.air = air;
                flag = weapon as FrozenFlag;
            }

            public override void GroundAttack(int index)
            {
                base.GroundAttack(index);

                Sequence seq = DOTween.Sequence();
                
                for (int i = 0; i < ground[index].count; i++)
                {
                    var i1 = i;
                    seq.AppendCallback(() =>
                    {
                        BeamEffect thorn1 = GameManager.Factory.Get<BeamEffect>(
                            FactoryManager.FactoryType.AttackObject, "FrozenFlagThorn",
                            player.transform.position + Vector3.right * ((int)player.Direction * flag.padding * i1) +
                            Vector3.right * (flag.distance1 * (int)player.Direction));

                        thorn1.Init(new(0.25f, ground[index].size.y, ground[index].size.x, Ease.Linear,
                            BeamEffect.FireDir.Vertical));
                        thorn1.Init(player, new AtkItemCalculation(player, weapon,ground[index].dmg), 1);
                        thorn1.Init(weapon.CalculateGroggy(ground[index].groggy));
                        thorn1.Fire();
                        BeamEffect thorn2 = GameManager.Factory.Get<BeamEffect>(
                            FactoryManager.FactoryType.AttackObject, "FrozenFlagThorn",
                            player.transform.position + Vector3.left * ((int)player.Direction * flag.padding * i1) +
                            Vector3.left * (flag.distance1 * (int)player.Direction));

                        thorn2.Init(new(0.25f, ground[index].size.y, ground[index].size.x, Ease.Linear,
                            BeamEffect.FireDir.Vertical));
                        thorn2.Init(player, new AtkItemCalculation(player, weapon,ground[index].dmg), 1);
                        thorn2.Init(weapon.CalculateGroggy(ground[index].groggy));
                        thorn2.Fire();
                    });
                    seq.AppendInterval(flag.timePadding);
                }
            }

            public override void AirAttack(int index)
            {
                base.AirAttack(index);

                for (int i = 0; i < air[index].count; i++)
                {
                    BeamEffect thorn = GameManager.Factory.Get<BeamEffect>(
                        FactoryManager.FactoryType.AttackObject, "FrozenFlagThorn", weapon.transform.position
                    );

                    thorn.Init(player, new AtkItemCalculation(player,weapon, air[index].dmg), 1);
                    thorn.Init(new(0.25f, air[index].size.x, air[index].size.y, Ease.Linear,
                        BeamEffect.FireDir.Horizontal));
                    thorn.Init(weapon.CalculateGroggy(air[index].groggy));

                    thorn.Fire();
                }
            }
        }
    }
}