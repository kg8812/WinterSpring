using chamwhy;
using UI;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "LilpaActive2", menuName = "Scriptable/Skill/LilpaActive2")]
    public class LilpaActiveSkill2 : PlayerActiveSkill
    {
        [HideInInspector] public LilpaActiveSkill active;
        
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        public override void Active()
        {
            base.Active();
            if (GameManager.instance.Player.OnAttack)
            {
                GameManager.instance.Player.CancelAttack();
            }
            Player player = activeUser as Player;
            AttackItemManager.ApplyPreset((int)(player?.playerType ?? 0));
            GameManager.instance.Player.Controller.Executors[Define.GameKey.ActiveSkill].keyDownCommand.Commands = GameManager.instance.Player
                .Controller.BaseCommands.Commands[Define.GameKey.ActiveSkill].keyDownCommand;
            Icon.showCDImage = true;
            GameManager.Sound.Play(Define.SFXList.LilpaGunDrop);
            active.OnWeaponUnEquip.Invoke();
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            active?.UnEquip();
        }
    }
}