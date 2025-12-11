using System.Collections;
using chamwhy.DataType;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis.SkillTree
{
    public class LilpaTree3E : SkillTree
    {
        private LilpaActiveSkill active;
        private LilpaPassiveSkill passive;

        public struct DataStruct
        {
            [LabelText("충전 발동 필요 스택")] public int necessaryStack;
            [LabelText("총알 충전량")] public int bulletCount;
            [LabelText("공격력 감소율 (%)")] public float atkReduce;
            [LabelText("지속시간")] public float duration;
        }

        public DataStruct[] datas;
        
        private BonusStat stat;

        private SubBuffOptionDataType data;
        
        BonusStat ReturnStat()
        {
            return stat;
            
        }
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            LilpaActiveSkill c = active as LilpaActiveSkill;
            this.active = c;
            if (c == null) return;
            stat ??= new();
            stat.Reset();
            stat.AddRatio(ActorStatType.Atk, -datas[level-1].atkReduce);
            BuffDatabase.DataLoad.TryGetSubBuffOption(SubBuffType.HunterStack, out data);
            GameManager.instance.Player.AddEvent(EventType.OnSubBuffApply,AddDebuff);
        }

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            this.passive = passive as LilpaPassiveSkill;
            if (this.passive == null) return;
            this.passive.OnHeal.RemoveListener(AddStack);
            this.passive.OnHeal.AddListener(AddStack);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            passive.OnHeal.RemoveListener(AddStack);
        }

        void AddStack(int stack)
        {
            if (stack >= datas[level-1].necessaryStack)
            {
                active.lilpaWeapons.Values.ForEach(x =>
                {
                    x.Skill.CurStack += datas[level-1].bulletCount;
                });
            }
        }

        void AddDebuff(EventParameters parameters)
        {
            if (data != null && parameters?.target is Actor target && target.SubBuffCount(SubBuffType.HunterStack) >= data.maxStack)
            {
                target.BonusStatEvent += ReturnStat;
                GameManager.instance.StartCoroutineWrapper(DurationCoroutine(target));
            }
        }

        private float curDuration;
        private bool isDuration;
        
        IEnumerator DurationCoroutine(Actor target)
        {
            curDuration = datas[level-1].duration;

            if (isDuration) yield break;

            isDuration = true;

            while (curDuration > 0)
            {
                curDuration -= Time.deltaTime;
                yield return null;
            }

            target.BonusStatEvent -= ReturnStat;
            isDuration = false;
        }
    }
}