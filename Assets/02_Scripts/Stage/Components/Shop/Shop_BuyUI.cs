using chamwhy;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Apis
{
    public class Shop_BuyUI : MonoBehaviour
    {
        // 구매 창

        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Button y;
        [SerializeField] Button n;

        private void Awake()
        {
            n.onClick.AddListener(() => gameObject.SetActive(false));
        }

        // 구매 버튼에 할당할 함수(구매)와 아이템 설정하는 함수
        public void Set(Item item, UnityAction action)
        {
            // TODO: UI text
            text.text = $"'{StrUtil.GetEquipmentName(item.ItemId)}'를 구매하시겠습니까?";
            y.onClick.RemoveAllListeners();
            y.onClick.AddListener(action);
            y.onClick.AddListener(() => gameObject.SetActive(false));
        }
    }
}
