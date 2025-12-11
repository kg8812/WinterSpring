using System.Data;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis.SkillTree
{
    public class JururuTree3E : SkillTree
    {
        private JururuActiveSkill activeSkill;
        private JururuPassiveSkill passiveSkill;

        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("화상 확률")] public float prob;
        }

        public DataStruct[] datas;
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            activeSkill = active as JururuActiveSkill;
            if (activeSkill == null) return;
            activeSkill.OnPokdoSpawn -= PokdoEvent;
            activeSkill.OnPokdoSpawn += PokdoEvent;
        }

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            passiveSkill = passive as JururuPassiveSkill;

            if (passiveSkill == null) return;
            
            passiveSkill.OnSoldierSpawn.AddListener(RemoveSoldierBurn);
            passiveSkill.OnSoldierSpawn.RemoveListener(SoldierEvent);
            passiveSkill.OnSoldierSpawn.AddListener(SoldierEvent);
            passiveSkill.spawnedSoldiers.ForEach(x =>
            {
                x.AddEvent(EventType.OnAttackSuccess,AddBurn);
            });
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (activeSkill != null)
            {
                activeSkill.OnPokdoSpawn -= PokdoEvent;
                activeSkill.OnPokdoSpawn += RemoveBurn;
            }

            if (passiveSkill != null)
            {
                passiveSkill?.OnSoldierSpawn.RemoveListener(SoldierEvent);
                passiveSkill?.OnSoldierSpawn.AddListener(RemoveSoldierBurn);
                passiveSkill?.spawnedSoldiers.ForEach(x => { x.RemoveEvent(EventType.OnAttackSuccess, AddBurn); });
            }
        }

        void SoldierEvent(FoxSoldier soldier)
        {
            soldier.AddEvent(EventType.OnAttackSuccess,AddBurn);
        }

        void RemoveSoldierBurn(FoxSoldier soldier)
        {
            soldier.RemoveEvent(EventType.OnAttackSuccess,AddBurn);
        }
        void PokdoEvent(PokdoStand pokdo)
        {
            pokdo.AddEvent(EventType.OnAttackSuccess,AddBurn);
        }

        void RemoveBurn(PokdoStand pokdo)
        {
            pokdo.RemoveEvent(EventType.OnAttackSuccess,AddBurn);
        }
        void AddBurn(EventParameters parameters)
        {
            if (parameters?.target is Actor target)
            {
                float rand = Random.Range(0, 1f);
                if (rand < datas[level-1].prob)
                {
                    target.AddSubBuff(parameters.master, SubBuffType.Debuff_Burn);
                }
            }
        }
    }
}