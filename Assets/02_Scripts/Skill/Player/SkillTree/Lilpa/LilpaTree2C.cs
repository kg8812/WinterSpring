using chamwhy;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis.SkillTree
{
    public class LilpaTree2C : SkillTree
    {
        private LilpaActiveSkill skill;

        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("파편 공격 설정")] public ProjectileInfo info;
            [LabelText("파편 그로기 수치")] public int groggy;
            [LabelText("파편 반경")] public float radius;
        }

        public DataStruct[] datas;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as LilpaActiveSkill;
            if (skill == null) return;
            
            skill.lilpaWeapons.Values.ForEach(x =>
            {
                x.OnAtkObjectInit.RemoveListener(AddEvent);
                x.OnAtkObjectInit.AddListener(AddEvent);
            });
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill.lilpaWeapons.Values.ForEach(x =>
            {
                x.OnAtkObjectInit.RemoveListener(AddEvent);
            });
        }

        void AddEvent(EventParameters eventParameters)
        {
            if (eventParameters.user is AttackObject bullet)
            {
                bullet.AddEventUntilInitOrDestroy(SpawnStarFragments);
            }
        }
        void SpawnStarFragments(EventParameters eventParameters)
        {
            if (eventParameters?.target == null) return;

            AttackObject fragment = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                Define.PlayerSkillObjects.LilpaBulletFragment,
                eventParameters.target is Actor target ? target.Position : eventParameters.target.transform.position);

            fragment.Init(skill.Player, new AtkItemCalculation(skill.user as Actor, skill, datas[level-1].info.dmg), 1);
            fragment.Init(datas[level-1].info);
            fragment.Init(datas[level-1].groggy);

            fragment.transform.localScale = Vector3.one * (datas[level-1].radius * 2);
        }
    }
}