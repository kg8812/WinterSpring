using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace chamwhy
{
    public struct MarkerSaveData
    {
        public Vector2 position;
        public MarkerType markerType;
    }
    public enum MarkerType
    {
        Player, UnlockBox, DropItem, Janhyang, Shop, Shelter, Npc, UnopenableObstacle, OpenableObstacle
    }
    public class Marker: MonoBehaviour
    {
        public MarkerType markerType;

        private Action<Vector2> _onPos;

        public Action<Vector2> OnPositionChanged
        {
            get => _onPos;
            set
            {
                _onPos = value;
                // Debug.Log("onposition 수정됨.");
            }
        }
        public Action OnDeactivated;

        [SerializeField] private bool activeOnAwake;
        [SerializeField] private bool isSectorObject = true;
        [HideIf("isSectorObject", true)][SerializeField] private string key;
        public string Key { get; private set; }
        

        private IMarkerObject _mo;
        private Vector2 _myPos;
        private Transform _myTrans;
        private SectorObject _so;
        private bool activated;
        

        private void Awake()
        {
            _myTrans = transform;
            _mo = GetComponent<IMarkerObject>();
            Key = isSectorObject ? _mo.Key : Guid.NewGuid().ToString();
            if (isSectorObject)
            {
                _so = GetComponent<SectorObject>();
                _so.OnActive += RemoveMarker;
            }
        }

        private void OnEnable()
        {
            if(activeOnAwake)
                Activate();
        }

        public void Activate()
        {
            activated = true;
            Map.instance.RegisterMarker(this);
            PositionUpdate(true);
        }

        private void Update()
        {
            if(activated)
                PositionUpdate();
        }

        public void PositionUpdate(bool force = false)
        {
            // Debug.Log($"{transform.parent.name} marker obj");
            if (force || (Vector2)_myTrans.position != _myPos)
            {
                _myPos = _myTrans.position;
                // Debug.Log($"{transform.parent.name} marker obj2 {OnPositionChanged != null}");
                OnPositionChanged?.Invoke(_myPos);
            }
        }

        private void RemoveMarker()
        {
            activated = false;
            if (GameManager.IsQuitting) return;
            Map.instance.RemoveMarker(this);
            OnPositionChanged = null;
            OnDeactivated?.Invoke();
            OnDeactivated = null;
            if (isSectorObject)
            {
                _so.OnActive -= RemoveMarker;
            }
        }

        private void OnDisable()
        {
            RemoveMarker();
        }
    }
}