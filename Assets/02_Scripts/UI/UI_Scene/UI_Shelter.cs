using System.Collections;
using System.Collections.Generic;
using chamwhy;
using chamwhy.UI;
using chamwhy.UI.Focus;
using Default;
using Directing;
using Managers;
using Save.Schema;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Apis
{
    public class UI_Shelter : UI_TabMenu
    {
        public override void TryActivated(bool force = false)
        {
            base.TryActivated(force);

            UITab_Inventory.PreventMovingOrigin = false;
        }

        public override void TryDeactivated(bool force = false)
        {
            base.TryDeactivated(force);
            UITab_Inventory.PreventMovingOrigin = true;
            GameManager.instance.SaveSlot();
        }

        
        // exit
        public override void CloseOwn()
        {
            FadeManager.instance.Fading(CloseOwnForce);
        }
    }
}