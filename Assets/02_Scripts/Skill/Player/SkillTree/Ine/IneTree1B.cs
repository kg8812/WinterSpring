
using System;
using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class IneTree1B : SkillTree
    {
        [Serializable]
        public struct Data
        {
            [LabelText("작은 깃털 소환 갯수")] public int count;
            [LabelText("작은 깃털 투사체 설정")] public ProjectileInfo info;
            [LabelText("작은 깃털 그로기 수치")] public int groggy;
        }
        

        private InePassiveSkill skill;

        public Data[] datas;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            
            skill = passive as InePassiveSkill;

            if (skill != null)
            {
                skill.OnFire.RemoveListener(SpawnSmallFeather);
                skill.OnFire.AddListener(SpawnSmallFeather);
            }
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            skill?.OnFire.RemoveListener(SpawnSmallFeather);
        }

        public void SpawnSmallFeather(GameObject feather, IOnHit target)
        {
            for (int i = 1; i <= datas[level-1].count; i++)
            {
                ParticleSystem appear = GameManager.Factory.Get<ParticleSystem>(FactoryManager.FactoryType.Effect,
                    Define.PlayerEffect.IneFeatherAppear,
                    feather.transform.position + Vector3.up * (0.5f * Mathf.Pow(-1, i) * (i / 2 + i % 2)));
                
                GameManager.Factory.Return(appear.gameObject, appear.main.duration);

                Projectile smallFeather = GameManager.Factory.Get<Projectile>(
                    FactoryManager.FactoryType.Effect,
                    Define.PlayerSkillObjects.IneSmallFeather,
                    feather.transform.position + Vector3.up * (0.5f * Mathf.Pow(-1, i) * (i / 2 + i % 2)));

                smallFeather.Init(skill.attacker, new AtkItemCalculation(skill.user as Actor , skill, datas[level-1].info.dmg));
                smallFeather.Init(datas[level-1].info);
                smallFeather.Init(datas[level-1].groggy);
                
                smallFeather.Fire(false);
                smallFeather.LookAtTarget(target);
                smallFeather.rigid.velocity = Quaternion.AngleAxis(30 * Mathf.Pow(-1, i), Vector3.forward) *
                                              smallFeather.rigid.velocity; 
                
                smallFeather.AddEventUntilInitOrDestroy(x =>
                {
                    ParticleSystem hit = GameManager.Factory.Get<ParticleSystem>(FactoryManager.FactoryType.Effect,
                        Define.PlayerEffect.IneFeatherHit, smallFeather.transform.position);
                    GameManager.Factory.Return(hit.gameObject, hit.main.duration);
                });
            }
        }
    }
}