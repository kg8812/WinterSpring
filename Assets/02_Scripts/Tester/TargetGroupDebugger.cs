using UnityEngine;
using Cinemachine;

[ExecuteAlways]
public class TargetGroupDebugger : MonoBehaviour
{
    public CinemachineTargetGroup targetGroup;

    private void OnDrawGizmos()
    {
        if (targetGroup == null || targetGroup.m_Targets.Length == 0)
            return;

        // 그룹 전체 위치 계산 (중심점)
        Bounds bounds = new Bounds(targetGroup.m_Targets[0].target.position, Vector3.zero);

        foreach (var member in targetGroup.m_Targets)
        {
            if (member.target == null) continue;

            bounds.Encapsulate(member.target.position + Vector3.one * member.radius);
            bounds.Encapsulate(member.target.position - Vector3.one * member.radius);
        }

        // 경계 상자 그리기
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        // Bounding Sphere 계산
        float boundingRadius = bounds.extents.magnitude;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(bounds.center, boundingRadius);
    }
}