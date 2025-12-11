using chamwhy;
using UnityEngine;
using Cinemachine;
using Managers;

public class PlayerCameraReveal : MonoBehaviour
{
    public MapImgDrawer drawer;

    private Camera mainCam;

    void Start()
    {
        mainCam = CameraManager.instance.MainCam;
    }

    void Update()
    {
        RevealVisibleGrid();
    }

    void RevealVisibleGrid()
    {
        // Debug.Log($"Reveal visible grid {mainCam == null} {drawer == null}");
        if (mainCam == null || drawer == null) return;
        // Debug.Log("Reveal visible grid2");
        // 1. 카메라의 Frustum Corners (시야 범위) 계산
        Vector3[] frustumCorners = new Vector3[4];
        mainCam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), mainCam.nearClipPlane + Mathf.Abs(mainCam.transform.position.z), Camera.MonoOrStereoscopicEye.Mono, frustumCorners);
        
        // 2. 월드 좌표 변환
        for (int i = 0; i < 4; i++)
            frustumCorners[i] = mainCam.transform.TransformPoint(frustumCorners[i]);

        // 3. z=0 평면에서 XY 좌표 변환
        Vector2 bottomLeft = ProjectToZ0(frustumCorners[0], mainCam.transform.position);
        Vector2 topRight = ProjectToZ0(frustumCorners[2], mainCam.transform.position);

        drawer.RevealRect((bottomLeft, topRight));
    }

    Vector2 ProjectToZ0(Vector3 worldPos, Vector3 camPos)
    {
        if (Mathf.Approximately(worldPos.z, camPos.z)) return new Vector2(worldPos.x, worldPos.y);

        float t = (0 - camPos.z) / (worldPos.z - camPos.z);
        return new Vector2(
            camPos.x + t * (worldPos.x - camPos.x),
            camPos.y + t * (worldPos.y - camPos.y)
        );
    }
}
