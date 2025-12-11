using chamwhy.Managers;
using Managers;
using UI;
using UnityEngine;

namespace chamwhy.Components
{
    
    public class TA_ConfinerToggle: ITriggerActivate
    {
        private Trigger _triggerComponent;
        private PolygonCollider2D _polygon;
        private BoxCollider2D _boxCollider;

        private bool isPoly;
        private int priority;
        private bool isOn;
        private bool resetConfiner;
        
        public TA_ConfinerToggle(Trigger triggerComponent, PolygonCollider2D polygon, int priority, bool isOn)
        {
            _triggerComponent = triggerComponent;
            _polygon = polygon;
            isPoly = true;
            this.priority = priority;
            this.isOn = isOn;
        }
        
        public TA_ConfinerToggle(Trigger triggerComponent, BoxCollider2D box, int priority, bool isOn)
        {
            _triggerComponent = triggerComponent;
            _boxCollider = box;
            isPoly = false;
            this.priority = priority;
            this.isOn = isOn;
        }
        
        void SetConfiner()
        {
            if (isOn)
            {
                if (isPoly)
                {
                    ConfinerManager.instance.RegisterConfiner(_polygon, priority);
                }
                else
                {
                    ConfinerManager.instance.RegisterConfiner(_boxCollider, priority);
                }
            }
            else
            {
                if (isPoly)
                {
                    ConfinerManager.instance.RemoveConfiner(_polygon);
                }
                else
                {
                    ConfinerManager.instance.RemoveConfiner(_boxCollider);
                }
            }
            CameraManager.instance.ConfinerForceUpdate();
            if (GameManager.SectorMag.IsFirstSector)
            {
                GameManager.SectorMag.IsFirstSector = false;
            }
        }
        public void Activate()
        {
            SetConfiner();
        }
    }
}