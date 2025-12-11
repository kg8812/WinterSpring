using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class Jingburger1D : SkillTree
    {
        private JingburgerPassiveSkill skill;
        [LabelText("폭발 반경")] public float radius;
        [LabelText("폭발 데미지")] public float dmg;
        [LabelText("폭발 딜레이")] public float delay;

        private JingburgerPassiveSkill.IFireEvent dog;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as JingburgerPassiveSkill;

            if (skill == null) return;

            dog = skill.GetAnimalPaint(JingburgerPassiveSkill.AnimalPaints.Dog);
            dog?.OnUse.Remove(Invoke);
            dog?.OnUse.Add(Invoke);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            dog?.OnUse.Remove(Invoke);
        }

        void Invoke(JingPaint paint)
        {
            paint.AttackSequence.Add(SpawnMine);
        }
        Tween SpawnMine(EventParameters parameters)
        {
            if (parameters.user != null)
            {
                GameObject mine = GameManager.Factory.Get(FactoryManager.FactoryType.Normal,
                    Define.PlayerSkillObjects.JingPuppyMine, parameters.user.transform.position);
                mine.SetActive(false);
                Sequence seq = DOTween.Sequence();
                seq.AppendCallback(() =>
                {
                    mine.SetActive(true);
                    Sequence s = DOTween.Sequence();
                    s.AppendInterval(delay);
                    s.AppendCallback(() =>
                    {
                        AttackObject explosion = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                            Define.DummyEffects.Explosion, mine.transform.position);
                        explosion.transform.localScale = Vector3.one * (0.6f * radius);
                        explosion.Init(skill.attacker,new AtkItemCalculation(skill.user as Actor ,skill, dmg ),1);
                        explosion.Init(GameManager.instance.Player.atkInfo);
                        GameManager.Factory.Return(mine);
                    });
                    
                });

                return seq;
            }
            return null;

        }
    }
}