using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class Jingburger2D : SkillTree
    {
        private JingburgerActiveSkill skill;
        [LabelText("추가 폭발 데미지")] public float dmg;
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as JingburgerActiveSkill;
            if (skill == null) return;
            
            skill.OnExplosionSpawn.RemoveListener(AddExplode);
            skill.OnExplosionSpawn.AddListener(AddExplode);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.OnExplosionSpawn.RemoveListener(AddExplode);
        }

        void AddExplode(AttackObject atk)
        {
            atk.AddEventUntilInitOrDestroy(x =>
            {
                Vector2 pos = x.user.Position;
                Sequence seq = DOTween.Sequence();
                seq.SetDelay(0.25f);
                seq.AppendCallback(() =>
                {
                    skill.SpawnExtraExplosion(pos, dmg);
                });
            },EventType.OnDestroy);
        }
    }
}