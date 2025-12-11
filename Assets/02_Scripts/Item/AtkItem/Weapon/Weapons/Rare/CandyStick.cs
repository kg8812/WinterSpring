
using chamwhy;
using chamwhy.Managers;
using UnityEngine;

namespace Apis
{
    public class CandyStick : Weapon
    {
        [HideInInspector] public Weapon lollipop;
        protected override void OnEquip(IMonoBehaviour user)
        {
            base.OnEquip(user);
            GameManager.Scene.WhenSceneLoadBegin.RemoveListener(ChangeToLollipop);
            GameManager.Scene.WhenSceneLoadBegin.AddListener(ChangeToLollipop);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            GameManager.Scene.WhenSceneLoadBegin.RemoveListener(ChangeToLollipop);
        }
        
        void ChangeToLollipop(SceneData data)
        {
            if (lollipop == null) return;
            lollipop = InvenManager.instance.Storage.Get(lollipop) as BigLollipop;

            if (lollipop == null) return;
            lollipop.Activate();
            InvenManager.instance.Storage.Store(InvenManager.instance.AttackItem.ChangeAttackItem(lollipop));
                
            // Player.UnEquip();
            // Item wp = InvenManager2.instance.Wp.Remove(0, InvenType.Equipment);
            //
            //
            // InvenManager2.instance.Wp.Invens[InvenType.Equipment].AddItem(0, lollipop);
            // GameManager.instance.Player.Equip(lollipop);
        }
    }
}