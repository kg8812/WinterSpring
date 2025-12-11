using System.Collections.Generic;
using _02_Scripts.UI.UI_SubItem;
using UnityEngine;

namespace chamwhy
{
    public partial class Map
    {
        
        [SerializeField] private Transform markerParent;
        [System.Serializable]
        public class MarkerTypeSpriteDictionary: UnitySerializedDictionary<MarkerType, Sprite> {}

        public MarkerTypeSpriteDictionary MarkerSprites;
        
        private readonly Dictionary<string, (Marker, UI_Marker)> _markers = new();
        
        
        

        public void RegisterMarker(Marker marker)
        {
            if (_markers.TryGetValue(marker.Key, out var value))
            {
                // ui marker만 등록되어있고, marker는 등록되어있지 않은 상태.
                if (value.Item1 == null)
                {
                    value.Item1 = marker;
                    ConnectMarkerWithUI(marker, value.Item2);
                }
            }
            else
            {
                // 새로운 marker 등록
                UI_Marker markerUi = CreateMarker();
                ConnectMarkerWithUI(marker, markerUi);
                _markers.Add(marker.Key, (marker, markerUi));
            }
        }

        public void RemoveMarker(Marker marker) => RemoveMarker(marker.Key);
        public void RemoveMarker(string key)
        {
            if (_markers.TryGetValue(key, out (Marker, UI_Marker) value))
            {
                value.Item1.OnPositionChanged = null;
                value.Item1.OnDeactivated = null;
                value.Item2.CloseOwn();
                _markers.Remove(key);
            }
        }

        

        private UI_Marker CreateMarker()
        {
            UI_Marker markerUi = GameManager.UI.MakeSubItem("UI_Marker", markerParent) as UI_Marker;
            if (markerUi == null) return null;
            return markerUi;
        }

        private void ResetMarkerWithSaveData(UI_Marker uiMarker, MarkerSaveData markerSaveData)
        {
            uiMarker.SetMarkerType(markerSaveData.markerType);
            uiMarker.UpdatePos(markerSaveData.position);
        }

        private void ConnectMarkerWithUI(Marker marker, UI_Marker uiMarker)
        {
            uiMarker.SetMarkerType(marker.markerType);
            
            marker.OnPositionChanged -= uiMarker.UpdatePos;
            marker.OnPositionChanged += uiMarker.UpdatePos;
            marker.OnDeactivated -= uiMarker.CloseOwn;
            marker.OnDeactivated += uiMarker.CloseOwn;
            
            marker.PositionUpdate(true);
        }
        
        
        public void MarkerReset()
        {
            foreach (var value in _markers)
            {
                if (value.Value.Item1 != null)
                {
                    value.Value.Item1.OnPositionChanged = null;
                    value.Value.Item1.OnDeactivated = null;
                }
                value.Value.Item2.CloseOwn();
            }
            _markers.Clear();
        }
        
        
        
        public void LoadSaveData()
        {
            foreach (var value in GameManager.Save.currentSlotData.MapSaveData.objectFound)
            {
                MakeMarkerWithSaveData(value.Key, value.Value);
            }
        }

        private void MakeMarkerWithSaveData(string key, MarkerSaveData data)
        {
            if (!_markers.ContainsKey(key))
            {
                UI_Marker markerUi = CreateMarker();
                ResetMarkerWithSaveData(markerUi, data);
                _markers.Add(key, (null, markerUi));
            }
        }
        
        
    }
}