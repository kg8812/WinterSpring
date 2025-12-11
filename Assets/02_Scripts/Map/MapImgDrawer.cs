using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace chamwhy
{
    public struct MapDrawerRect
    {
        public int sX, sY, eX, eY;

        public static Rect GetRect(MapDrawerRect mapRect)
        {
            return new Rect(mapRect.sX, mapRect.sY, mapRect.eX-mapRect.sX, mapRect.eY-mapRect.sY);
        }
    }
    public class MapImgDrawer : MonoBehaviour
    {
        [SerializeField] private Transform mapImgParent;
        [SerializeField] private Transform mapBoxParent;
        [SerializeField] private Sprite mapBoxBgSprite;
        // [SerializeField] private MapImgPermanaentData[] imgDatas;
        [SerializeField] private Material fogMat;
        [SerializeField] private Texture2D brushTex;
        // 일단은 고정으로 진행
        [SerializeField] private Vector2Int brushOriginPixel = new Vector2Int(800, 450);
        [SerializeField] private Material brushMaxMat;
        [SerializeField] private float pixelPerGrid = 4;
        [Tooltip("map img들의 ppu와 동일하게")]
        [SerializeField] private int ppu = 32;
        
        private Dictionary<int, MapImg> _mapImgFogOfWars;
        private float _gridPerUnit;
        private Dictionary<int, Image> _mapBoxBg;
        private Vector2 _curPlayerFogMapPos;
        private MapImg _curPlayerMapImg;

        public Dictionary<int, MapImg> MapImgs => _mapImgFogOfWars;

        private Dictionary<int, MapBoxPermanentData> BoxDatas => Map.instance.BoxDatas;
        private Dictionary<int, MapImgPermanaentData> ImgDatas => Map.instance.ImgDatas;
        

        #region ColorSetting
        [Serializable]
        public enum MapColorType
        {
            UnClear, Default, Ine, Jingburger, Lilpa, Jururu, Gosegu, Viichan
        }
        [Serializable]
        public class MapColorTypeColorDictionary: UnitySerializedDictionary<MapColorType, Color> {}

        public MapColorTypeColorDictionary mapColors = new ()
        {
            { MapColorType.UnClear, new Color(0.7f, 0.7f, 0.7f) },
            { MapColorType.Default, Color.white },
            { MapColorType.Ine, new Color(0.7f, 0.7f, 0.7f) },
            { MapColorType.Jingburger, new Color(0.7f, 0.7f, 0.7f) },
            { MapColorType.Lilpa, new Color(0.7f, 0.7f, 0.7f) },
            { MapColorType.Jururu, new Color(0.7f, 0.7f, 0.7f) },
            { MapColorType.Gosegu, new Color(0.7f, 0.7f, 0.7f) },
            { MapColorType.Viichan, new Color(0.7f, 0.7f, 0.7f) },
        };
        
        public MapColorTypeColorDictionary mapBoxBgColors = new ()
        {
            { MapColorType.UnClear, new Color(0.7f, 0.7f, 0.7f) },
            { MapColorType.Default, Color.white },
            { MapColorType.Ine, new Color(0.7f, 0.7f, 0.7f) },
            { MapColorType.Jingburger, new Color(0.7f, 0.7f, 0.7f) },
            { MapColorType.Lilpa, new Color(0.7f, 0.7f, 0.7f) },
            { MapColorType.Jururu, new Color(0.7f, 0.7f, 0.7f) },
            { MapColorType.Gosegu, new Color(0.7f, 0.7f, 0.7f) },
            { MapColorType.Viichan, new Color(0.7f, 0.7f, 0.7f) },
        };
        #endregion
        
        private void Awake()
        {
            _gridPerUnit = ppu / (float)pixelPerGrid;
            _mapImgFogOfWars = new();
            _mapBoxBg = new();
            FogOfWar.BrushTex = brushTex;
            FogOfWar.MaxMat = brushMaxMat;
            FogOfWar.BrushRectRatio = new Vector2(brushTex.width / (float)brushOriginPixel.x,
                brushTex.height / (float)brushOriginPixel.y);
        }
        
        #region Calc
        private Color GetColor(MapBoxPermanentData.MapBoxData boxData)
        {
            return boxData.useOwnColor ? boxData.color : mapColors[boxData.colorType];
        }
        private Color GetBgColor(MapBoxPermanentData.MapBoxData boxData)
        {
            return boxData.useOwnColor ? boxData.color : mapBoxBgColors[boxData.colorType];
        }

        private (Vector2, Vector2) GetMapPosBoxRect(MapBoxPermanentData.MapBoxData boxData)
        {
            Vector2 leftDown = Map.CalculateWorldPosToMapPos(boxData.position - boxData.size * 0.5f);
            Vector2 rightUp = Map.CalculateWorldPosToMapPos(boxData.position + boxData.size * 0.5f);
            return (leftDown, rightUp);
        }

        /// <summary>
        /// mapPos를 받아 fogOfWar에 사용되는 gridIndex를 반환합니다.
        /// </summary>
        private MapDrawerRect GetGridIndByMapPos((Vector2, Vector2) mapPos)
        {
            float under = Map.ratio * pixelPerGrid;
            (Vector2, Vector2) rectPos = (mapPos.Item1 / under, mapPos.Item2 / under);
            return new MapDrawerRect(){
                sX = Mathf.FloorToInt(rectPos.Item1.x),
                sY = Mathf.FloorToInt(rectPos.Item1.y),
                eX = Mathf.FloorToInt(rectPos.Item2.x),
                eY = Mathf.FloorToInt(rectPos.Item2.y)
            };
        }
        #endregion

        
        
        #region MapImg

        public void RevealRect((Vector2, Vector2) fRect)
        {
            if (_curPlayerMapImg == null) return;
            // Debug.Log($"reveal {fRect} {_curPlayerMapImg.LeftDownPos}");
            Vector2 ld = Map.CalculateWorldPosToMapPos(fRect.Item1) - _curPlayerMapImg.LeftDownPos;
            Vector2 ru = Map.CalculateWorldPosToMapPos(fRect.Item2) - _curPlayerMapImg.LeftDownPos;
            var rect = GetGridIndByMapPos((ld, ru));
            // Debug.Log($"Reveal rect {Map.CalculateWorldPosToMapPos(fRect.Item1)} {Map.CalculateWorldPosToMapPos(fRect.Item2)} / {rect.sX} {rect.sY} {rect.eX} {rect.eY}");
            _curPlayerMapImg?.Fog.PaintReveal(rect);
        }

        public void DrawMapImg(int mapImgId, bool isClear)
        {
            DrawMapImg(ImgDatas[mapImgId], isClear);
        }

        public void ApplyMapImgTexture(int mapImgId, Texture2D tex, bool isReveal)
        {
            if (_mapImgFogOfWars.TryGetValue(mapImgId, out var mapImg))
            {
                mapImg.Fog.CopyTexture(tex, isReveal);
            }
        }
        
        private MapImg DrawMapImg(MapImgPermanaentData mapImgData, bool isClear)
        {
            if (!_mapImgFogOfWars.TryGetValue(mapImgData.mapImgId, out MapImg mapImg))
            {
                GameObject go = new GameObject($"MapImg{mapImgData.mapImgId}");
                go.transform.SetParent(mapImgParent);
                RectTransform rt = go.AddComponent<RectTransform>();
                Image img = go.AddComponent<Image>();
                FogOfWar newFog = go.AddComponent<FogOfWar>();
                mapImg = go.AddComponent<MapImg>();
            
                
                mapImg.Img = img;
                
                // 개별 material을 위해 새로 생성하여 넣어주기.
                newFog.Mat = isClear ? null : new Material(fogMat);
                newFog.IsClear = isClear;
                
                img.sprite = mapImgData.mapImg;
                img.material = newFog.Mat;
                
                int fogWidth = Mathf.RoundToInt(mapImgData.mapImg.textureRect.size.x / pixelPerGrid);
                int fogHeight = Mathf.RoundToInt(mapImgData.mapImg.textureRect.size.y / pixelPerGrid);
                newFog.Init(fogWidth, fogHeight);
            
                // Debug.Log($"pivot {new Vector2(mapImgData.mapImg.pivot.x / mapImgData.mapImg.rect.width, mapImgData.mapImg.pivot.y / mapImgData.mapImg.rect.height)}");
                rt.pivot = new Vector2(
                    mapImgData.mapImg.pivot.x / mapImgData.mapImg.rect.width,
                    mapImgData.mapImg.pivot.y / mapImgData.mapImg.rect.height
                );
                rt.sizeDelta = mapImgData.mapImg.rect.size;
                rt.localScale = Vector3.one * Map.ratio;
                rt.localPosition = Vector3.zero;
                rt.anchoredPosition = Map.CalculateWorldPosToMapPos(mapImgData.mapImgPivotWorldPos);
                mapImg.PositionUpdate();
            
                _mapImgFogOfWars.Add(mapImgData.mapImgId, mapImg);
            }

            if (isClear)
            {
                mapImg.Fog.IsClear = true;
                mapImg.Img.material = null;
            }
            
            return mapImg;
        }        

        #endregion
        

        
        #region MapBox

        public MapDrawerRect InitMapBox(int mapBoxId)
        {
            MapImg mapImg = GetOrCreateMapImg(mapBoxId);
            _curPlayerFogMapPos = mapImg.LeftDownPos;
            _curPlayerMapImg = mapImg;
            if (Map.instance.WasEntered(mapBoxId)) return default;
            if (!BoxDatas.TryGetValue(mapBoxId, out MapBoxPermanentData mapBoxData)) return default;
            MapDrawerRect rect = GetGridIndByMapPos(GetMapPosBoxRect(mapBoxData.mapBoxData));
            // _curPlayerMapImg.Fog.ColorSector(rect, mapColors[MapColorType.UnClear]);
            DrawMapBoxBg(mapBoxData, false);
            return rect;
        }
        
        public void ClearMapBox(int mapBoxId)
        {
            MapBoxPermanentData mapBoxData = BoxDatas[mapBoxId];
            MapImg mapImg = GetOrCreateMapImg(mapBoxData.mapBoxId);
            (Vector2, Vector2) leftRight = GetMapPosBoxRect(mapBoxData.mapBoxData);
            
            MapDrawerRect rect = GetGridIndByMapPos((Vector2.zero, leftRight.Item2-leftRight.Item1));
            
            mapImg.Fog.ColorSector(rect);
            mapImg.Fog.PaintReveal(rect);
            DrawMapBoxBg(mapBoxData, true);
        }

        public void DrawMapBoxBg(int mapBoxId, bool isClear)
        {
            DrawMapBoxBg(BoxDatas[mapBoxId], isClear);
        }
        
        private void DrawMapBoxBg(MapBoxPermanentData mapBoxData, bool isClear)
        {
            if (!_mapBoxBg.TryGetValue(mapBoxData.mapBoxId, out Image img))
            {
                GameObject box = new GameObject("UIBox", typeof(Image));
                box.transform.SetParent(mapBoxParent, false);
            
                RectTransform rect = box.GetComponent<RectTransform>();
                rect.anchoredPosition = Map.CalculateWorldPosToMapPos(mapBoxData.mapBoxData.position);
                rect.sizeDelta = Map.CalculateWorldScaleToMapScale(mapBoxData.mapBoxData.size);
            
                img = box.GetComponent<Image>();
                img.sprite = mapBoxBgSprite;
                img.type = Image.Type.Sliced;
                _mapBoxBg.Add(mapBoxData.mapBoxId, img);
            }
            img.color = isClear ? GetBgColor(mapBoxData.mapBoxData) : mapBoxBgColors[MapColorType.UnClear];
        }

        #endregion
        
        

        #region utilSection

        public MapImgPermanaentData GetMapImgData(int getMapBoxId)
        {
            foreach (var (_, img) in ImgDatas)
            {
                if (img.mapBoxIds == null) continue;
                foreach (var fromTo in img.mapBoxIds)
                {
                    if ((fromTo.x <= getMapBoxId && getMapBoxId <= fromTo.y) || (fromTo.x == 0 && fromTo.y == getMapBoxId))
                    {
                        return img;
                    }
                }
            }

            return null;
        }
        
        private MapImg GetOrCreateMapImg(int mapBoxId)
        {
            MapImgPermanaentData imgData = GetMapImgData(mapBoxId);
            if (!_mapImgFogOfWars.TryGetValue(imgData.mapImgId, out var mapImg))
            {
                Map.instance.OpenedMapImg.TryAdd(imgData.mapImgId, false);
                mapImg = DrawMapImg(imgData, false);
            }
            return mapImg;
        }

        #endregion


        public int mapBoxId;

        [Button(50)]
        public void ClearMapBoxButton()
        {
            ClearMapBox(mapBoxId);
        }

    }
}