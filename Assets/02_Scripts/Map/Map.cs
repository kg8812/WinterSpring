using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace chamwhy
{
    public partial class Map: SingletonPersistent<Map>
    {
        public HashSet<int> EnteredMapBox { get; private set; }
        public bool UpdateToPlayer { get; set; }
        private MapImgDrawer _drawer;
        public RectTransform moveRect;
        [SerializeField] private RectTransform mapObj;
        public int CurMapBox { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _drawer = GetComponent<MapImgDrawer>();
            EnteredMapBox = new();
            
        }

        public bool WasEntered(int mapBoxId) => EnteredMapBox.Contains(mapBoxId);

        public void AddEnteredMapBox(int mapBoxId, bool updateData = true)
        {
            if (EnteredMapBox.Add(mapBoxId) && updateData)
            {
                OpenedMapBox.TryAdd(mapBoxId, false);
            }
        }

        public void EnterMapBox(int mapBoxId)
        {
            CurMapBox = mapBoxId;
            _drawer.InitMapBox(mapBoxId);
            AddEnteredMapBox(mapBoxId);
        }
        
        
        public void InitByData()
        {
            if (OpenedMapBox != null)
            {
                foreach (var (mapBoxId, isClear) in OpenedMapBox)
                {
                    // Debug.Log($"map box init {mapBoxId} {isClear}");
                    AddEnteredMapBox(mapBoxId, false);
                    // null이면 해당 map box clear
                    _drawer.DrawMapBoxBg(mapBoxId, isClear);
                }
            }

            if (OpenedMapImg != null)
            {
                foreach (var (mapImgId, isClear) in OpenedMapImg)
                {
                    // Debug.Log($"map img init {mapImgId} {isClear}");
                    // null이면 해당 map box clear
                    _drawer.DrawMapImg(mapImgId, isClear);
                    if(!isClear)
                    {
                        (string, string) fileNames = GetFileName(mapImgId);
                        Texture2D revealTex = LoadTextureFromFile(fileNames.Item1);
                        Texture2D colorTex = LoadTextureFromFile(fileNames.Item2);
                        _drawer.ApplyMapImgTexture(mapImgId, revealTex, true);
                        _drawer.ApplyMapImgTexture(mapImgId, colorTex, false);
                    }
                    
                }
            }
        }

        public void SaveAllMapImgTexture()
        {
            foreach (var (id, mapImg) in _drawer.MapImgs)
            {
                (string, string) fileNames = GetFileName(id);
                SaveRenderTexture(fileNames.Item1, mapImg.Fog.RevealTex);
                SaveRenderTexture(fileNames.Item2, mapImg.Fog.ColorTex);
            }
        }

        private void Update()
        {
            if(UpdateToPlayer)
                MoveToPlayer();
        }

        public void MoveToPlayer()
        {
            if (GameManager.instance.Player != null)
            {
                MoveToWorld(GameManager.instance.ControllingEntity.Position);
            }
        }

        public void MoveToWorld(Vector2 worldPos)
        {
            moveRect.anchoredPosition = -CalculateWorldPosToMapPos(worldPos);
        }

        public void MoveTo(Vector2 pos)
        {
            moveRect.anchoredPosition = pos;
        }

        public void Move(Vector2 addPos)
        {
            moveRect.anchoredPosition += addPos;
        }

        public RectTransform SetParent(RectTransform parent)
        {
            mapObj.SetParent(parent, false);
            return mapObj;
        }





        public void ObtainedObject(string guid)
        {
            foreach (var (_, box) in BoxDatas)
            {
                if (box.requiredObtainableGuids.Contains(guid))
                {
                    ObtainedObjects.TryAdd(box.mapBoxId, new HashSet<string>());
                    if (ObtainedObjects[box.mapBoxId].Add(guid))
                    {
                        if (ObtainedObjects[box.mapBoxId].Count == box.requiredObtainableGuids.Length)
                        {
                            if (OpenedMapBox.ContainsKey(box.mapBoxId))
                            {
                                ClearedMapBox(box.mapBoxId);
                            }
                            else
                                Debug.LogError($"{box.mapBoxId} mapbox가 opened에 없는데 획득 요소 획득함.");
                            _drawer.ClearMapBox(box.mapBoxId);
                        }
                    }
                    break;
                }
            }
        }

        private void ClearedMapBox(int mapBoxId)
        {
            // mapBox data section
            OpenedMapBox[mapBoxId] = true;
            
            // mapImg data section
            MapImgPermanaentData data = _drawer.GetMapImgData(mapBoxId);
            if (OpenedMapImg.TryGetValue(data.mapImgId, out bool isClear) && !isClear)
            {
                foreach (var mapBoxIdRange in data.mapBoxIds)
                {
                    if (mapBoxIdRange.x == 0)
                    {
                        // x가 0이면 y 고정 mapBoxId
                        // open 되지 않은 mapBox가 존재하거나 open되었지만 clear가 되어있지 않은 상태
                        if (!OpenedMapBox.TryGetValue(mapBoxIdRange.y, out var boxClear) || !boxClear)
                            return;
                    }
                    else
                    {
                        // 아니라면 [x, y] 범위로 mapBoxId
                        for (int i = mapBoxIdRange.x; i <= mapBoxIdRange.y; i++)
                        {
                            if (!OpenedMapBox.TryGetValue(i, out var boxClear) || !boxClear)
                                return;
                        }
                    }
                }
                
                // 모두 clear라 return되지 않고 실행.
                OpenedMapImg[data.mapImgId] = true;
            }
        }
    }
}