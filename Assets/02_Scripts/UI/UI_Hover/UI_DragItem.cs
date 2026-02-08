using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class UI_DragItem: UI_Hover
    {
        public Image DragImg;

        enum Rects
        {
            ItemImg
        }

        public override void Init()
        {
            base.Init();
            Bind<RectTransform>(typeof(Rects));
            _contentTrans = Get<RectTransform>((int)Rects.ItemImg);
            DragImg.enabled = false;
        }

        public override void TryActivated(bool force = false)
        {
            base.TryActivated(force);
            DragImg.enabled = true;
        }
        
        public override void TryDeactivated(bool force = false)
        {
            base.TryDeactivated(force);
            DragImg.enabled = false;
        }
    }
}