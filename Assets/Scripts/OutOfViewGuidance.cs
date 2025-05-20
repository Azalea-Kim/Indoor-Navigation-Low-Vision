using UnityEngine;
using UnityEngine.UI;


public class OutOfViewGuidance : MonoBehaviour
{
    public Transform target;              // The object to point to
    public Camera mainCamera;             // XR rig camera or center eye anchor
    public RectTransform arrowUI;         // The UI element in a World Space Canvas
    public Sprite arrowSprite;            // Sprite for off-screen arrow
    public Sprite visibleSprite;          // Sprite for on-screen object
    public float distanceFromCamera = 2f; // How far in front of camera to place arrow

    private Image arrowImage;

    public float inViewAngle = 30f;
    public float nearFrustumAngle = 45f;



    bool IsTargetInMainCameraView()
    {
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer == null) return false;

        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        return GeometryUtility.TestPlanesAABB(frustumPlanes, renderer.bounds);
    }



    void Start()
    {
        arrowImage = arrowUI.GetComponent<Image>();
        if (arrowImage == null)
        {
            Debug.LogError("arrowUI must have an Image component.");
        }
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
            if (arrowImage.sprite != visibleSprite)
                arrowImage.sprite = visibleSprite;

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPoint);
            arrowUI.position = worldPosition;
            // Make arrow face the camera
            arrowUI.rotation = Quaternion.LookRotation(arrowUI.position - mainCamera.transform.position);
        }
        else
        {
            if (arrowImage.sprite != arrowSprite)
                arrowImage.sprite = arrowSprite;

            arrowUI.position = mainCamera.transform.position + camForward * distanceFromCamera;

            arrowUI.rotation = Quaternion.LookRotation(mainCamera.transform.forward, Vector3.up);

            Vector3 dirToTarget = target.position - arrowUI.position;
            Vector3 localDir = mainCamera.transform.InverseTransformDirection(dirToTarget);

            float angle = Mathf.Atan2(localDir.y, localDir.x) * Mathf.Rad2Deg;
            arrowUI.Rotate(0, 0, angle - 90, Space.Self);




            // Make arrow face the camera
            //Vector3 direction = (target.position - arrowUI.position).normalized;
            //Quaternion lookRotation = Quaternion.LookRotation(direction);
            //arrowUI.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90);
            //arrowUI.rotation = Quaternion.LookRotation(arrowUI.position - mainCamera.transform.position);
        }
    }
}