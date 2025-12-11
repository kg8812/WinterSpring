using System;
using GameStateSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class IneTree2C : SkillTree
    {
        private IneActiveSkill skill;

        [Serializable]
        public struct Data
        {
            [LabelText("마나 회복량(%)")] public float manaGain;
        }

        private IneActiveAttachment _attachment;

        public Data[] datas;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as IneActiveSkill;
            _attachment ??= new(new IneActiveStat());
            
            if (skill == null) return;
            
            skill.AddAttachment(_attachment);
            skill.OnUpdate.AddListener(AddMana);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.OnUpdate.RemoveListener(AddMana);
            skill?.RemoveAttachment(_attachment);
        }

        
        void AddMana()
        {
            _attachment.IneStat.manaGainInSecond = (GameManager.instance.CurGameStateType != GameStateType.BattleState || level > 1) && !skill.IsToggleOn  ? datas[level-1].manaGain : 0;
        }
    }
}