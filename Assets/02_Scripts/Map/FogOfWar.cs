using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace chamwhy
{
    public class FogOfWar: MonoBehaviour
    {
        private static readonly int RevealTexID = Shader.PropertyToID("_AlphaTex");
        private static readonly int ColorTexId = Shader.PropertyToID("_ClearTex");
        public static Material MaxMat { get; set; }
        public static Texture2D BrushTex { get; set; }
        
        // 실제 brush 크기와 reveal 크기의 차이를 위함(실제는 그라데이션 부분까지 포함, reveal은 가장 밝은 부분만 포함)
        public static Vector2 BrushRectRatio { get; set; }

        private Texture2D _colorBrush;

        private Texture2D ColorBrush
        {
            get
            {
                if (_colorBrush == null)
                {
                    _colorBrush = new Texture2D(1, 1);
                    _colorBrush.SetPixel(0,0,Color.white);
                    _colorBrush.Apply();
                }
                return _colorBrush;
            }
        }
        
        public Material Mat { get; set; }
        public bool IsClear { get; set; }

        private int _width;
        private int _height;

        private RenderTexture _revealTex;
        private RenderTexture _colorTex;
        public RenderTexture RevealTex => _revealTex;
        public RenderTexture ColorTex => _colorTex;

        public RawImage rawImg;

        [Button(50)]
        public void Set()
        {
            rawImg.texture = _revealTex;
        }
        
        
        public void Init(int width, int height)
        {
            _width = width;
            _height = height;

            _revealTex = RenderTexture.GetTemporary(_width, _height, 0);
            _colorTex = RenderTexture.GetTemporary(_width, _height, 0);

            _revealTex.filterMode = FilterMode.Point;
            
            UpdateRevealTexture();
            UpdateColorTexture();
        }

        public void CopyTexture(Texture2D tex, bool isReveal)
        {
            if (IsClear) return;
            RenderTexture.active = isReveal ? _revealTex : _colorTex;

            // 2. Graphics.Blit을 사용하여 Texture2D를 RenderTexture에 복사
            Graphics.Blit(tex, isReveal ? _revealTex : _colorTex);

            // 3. RenderTexture 사용 해제
            RenderTexture.active = null;
        }
        
        
        public void PaintReveal(MapDrawerRect mapRect)
        {
            if (IsClear) return;
            // Debug.Log($"paint reveal {MapDrawerRect.GetRect(mapRect)}");
            RenderTexture.active = _revealTex;
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, _width, 0, _height);

            // brush 자체에 부드러운 가장자리 효과가 들어있어서
            // 브러쉬 거의 white인 부분 = 실제 카메라 시야 이기 때문에 확장해서 Draw
            Rect rect = MapDrawerRect.GetRect(mapRect);
            // Debug.Log(BrushRectRatio);
            // Debug.Log($"paint reveal rect check 1 {rect}");
            rect.position -= new Vector2(rect.width * (BrushRectRatio.x-1) * 0.5f, rect.height * (BrushRectRatio.y-1) * 0.5f);
            rect.size = new Vector2(rect.width * BrushRectRatio.x, rect.height * BrushRectRatio.y);
            // Debug.Log($"paint reveal rect check 2 {rect}");
            Graphics.DrawTexture(rect, BrushTex, MaxMat);

            GL.PopMatrix();
            RenderTexture.active = null;

            UpdateRevealTexture();
        }
        
        public void ColorSector(MapDrawerRect mapRect)
        {
            if (IsClear) return;
            RenderTexture.active = _colorTex;
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, _width, 0, _height);

            Graphics.DrawTexture(MapDrawerRect.GetRect(mapRect), ColorBrush);

            GL.PopMatrix();
            RenderTexture.active = null;
            UpdateColorTexture();
        }
        
        

        private void UpdateRevealTexture()
        {
            if (IsClear) return;
            Mat.SetTexture(RevealTexID, _revealTex);
        }
        private void UpdateColorTexture()
        {
            if (IsClear) return;
            Mat.SetTexture(ColorTexId, _colorTex);
        }
    }
}