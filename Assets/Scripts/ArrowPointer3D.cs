using UnityEngine;

public class ArrowPointer3D : MonoBehaviour
{
    public Transform target;              // Ҫָ�������
    public float heightAboveTarget = 1.5f;
    public Camera mainCamera;
    public bool faceCamera = false;       // �Ƿ��ü�ͷ�������
    public bool showOnlyInView = false;   // ����ɼ�ʱ����ʾ

    private Renderer arrowRenderer;

    void Start()
    {
        arrowRenderer = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (target == null || mainCamera == null) return;

        // ���ü�ͷ�������Ϸ�
        transform.position = target.position + Vector3.up * heightAboveTarget;

        // ��ͷָ��Ŀ�꣨���£�
        transform.rotation = Quaternion.LookRotation(
            target.position - transform.position
        );

        // ��ѡ����ͷ�泯�����billboard Ч����
        if (faceCamera)
        {
            Vector3 dir = mainCamera.transform.position - transform.position;
            dir.y = 0; // ֻ��ˮƽ������ת����ѡ��
            transform.forward = -dir.normalized;
        }

        // ��ѡ������Ŀ������Ұ��ʱ��ʾ��ͷ
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
