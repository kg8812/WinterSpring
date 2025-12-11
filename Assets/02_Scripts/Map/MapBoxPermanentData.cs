using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace chamwhy
{
    /// <summary>
    /// Map을 이루는 Box들에 대하여 저장하는 스크립터블 오브젝트입니다.
    /// </summary>
    public class MapBoxPermanentData: ScriptableObject
    {
        [Tooltip("만약 섹터라면 sectorId와 같게")]
        public int mapBoxId;
    
        [Serializable]
        public struct MapBoxData
        {
            public Vector2 size;
            public Vector2 position;
            public bool useOwnColor;
            
            [HideIf("useOwnColor", true)] 
            public MapImgDrawer.MapColorType colorType;
            
            [ShowIf("useOwnColor", true)] 
            public Color color;
        }

        public MapBoxData mapBoxData;

        public string[] requiredObtainableGuids;
    }
}