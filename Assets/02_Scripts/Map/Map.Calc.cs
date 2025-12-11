using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using _02_Scripts.UI.UI_SubItem;
using UnityEngine;

namespace chamwhy
{
    public partial class Map
    {
        // 미니맵 캡처 이미지 크기 (pixel 기준 기준)
        public const float ratio = 5;

        private static float mapRatio = 2.523f;

        public static Vector2 CalculateWorldPosToMapPos(Vector2 worldPos)
        {
            /*
            float worldWidth = worldMax.x - worldMin.x;
            float worldHeight = worldMax.y - worldMin.y;

            float normalizedX = (worldPos.x - worldMin.x) / worldWidth; // 0~1로 정규화
            float normalizedY = (worldPos.y - worldMin.y) / worldHeight;

            float mapX = normalizedX * miniMapSize.x;
            float mapY = normalizedY * miniMapSize.y;
            */

            return worldPos * (mapRatio * ratio);
        }

        public static Vector2 CalculateWorldScaleToMapScale(Vector2 worldScale)
        {
            /*
            float worldWidth = worldMax.x - worldMin.x;
            float worldHeight = worldMax.y - worldMin.y;

            float scaleX = worldScale.x * (miniMapSize.x / worldWidth);
            float scaleY = worldScale.y * (miniMapSize.y / worldHeight);
*/
            return worldScale * (mapRatio * ratio);
        }


        public void OpenSector(int sectorId)
        {
            // GameManager.SectorMag.loadedSectors
        }
    }
}