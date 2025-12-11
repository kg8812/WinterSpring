using System.Collections;
using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class ViichanTree2D : ViichanTree
    {
        public struct DataStruct
        {
            [LabelText("검 공격설정")] public ProjectileInfo atkInfo;
            [LabelText("공격 반경")] public float radius;
            [LabelText("검 크기")] public float swordScale;
            [LabelText("쿨타임")] public float cd;
        }

        public DataStruct[] datas;

        private ViichanActiveSkill skill;

        private bool isCd;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as ViichanActiveSkill;

            if (skill == null) return;

            isCd = false;
            skill.OnShield -= SpawnSword;
            skill.OnShield += SpawnSword;
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill == null) return;

            skill.OnShield -= SpawnSword;
        }

        void SpawnSword()
        {
            if (isCd) return;
            
            GameObject obj = GameManager.Factory.Get(FactoryManager.FactoryType.Normal,
                Define.PlayerEffect.ViichanSwordEffect, skill.user.Position + Vector3.up * 2);
            obj.transform.localScale = Vector3.one * (datas[level-1].swordScale * 2);
            obj.transform.DOMoveY(skill.user.Position.y, 0.5f).onComplete += () =>
            {
                AttackObject explosion = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                    Define.DummyEffects.Explosion, obj.transform.position);
                explosion.transform.localScale = Vector3.one * (0.6f * datas[level-1].radius);
                explosion.Init(skill.Player,new AtkItemCalculation(skill.Player,skill, datas[level-1].atkInfo.dmg));
                explosion.Init(datas[level-1].atkInfo);
                GameManager.Factory.Return(obj);
            };

            GameManager.instance.StartCoroutineWrapper(CDCoroutine());
        }

        IEnumerator CDCoroutine()
        {
            if (isCd) yield break;

            isCd = true;

            yield return new WaitForSeconds(datas[level-1].cd);

            isCd = false;
        }
    }
}