using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class ViichanTree2E : ViichanTree
    {
        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("회복량(%)")] public float shield;
            [LabelText("쿨타임")] public float cd;
        }
        private ViichanActiveSkill skill;

        public DataStruct[] datas;

        public override void Init()
        {
            base.Init();
            isCd = false;
        }

        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as ViichanActiveSkill;

            if (skill == null) return;
            skill.OnShieldBreak -= AddGauge;
            skill.OnShieldBreak += AddGauge;
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill != null)
            {
                skill.OnShieldBreak -= AddGauge;
            }
        }

        void AddGauge()
        {
            if (isCd) return;
            
            if (skill != null)
            {
                skill.CurGauge +=  skill.MaxGauge * datas[level-1].shield / 100f;
                GameManager.instance.StartCoroutineWrapper(CoolDownCoroutine());
            }
        }

        private bool isCd;
        private float curTime;
        IEnumerator CoolDownCoroutine()
        {
            if (isCd) yield break;

            isCd = true;
            curTime = datas[level-1].cd;

            while (curTime > 0)
            {
                curTime -= Time.deltaTime;
                yield return null;
            }

            isCd = false;
        }
    }
}