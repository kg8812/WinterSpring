using System;
using UnityEngine;
using UnityEngine.UI;

namespace chamwhy
{
    public class MapImg: MonoBehaviour
    {
        public Vector2 LeftDownPos { get; private set; }

        private RectTransform _rectTransform;
        private FogOfWar _fog;
        public FogOfWar Fog => _fog ??= GetComponent<FogOfWar>();
        public Image Img { get; set; }

        public bool isDirty;
        
        private void Awake()
        {
            _fog ??= GetComponent<FogOfWar>();
            _rectTransform ??= GetComponent<RectTransform>();
            isDirty = false;
        }

        public void PositionUpdate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
            Vector2 size = _rectTransform.sizeDelta;    // UI 크기
            Vector2 pivot = _rectTransform.pivot;       // Pivot 값 (0~1)
            Vector3 scale = _rectTransform.localScale;  // 월드 스케일 적용


            // UI의 왼쪽 하단 Offset을 계산 (Pivot 기준 보정)
            Vector2 offset = new Vector2(-pivot.x * size.x * scale.x, -pivot.y * size.y * scale.y);
            LeftDownPos = _rectTransform.anchoredPosition + offset;
            isDirty = true;
        }
    }
}