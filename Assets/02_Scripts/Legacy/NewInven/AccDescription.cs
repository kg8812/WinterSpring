using chamwhy;
using TMPro;
using UnityEngine;

namespace Apis
{
    public class AccDescription : ItemDescription
    {
        [SerializeField] GameObject infos;

        [SerializeField] GameObject selectText;

        [SerializeField] private TextMeshProUGUI buffDescriptionText;
        // public override AbsInvenManager Inven => InvenManager.instance.Acc;
        
        protected override void Awake()
        {
            base.Awake();
            InfoOff();

        }

        public void InfoOff()
        {
            infos.SetActive(false);
            description.text = "";
            if (selectText != null)
            {
                selectText.SetActive(false);
            }
        }

        public override void ChangeInfo(Item item)
        {
            base.ChangeInfo(item);
            if (item is not Accessory acc)
            {
                infos.SetActive(false);
                buffDescriptionText.text = "";
            }
            else
            {
                infos.SetActive(true);
                buffDescriptionText.text = StrUtil.GetFlavorText(acc.ItemId);
            }
        }
    }

}