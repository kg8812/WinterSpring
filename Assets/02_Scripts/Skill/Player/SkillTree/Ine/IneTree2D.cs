using System.Collections;
using chamwhy;
using NewNewInvenSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class IneTree2D : SkillTree
    {
        private IneActiveSkill skill;

        private IneActiveAttachment attachment;

        [LabelText("달빛 조각 개수 증가량")] public int circle1Count;
        [LabelText("영역 공격주기 감소량")] public float circle2Frequency;
        [LabelText("홀리빔 반경")] public float holyBeamRadius;
        [LabelText("홀리빔 공격 설정")] public ProjectileInfo holyBeamInfo;
        [LabelText("홀리빔 생성 갯수")] public int count;
        [LabelText("홀리빔 생성 거리간격")] public float padding;
        [LabelText("홀리빔 생성 시간간격")] public float paddingTime;
        [LabelText("홀리빔 데미지")] public float dmg;
        [LabelText("홀리빔 그로기(고정값)")] public int groggy;
        [LabelText("홀리빔 생성 지연시간")] public float time;

        private IneCircle3 _circle3;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as IneActiveSkill;
            if (skill == null) return;

            attachment ??= new IneActiveAttachment(new IneActiveStat()
            {
                circle1Count = circle1Count, circle2AtkFrequency = -circle2Frequency
            });
            // ItemId - 4105 : 서클4
            _circle3 = (InvenManager.instance.PresetManager.GetOverrideItem(4105) as ActiveSkillItem)?.ActiveSkill as IneCircle3;
            skill.AddAttachment(attachment);
            skill.OnMeteorCollide.AddListener(SpawnHolyLight);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            skill?.RemoveAttachment(attachment);
            skill?.OnMeteorCollide.RemoveListener(SpawnHolyLight);
        }

        public void SpawnHolyLight(Vector2 pos)
        {
            GameManager.instance.StartCoroutine(HolyLight(pos));
        }

        IEnumerator HolyLight(Vector2 pos)
        {
            yield return new WaitForSeconds(time);

            for (int i = 0; i < count; i++)
            {
                AttackObject holyBeam = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                    Define.PlayerEffect.Ine_Magic_3Circle_GodRay,
                    // ReSharper disable once PossibleLossOfFraction , 일부러 소수부 소실시킴
                    pos + Vector2.right * (padding * Mathf.Pow(-1, i) * ((i + 1) / 2)));
                holyBeam.transform.localScale = Vector3.one * (2 * holyBeamRadius);
                holyBeam.Init(skill.attacker, new AtkItemCalculation(_circle3.user as Actor, _circle3, dmg));
                holyBeam.Init(holyBeamInfo);
                holyBeam.Init(groggy);
                GameManager.Factory.Return(holyBeam.gameObject, 1f);
                yield return new WaitForSeconds(paddingTime);
            }
        }
    }
}