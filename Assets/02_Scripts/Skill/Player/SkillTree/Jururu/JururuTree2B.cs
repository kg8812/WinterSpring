using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class JururuTree2B : SkillTree
    {
        [InfoBox("프리팹으로 이동해서 자세한 수치를 조정해주세요")]
        [LabelText("불기둥")] public BeamEffect flameWallPrefab;
        [LabelText("소환 개수")] public int count;
        [LabelText("소환 시간간격")] public float paddingTime;
        [LabelText("소환 거리간격")] public float padding;

        
        void SpawnFlameWalls(PokdoStand stand,int combo)
        {
            if (combo == 2)
            {
                Sequence seq = DOTween.Sequence();
                Vector3 startPos = stand.transform.position;

                for (int i = 0; i < count; i++)
                {
                    int temp = i + 1;
                    seq.AppendCallback(() =>
                    {
                        BeamEffect flame = GameManager.Factory.Get<BeamEffect>(FactoryManager.FactoryType.AttackObject,
                            Define.PlayerSkillObjects.JururuPokdoFlameWall,
                            startPos + (int)stand.Direction * padding * temp * stand.transform.localScale.x * Vector3.right);
                        
                        flame.transform.SetParent(stand.effectParent);
                        flame.transform.localScale = Vector3.one;
                        flame.Init(stand, new AtkBase(stand, flame.projectileInfo.dmg), 1f);
                        flame.AddEventUntilInitOrDestroy(AddBurn);
                        flame.Fire();
                    });
                    seq.AppendInterval(paddingTime);
                }
            }
        }

        private JururuActiveSkill skill;

        void AddBurn(EventParameters parameters)
        {
            if (parameters?.target is Actor target)
            {
                target.AddSubBuff(skill.Player, SubBuffType.Debuff_Burn);
            }
        }
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as JururuActiveSkill;

            if (skill == null) return;
            skill.OnPokdoAttack -= SpawnFlameWalls;
            skill.OnPokdoAttack += SpawnFlameWalls;
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill != null)
            {
                skill.OnPokdoAttack -= SpawnFlameWalls;
            }
        }
    }
}