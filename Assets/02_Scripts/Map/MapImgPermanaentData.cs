using UnityEngine;

namespace chamwhy
{
    [CreateAssetMenu(menuName = "MapImg", fileName = "New MapImg")]
    public class MapImgPermanaentData: ScriptableObject
    {
        public int mapImgId;
        [Tooltip("from to 입니다. 만약 from이 0이면 to와 동일한지만 체크합니다")]
        public Vector2Int[] mapBoxIds;
        public Sprite mapImg;
        public Vector2 mapImgPivotWorldPos;
    }
}