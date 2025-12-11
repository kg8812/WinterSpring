using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    class LeadVocal : ProjectileWeapon
    {
        private LeadVocalAtk iweaponAttack;

        public override IWeaponAttack IAttack => iweaponAttack ??= new LeadVocalAtk(this);

        [TabGroup("공격설정/설정","지상")]
        [ShowIf("attackType",AttackType.Projectile)]
         public List<WaveInfo> groundWaveInfo;
        [TabGroup("공격설정/설정","공중")]
        [ShowIf("attackType",AttackType.Projectile)]
        public List<WaveInfo> airWaveInfo;
        public class LeadVocalAtk : ProjectileAttack
        {
            private LeadVocal leadVocal;
            
            public LeadVocalAtk(LeadVocal weapon) : base(weapon)
            {
                leadVocal = weapon;
                BeforeGroundFire += SetGroundWaveInfo;
                BeforeAirFire += SetAirWaveInfo;
                OnGroundFire += AddGroundEvent;
                OnAirFire += AddAirEvent;
            }

            void AddGroundEvent(Projectile proj, int atkIndex, int order)
            {
                proj.AddAtkEventOnce(x =>
                {
                    var info = leadVocal.groundWaveInfo[atkIndex];
                    Projectile temp = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject,
                        GetProjectileName(ProjectileType.LeadVocalBeam)
                        , x.target.Position);
                    temp.Init(player, new AtkItemCalculation(player, weapon,info.projInfo.info.dmg), info.beamInfo.fireTime);
                    temp.Init(info.projInfo.info);
                    if (temp is BeamEffect beam)
                    {
                        beam.Init(info.beamInfo);
                    }
                    temp.Init(weapon.CalculateGroggy(info.projInfo.groggy));
                    temp.SetDirection(proj.Direction);
                    temp.Fire(false);
                    weapon.OnAtkObjectInit.Invoke(new EventParameters(temp));
                });
            }

            void AddAirEvent(Projectile proj, int atkIndex, int order)
            {
                proj.AddAtkEventOnce(x =>
                {
                    var info = leadVocal.airWaveInfo[atkIndex];
                    Projectile temp = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject,
                        GetProjectileName(ProjectileType.LeadVocalBeam)
                        , x.target.Position);
                    temp.Init(player, new AtkItemCalculation(player, weapon,info.projInfo.info.dmg), info.beamInfo.fireTime);
                    temp.Init(info.projInfo.info);
                    if (temp is BeamEffect beam)
                    {
                        beam.Init(info.beamInfo);
                    }
                    temp.Init(weapon.CalculateGroggy(info.projInfo.groggy));
                    temp.SetDirection(proj.Direction);
                    temp.Fire(false);
                    weapon.OnAtkObjectInit.Invoke(new EventParameters(temp));
                });
            }
            void SetGroundWaveInfo(Projectile proj, int atkIndex, int order)
            {
                if (proj.TryGetComponent(out SoundWaveExtension wave))
                {
                    wave.size1 = leadVocal.groundWaveInfo[atkIndex].size1;
                    wave.size2 = leadVocal.groundWaveInfo[atkIndex].size2;
                    wave.dmgReduce = leadVocal.groundWaveInfo[atkIndex].dmgReduce;
                    wave.groggyReduce = leadVocal.groundWaveInfo[atkIndex].groggyReduce;
                    wave.reduceTime = leadVocal.groundWaveInfo[atkIndex].reduceTime;
                    wave.ease = leadVocal.groundWaveInfo[atkIndex].ease;
                }
            }
            void SetAirWaveInfo(Projectile proj, int i1, int i2)
            {
                if (proj.TryGetComponent(out SoundWaveExtension wave))
                {
                    wave.size1 = leadVocal.airWaveInfo[i1].size1;
                    wave.size2 = leadVocal.airWaveInfo[i1].size2;
                    wave.dmgReduce = leadVocal.airWaveInfo[i1].dmgReduce;
                    wave.groggyReduce = leadVocal.airWaveInfo[i1].groggyReduce;
                    wave.reduceTime = leadVocal.airWaveInfo[i1].reduceTime;
                    wave.ease = leadVocal.airWaveInfo[i1].ease;
                }
            }
        }
        [Serializable]
        public struct WaveInfo
        {
            [Title("음파 설정")]
            [LabelText("초기 크기")] public Vector2 size1;
            [LabelText("최종 크기")] public Vector2 size2;
            [LabelText("총 데미지 감소율(%)")] public float dmgReduce;
            [LabelText("총 그로기 감소율(%)")] public float groggyReduce;
            [LabelText("감소 적용시간")] [Tooltip("이 시간에 거쳐서 수치가 서서히 감소함")] public float reduceTime;
            [LabelText("감소 Ease")] public Ease ease;
            [Title("빔 설정")] 
            public BeamEffect.BeamInfo beamInfo;
            [LabelText("빔 투사체 설정")] public ProjInfo projInfo;
        }
    }
}