using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class LilpaTree1B : SkillTree
    {
        public struct DataStruct
        {
            [LabelText("단검 투사체 설정")] public ProjectileInfo info;
            [LabelText("단검 발사 개수")] public int count;
        }

        public DataStruct[] datas;
        private LilpaPassiveSkill skill;
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as LilpaPassiveSkill;
            if (skill == null) return;
            
            skill.Player.AddEvent(EventType.OnKill,SpawnDaggers);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.Player.RemoveEvent(EventType.OnKill,SpawnDaggers);
        }

        void SpawnDaggers(EventParameters eventParameters)
        {
            if (eventParameters?.target is not Actor actor) return;

            for (int i = 0; i < datas[level-1].count; i++)
            {
                Projectile dagger = GameManager.Factory.Get<Projectile>(
                    FactoryManager.FactoryType.AttackObject,
                    Define.PlayerSkillObjects.LilpaDagger, actor.Position + Vector3.right * (0.5f * i));

                dagger.Init(skill.Player, new AtkItemCalculation(skill.user as Actor , skill, datas[level-1].info.dmg), 10);
                dagger.Init(datas[level-1].info);
                var enemies = dagger.gameObject.GetTargetsInCircle(datas[level-1].info.followRange, LayerMasks.Enemy);
                if (enemies.Count > 0)
                {

                    float min = Vector2.Distance(dagger.transform.position, enemies[0].transform.position);
                    IOnHit target = enemies[0];

                    foreach (var x in enemies)
                    {
                        float distance = Vector2.Distance(dagger.transform.position, x.transform.position);
                        if (min > distance)
                        {
                            target = x;
                            min = distance;
                        }
                    }

                    dagger.LookAtTarget(target);
                }
                dagger.AddAtkEventOnce(AddStack);
                dagger.Fire(false);
                dagger.rigid.velocity = Quaternion.AngleAxis(30 * Mathf.Pow(-1, i), Vector3.forward) *
                                        dagger.rigid.velocity; 
            }
        }
        
        void AddStack(EventParameters parameters)
        {
            if (parameters?.target is Actor target)
            {
                target.AddSubBuff(skill.Player,SubBuffType.HunterStack);
            }
        }
    }
}