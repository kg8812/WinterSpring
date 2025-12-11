using UnityEngine;
using UnityEngine.EventSystems;

namespace _02_Scripts.Tester
{
    public class UITester: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log($"begin: {eventData}");
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log($"on:");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log($"end: {eventData}");
        }
    }
}