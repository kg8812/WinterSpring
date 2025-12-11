using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    class LeadVocalSkill : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        protected override bool UseAtkRatio => false;

        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("공격 설정")] public ProjectileInfo info;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("초기 크기")] public Vector2 size1;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("최종 크기")] public Vector2 size2;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("총 데미지 감소율(%)")] public float dmgReduce;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("총 그로기 감소율(%)")] public float groggyReduce;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("감소 적용시간")] [Tooltip("이 시간에 거쳐서 수치가 서서히 감소함")] public float reduceTime;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("감소 Ease")] public Ease ease;

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);
            actionList.Clear();
            actionList.Add(Fire);
        }

        void Fire()
        {
            Projectile proj = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject,
                "SoundWave", _weapon.FirePos);
            SoundWaveExtension wave = proj.GetComponent<SoundWaveExtension>();
            wave.size1 = size1;
            wave.size2 = size2;
            wave.dmgReduce = dmgReduce;
            wave.groggyReduce = groggyReduce;
            wave.reduceTime = reduceTime;
            wave.ease = ease;
            proj.Init(attacker, new AtkBase(attacker, info.dmg));
            proj.Init(info);
            proj.Init(_weapon.CalculateGroggy(BaseGroggyPower));
            proj.Fire();
           
        }
    }
}