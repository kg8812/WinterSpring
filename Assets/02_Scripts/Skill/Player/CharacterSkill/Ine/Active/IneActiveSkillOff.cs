using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "IneActiveOff", menuName = "Scriptable/Skill/Ine/ActiveOff")]

    public class IneActiveSkillOff : ActiveSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;
        public override UI_AtkItemIcon Icon => UI_MainHud.Instance.mainSkillIcon;

        [HideInInspector] public IneActiveSkill active;
        
        public override void Active()
        {
            base.Active();
            Player player = activeUser as Player;

            AttackItemManager.ApplyPreset((int)(player?.playerType ?? 0));
            active?.OnToggle.Invoke(false);
            if (active?.ineBook != null)
            {
                active.SpawnEffect(Define.PlayerEffect.Ine_Book_Disappear, 0.5f, active.ineBook.transform.position,
                    false
                );
                GameManager.Factory.Return(active.ineBook.gameObject);
                active.ineBook = null;
            }
            active?.RemoveEffect(Define.PlayerEffect.Ine_MoonCircle02);
            active?.SpawnEffect(Define.PlayerEffect.Ine_MoonCircle01_Change, Define.PlayerEffect.Ine_WingCircle02,
                active.magicCircleRadius,false,null,"ctrl");
            active?.passive?.Enable();
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            active?.UnEquip();
        }
    }
}