using UnityEngine;

public class ArrowPointer3D : MonoBehaviour
{
    public Transform target;              // 要指向的物体
    public float heightAboveTarget = 1.5f;
    public Camera mainCamera;
    public bool faceCamera = false;       // 是否让箭头面向相机
    public bool showOnlyInView = false;   // 物体可见时才显示

    private Renderer arrowRenderer;

    void Start()
    {
        arrowRenderer = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (target == null || mainCamera == null) return;

        // 设置箭头在物体上方
        transform.position = target.position + Vector3.up * heightAboveTarget;

        // 箭头指向目标（向下）
        transform.rotation = Quaternion.LookRotation(
            target.position - transform.position
        );

        // 可选：箭头面朝相机（billboard 效果）
        if (faceCamera)
        {
            Vector3 dir = mainCamera.transform.position - transform.position;
            dir.y = 0; // 只在水平方向旋转（可选）
            transform.forward = -dir.normalized;
        }

        // 可选：仅当目标在视野中时显示箭头
        if (showOnlyInView && arrowRenderer != null)
        {
            Vector3 viewPos = mainCamera.WorldToViewportPoint(target.position);
            bool isInView = viewPos.z > 0 &&
                            viewPos.x > 0 && viewPos.x < 1 &&
                            viewPos.y > 0 && viewPos.y < 1;

            arrowRenderer.enabled = isInView;
        }
    }
}
