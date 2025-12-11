using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class LilpaTree3B : SkillTree
    {
        [LabelText("이속 증가량 (%)")] public float speed;
        [LabelText("지속시간")] public float duration;

        private LilpaActiveSkill skill;

        private BonusStat stat;

        private float curTime;
        private bool isActive;
        BonusStat ReturnStat()
        {
            return stat;
        }
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            
            skill = active as LilpaActiveSkill;
            isActive = false;
            stat ??= new();
            stat.Reset();
            stat.AddRatio(ActorStatType.MoveSpeed, speed);
            
            if (skill == null) return;
            skill.OnWeaponEquip.RemoveListener(StartStat);
            skill.OnWeaponUnEquip.RemoveListener(StartStat);
            skill.OnWeaponEquip.AddListener(StartStat);
            skill.OnWeaponUnEquip.AddListener(StartStat);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            isActive = false;
            skill.Player.BonusStatEvent -= ReturnStat;
            curTime = 0;
            skill.OnWeaponEquip.RemoveListener(StartStat);
            skill.OnWeaponUnEquip.RemoveListener(StartStat);
        }

        void StartStat()
        {
            curTime = duration;
            GameManager.instance.StartCoroutineWrapper(StatCoroutine());
        }
        IEnumerator StatCoroutine()
        {
            if (isActive) yield break;

            skill.Player.BonusStatEvent += ReturnStat;
            isActive = true;
            curTime = duration;
            while (curTime > 0)
            {
                curTime -= Time.deltaTime;
                yield return null;
            }
            skill.Player.BonusStatEvent -= ReturnStat;
            isActive = false;
        }
    }
}