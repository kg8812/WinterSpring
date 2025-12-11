using System.Collections.Generic;
using System.IO;
using System.Linq;
using chamwhy.Model;
using Default;
using Save.Schema;
using UnityEngine;

namespace chamwhy
{
    public partial class Map
    {
        // 열려있는 sector와, 클리어된 sector는 다름.
        public Dictionary<int, MapBoxPermanentData> OpenedSectorSet { get; set; }
        public Dictionary<int, bool[,]> UnclearedSectorMapImgGrids { get; set; }


        public Dictionary<int, MapBoxPermanentData> BoxDatas { get; private set; }
        public Dictionary<int, MapImgPermanaentData> ImgDatas { get; private set; }

        // openedMapImg, mapBox bool은 clear 여부
        public Dictionary<int, bool> OpenedMapImg
            => GameManager.Save.currentSlotData?.MapSaveData?.OpenedMapImg;

        private Dictionary<int, bool> OpenedMapBox
            => GameManager.Save.currentSlotData?.MapSaveData?.OpenedMapBox;

        private Dictionary<int, HashSet<string>> ObtainedObjects
            => GameManager.Save.currentSlotData?.MapSaveData?.objectObtain;

        public Dictionary<int, SectorSaveData.ShelterData> OpenedShelters
            => GameManager.Save.currentSlotData?.MapSaveData?.openedShelters;

        public int ClearedMapBoxCnt => OpenedMapBox.Values.Count(v => v);
        public float Progress => ClearedMapBoxCnt / (float)BoxDatas.Count;

        public void DataInit()
        {
            BoxDatas = ResourceUtil.LoadAll<MapBoxPermanentData>("ScriptableObject/Map/MapBoxData")
                .ToDictionary(keySelector: x => x.mapBoxId);
            ImgDatas = ResourceUtil.LoadAll<MapImgPermanaentData>("ScriptableObject/Map/MapImgData")
                .ToDictionary(keySelector: x => x.mapImgId);
            
        }


        #region 맵 이미지 텍스처 저장

        private (string, string) GetFileName(int mapImgId)
        {
            return ($"MapImg{GameManager.Save.currentSlotData.slotId}-{mapImgId}_Reveal.png",
                $"MapImg{GameManager.Save.currentSlotData.slotId}-{mapImgId}_Color.png");
        }

        public void SaveRenderTexture(string fileName, RenderTexture renderTexture)
        {
            // RenderTexture → Texture2D 변환
            Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();
            RenderTexture.active = null; // 활성화 해제

            // PNG로 변환
            byte[] pngData = tex.EncodeToPNG();
            Destroy(tex); // Texture2D 메모리 해제

            // 저장 경로 설정
            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllBytes(path, pngData);

            //Debug.Log($"✅ RenderTexture 저장 완료: {path}");
        }

        private Texture2D LoadTextureFromFile(string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);

            if (!File.Exists(path))
            {
                //Debug.LogError("저장된 텍스처 파일이 없습니다.");
                return null;
            }

            byte[] pngData = File.ReadAllBytes(path);

            Texture2D tex = new Texture2D(2, 2); // LoadImage()에서 크기가 자동 조정됨
            tex.LoadImage(pngData);

            return tex;
        }

        #endregion
    }
}