using chamwhy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Apis
{
    public abstract class ItemDescription : MonoBehaviour
    {
        [SerializeField] protected Image itemImage;
        [SerializeField] protected TextMeshProUGUI description;
        [SerializeField] protected TextMeshProUGUI nameText;
        // public abstract AbsInvenManager Inven { get; }

        protected virtual void Awake()
        {
            itemImage.enabled = false;
            itemImage.sprite = null;
            nameText.text = "";
            description.text = "";
        }

        public virtual void ChangeInfo(Item item)
        {

            if (item == null)
            {
                itemImage.enabled = false;
                nameText.text = "";
                description.text = "";
            }
            else
            {
                itemImage.enabled = true;
                itemImage.sprite = item.Image;
                nameText.text = StrUtil.GetEquipmentName(item.ItemId);
                description.text = StrUtil.GetFlavorText(item.ItemId,1);
            }
        }
        
    }
}