using Apis;
using chamwhy.DataType;
using chamwhy.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace chamwhy
{
    public class ActiveSkillItem: Item, IAttackItem
    {
        protected string _name;
        protected string flavourText;
        protected string description;

        public int itemId { get; set; }

        public override int ItemId => itemId;
        public override string Name => _name;
        
        public override string FlavourText => flavourText;
        
        public override string Description => description;
        
        [ReadOnly][ShowInInspector]public ActiveSkill ActiveSkill { get; set; }

        private ActiveSkillItemDataType _data;
        
        public override void Init()
        {
            base.Init();
            ActiveSkillItemDatas.activeSkillItems.TryGetValue(itemId, out _data);
            if (Image == null && _data != null)
            {
                Image = Default.ResourceUtil.Load<Sprite>(_data.iconPath);
            }
            ActiveSkill.Init();
            _name = LanguageManager.Str(ActiveSkill.SkillName);
            description = LanguageManager.Str(ActiveSkill.Desc);
        }

        public override void Activate()
        {
        }

        public void BeforeAttack()
        {
            ActiveSkill.BeforeAttack();
        }

        public void UseAttack()
        {
            ActiveSkill.Use();
        }

        public bool TryAttack()
        {
            return ActiveSkill.TryUse();
        }

        public AttackCategory Category => AttackCategory.Magic;

        public void WhenIconIsSet(UI_AtkItemIcon icon)
        {
            if (icon == null) return;
            
            icon.Skill = ActiveSkill;
        }

        public void EndAttack()
        {
            ActiveSkill?.DeActive();
        }

        protected override void OnEquip(IMonoBehaviour user)
        {
            base.OnEquip(user);
            ActiveSkill.Equip(user);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            ActiveSkill.UnEquip();
        }

        public override void Return()
        {
            base.Return();
            GameManager.Item.ActiveSkillItem.Return(this);
        }

        
        public UI_AtkItemIcon Icon { get; set; }
        public int AtkSlotIndex { get; set; }

        public int InvenSlotIndex => ActiveSkill.InvenIndex;
    }
}