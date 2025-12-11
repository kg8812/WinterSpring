using Cinemachine;
using Managers;
using Sirenix.Utilities;
using UnityEngine;

namespace Directing
{
    [RequireComponent(typeof(CinemachineTargetGroup))]
    public class TargetGroupCamera: Singleton<TargetGroupCamera>
    {
        private CinemachineTargetGroup _citg;

        protected override void Awake()
        {
            base.Awake();
            _citg = GetComponent<CinemachineTargetGroup>();
        }


        public void RegisterTarget(Transform trans, float weight = 1, float radius = 1)
        {
            _citg.AddMember(trans, weight, radius);
            CameraManager.instance.UpdateConfinerMaxDistance();
        }

        public void RemoveTarget(Transform trans)
        {
            _citg.RemoveMember(trans);
            CameraManager.instance.UpdateConfinerMaxDistance();
        }

        public void ResetTargets()
        {
            foreach (var t in _citg.m_Targets)
            {
                RemoveTarget(t.target);
            }
        }

        public void DoUpdate()
        {
            _citg.DoUpdate();
        }

        public void AdjustTargetRadius(Transform trans, float radius)
        {
            for (int i = 0; i < _citg.m_Targets.Length; i++)
            {
                if (_citg.m_Targets[i].target == trans)
                {
                    _citg.m_Targets[i].radius = radius;
                }
            }
            CameraManager.instance.UpdateConfinerMaxDistance();
        }
    }
}