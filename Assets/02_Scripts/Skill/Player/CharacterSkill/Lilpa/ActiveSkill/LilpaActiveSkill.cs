using System.Collections.Generic;
using System.Linq;
using chamwhy;
using Command;
using Default;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Apis
{
    [CreateAssetMenu(fileName = "LilpaActive1", menuName = "Scriptable/Skill/LilpaActive1")]

    public class LilpaActiveSkill : PlayerActiveSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Toggle;

        private ToggleSkill _active => ActiveStrategy as ToggleSkill;

        private UnityEvent _onWeaponEquip;
        public UnityEvent OnWeaponEquip => _onWeaponEquip ??= new();

        private UnityEvent _onWeaponUnEquip;
        public UnityEvent OnWeaponUnEquip => _onWeaponUnEquip ??= new();

        List<ActorCommand> _activeSkillCommand;

        private List<ActorCommand> activeSkillCommand => _activeSkillCommand ??= new()
        {
            ResourceUtil.Load<ActorCommand>("ActiveSkillCommand")
        };

        protected override TagManager.SkillTreeTag Tag => TagManager.SkillTreeTag.Rifle;

        public enum WeaponType
        {
            Shotgun,Sword
        }
        protected override float TagIncrement => GameManager.Tag.Data.RifleIncrement;
        
         Dictionary<WeaponType, Weapon> _lilpaWeapons;
         public Dictionary<WeaponType, Weapon> lilpaWeapons => _lilpaWeapons ??= new();
        public override void Init()
        {
            base.Init();
            
            // Hidden inven에 추가로 ui open자체에 remove 로직 달아줄 필요 없음.
            // UI_TabMenu.OnUIOpen = RemoveShotgun;
            // ItemId - 3052 : 릴파의 샷건
            LilpaShotgun shotgun = InvenManager.instance.PresetManager.GetOverrideItem(3052) as LilpaShotgun;
            shotgun.activeSkill = this;
            if (!lilpaWeapons.TryAdd(WeaponType.Shotgun, shotgun))
            {
                lilpaWeapons[WeaponType.Shotgun] = shotgun;
            }
            // ItemId - 3053 : 릴파의 장검
            LilpaSword sword = InvenManager.instance.PresetManager.AddNewOverrideItem(3053) as LilpaSword;
            sword.activeSkill = this;
            if (!lilpaWeapons.TryAdd(WeaponType.Sword, sword))
            {
                lilpaWeapons[WeaponType.Sword] = sword;
            }
            
        }

        void RemoveShotgun()
        {
            if (_active is { isToggleOn: true })
            {
                AttackItemManager.ApplyPreset((int)Player.playerType);
                GameManager.instance.Player.Controller.Executors[Define.GameKey.ActiveSkill].keyDownCommand.Commands = GameManager.instance.Player
                    .Controller.BaseCommands.Commands[Define.GameKey.ActiveSkill].keyDownCommand;
                OnWeaponUnEquip.Invoke();
                Icon.showCDImage = true;
            }
        }

        protected override void OnEquip(IMonoBehaviour owner)
        {
            base.OnEquip(owner);
        }

        public override void Active()
        {
            base.Active();
            
            
            if (GameManager.instance.Player.OnAttack)
            {
                GameManager.instance.Player.CancelAttack();
            }

            AttackItemManager.ApplyPreset(6);
            GameManager.instance.Player.Controller.Executors[Define.GameKey.ActiveSkill].keyDownCommand.Commands = activeSkillCommand;
            OnWeaponEquip.Invoke();
            Icon.showCDImage = false;
            
            if (Skill2 is LilpaActiveSkill2 active2)
            {
                active2.active = this;
            }

            GameManager.Sound.Play(Define.SFXList.LilpaGunPick);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();

            RemoveShotgun();
            Skill2?.UnEquip();
        }
        
        public void WhenChanged()
        {
            if (IsToggleOn)
            {
                Use();
            }
        }
    }
}