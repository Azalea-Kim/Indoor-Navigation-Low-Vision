using Meta.XR.BuildingBlocks;
using UnityEngine;
using UnityEngine.UI;


public class Needles : MonoBehaviour
{
    public Transform target;              // The object to point to
    public Camera mainCamera;             // XR rig camera or center eye anchor
    public RectTransform NeedleUI;         // The UI element in a World Space Canvas
    public float distanceFromCamera = 2f; // How far in front of camera to place arrow
    private Image needleImage;
    public Color visibleColor = Color.white;
    public float inViewAngle = 30f;
    public float nearFrustumAngle = 45f;
    private float currentX = 0f;
    private float xVelocity = 0f;


    void Start()
    {
        needleImage = NeedleUI.GetComponent<Image>();
        if (needleImage == null)
        {
            Debug.LogError("arrowUI must have an Image component.");
        }

        Debug.Log("Unity debug log test");


    }

    void Update()
    {
        Vector3 toTarget = target.position - mainCamera.transform.position;
        Vector3 camForward = mainCamera.transform.forward;
        float angleToTarget = Vector3.Angle(mainCamera.transform.forward, toTarget);

        bool isInFront = Vector3.Dot(camForward, toTarget) > 0;

        Vector3 screenPoint = mainCamera.WorldToScreenPoint(target.position);
        screenPoint.z = distanceFromCamera;

        Vector3 camScreenPos = mainCamera.WorldToScreenPoint(mainCamera.transform.position);
        Vector2 screenCenter = new Vector2(camScreenPos.x, camScreenPos.y);

        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(target.position);


        bool isOnScreen = isInFront &&
                       viewportPoint.x > 0 && viewportPoint.x < 1 &&
                      viewportPoint.y > 0 && viewportPoint.y < 1;
        // bool isOnScreen = isInFront && (angleToTarget < inViewAngle);


        if (isOnScreen)
        {
            needleImage.color = visibleColor;
            // Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPoint);
            // NeedleUI.position = worldPosition;
            // Make arrow face the camera
            // NeedleUI.rotation = Quaternion.LookRotation(NeedleUI.position - mainCamera.transform.position);

            //Vector3 forwardBase = mainCamera.transform.position + camForward * distanceFromCamera;
            //Vector3 upwardOffset = mainCamera.transform.up * 0.6f; // 可调节这个高度
            // NeedleUI.position = forwardBase + upwardOffset;
            Vector3 viewPosition = new Vector3(0.5f, 0.5f, distanceFromCamera);
            Vector3 basePos = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distanceFromCamera));
            NeedleUI.position = mainCamera.ViewportToWorldPoint(viewPosition);
            //NeedleUI.rotation = Quaternion.LookRotation(NeedleUI.position - mainCamera.transform.position);
            NeedleUI.LookAt(mainCamera.transform);

            // 目标与正前的水平夹角（带符号）
            float signedYaw =
                Vector3.SignedAngle(camForward,
                                    Vector3.ProjectOnPlane(toTarget, mainCamera.transform.up),
                                    mainCamera.transform.up); // +右 -左

            float normalized = Mathf.Clamp(signedYaw / inViewAngle, -1f, 1f);
            float maxOffset = 0.2f;                       // 世界坐标最大位移
            float offsetX = normalized * maxOffset;      // 世界空间偏移

            Vector3 finalPos = basePos + Vector3.ProjectOnPlane(mainCamera.transform.right, Vector3.up).normalized * offsetX;
            NeedleUI.position = finalPos;

        }
        else
        {

            needleImage.color = new Color(1f, 1f, 1f, 0f); // 30% 透明度
        }
    }
}

