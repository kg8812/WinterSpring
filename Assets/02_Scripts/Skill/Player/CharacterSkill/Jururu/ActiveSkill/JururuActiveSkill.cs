using System;
using chamwhy;
using NewNewInvenSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "JururuActive", menuName = "Scriptable/Skill/JururuActive")]
    public class JururuActiveSkill : PlayerActiveSkill
    {

        [HideInInspector] public PokdoStand curPokdoStand;
        Player MasterPlayer => GameManager.instance.Player;

        private ISkillActive activeType;

        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        float size;

        [HideInInspector] public Action<PokdoStand,int> OnPokdoAttack;
        [HideInInspector] public Action<PokdoStand> OnPokdoSpawn;

        [HideInInspector] public PokdoAttackSkill atkSkill;
        [HideInInspector] public PokdoESkill crySkill;
        [HideInInspector] public PokdoMouseRSkill grabSkill;

        public override void Init()
        {
            base.Init();
            // ItemId - 4403 : 기사 공격
            atkSkill = (InvenManager.instance.PresetManager.GetOverrideItem(4403) as ActiveSkillItem)?.ActiveSkill as PokdoAttackSkill;
        }

        public void InitPokdoStand()
        {
            if (curPokdoStand == null) return;
            curPokdoStand.Init(MasterPlayer,this);
            curPokdoStand.transform.localRotation = Quaternion.identity;
            curPokdoStand.transform.localScale = Vector3.one * size;
            curPokdoStand.effectParent.localScale = Vector3.one * size;
        }

        public void SetSize(float _size)
        {
            size = _size;
            if (curPokdoStand != null)
            {
                curPokdoStand.transform.localScale = Vector3.one * size;
                curPokdoStand.effectParent.localScale = Vector3.one * size;
            }
        }
        public override void AfterDuration()
        {
            base.AfterDuration();

            if (curPokdoStand != null && curPokdoStand.TryGetComponent<PokdoStand>(out var x))
            {
                x.Die();
                curPokdoStand = null;
            }
            AttackItemManager.ApplyPreset((int)MasterPlayer.playerType);
            
            GameManager.instance.Player.Controller.ReturnToBase();
            dashUser?.SetDashToNormal();
            GameManager.instance.Player.BlockSkillChange = false;
        }

        public void UpdatePokdo()
        {
            if (curPokdoStand == null) return;
            
            curPokdoStand.transform.localScale = Vector3.one * size;
            curPokdoStand.effectParent.localScale = Vector3.one * size;
            AttackItemManager.ApplyPreset(7);
            if (grabSkill != null)
            {
                grabSkill.pokdo = curPokdoStand;
            }

            if (crySkill != null)
            {
                crySkill.pokdo = curPokdoStand;
            }
        }

        protected override void OnEquip(IMonoBehaviour owner)
        {
            base.OnEquip(owner);
            size = 1;
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            
            if (curPokdoStand != null)
            {
                curPokdoStand.Die();
                curPokdoStand = null;
            }
        }

        protected override TagManager.SkillTreeTag Tag => TagManager.SkillTreeTag.Ghost;

        protected override float TagIncrement => GameManager.Tag.Data.GhostIncrement;

        public override void Active()
        {
            base.Active();
            curPokdoStand = GameManager.Factory.Get<PokdoStand>(FactoryManager.FactoryType.Normal, "PokdoStand");
            InitPokdoStand();
            OnPokdoSpawn?.Invoke(curPokdoStand);
            AttackItemManager.ApplyPreset(7);
            
            GameManager.instance.Player.BlockSkillChange = true;
            if (AttackItemManager.CurrentItem is ActiveSkillItem skill)
            {
                skill.ActiveSkill.Cancel();
            }
        }
    }
}