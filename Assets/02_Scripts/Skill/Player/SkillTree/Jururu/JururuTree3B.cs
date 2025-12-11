using System.Collections.Generic;
using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class JururuTree3B : SkillTree
    {
        private JururuActiveSkill skill;

        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("파이어볼 투사체 설정")] public ProjectileInfo atkInfo;
            [LabelText("파이어볼 그로기수치")] public int groggy;
            [LabelText("파이어볼 개수")] public int count;
            [LabelText("파이어볼 반경")] public float radius;
            [LabelText("파이어볼 소환 각도")] public float angle;
        }

        public DataStruct[] datas;
        
        void SpawnFireBall(PokdoStand pokdo)
        {
            List<Projectile> fireballs = new();
            
            for (int i = 0; i < datas[level-1].count; i++)
            {
                Projectile explosion = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.Effect,
                    Define.PlayerEffect.JururuSoulKnightFireball, pokdo.Position);
                explosion.Init(pokdo, new AtkBase(pokdo, datas[level-1].atkInfo.dmg),10);
                explosion.Init(datas[level-1].atkInfo);
                explosion.Init(datas[level-1].groggy);
                explosion.gameObject.SetRadius(datas[level-1].radius);
                explosion.Fire();
                fireballs.Add(explosion);
            }
            Utils.SetProjectilesAngle(fireballs,datas[level-1].angle);
        }
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as JururuActiveSkill;
            if (skill == null) return;
            skill.OnPokdoSpawn -= SpawnFireBall;
            skill.OnPokdoSpawn += SpawnFireBall;
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill != null)
            {
                skill.OnPokdoSpawn -= SpawnFireBall;
            }
        }
    }
}