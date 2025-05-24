using Meta.XR.BuildingBlocks;
using UnityEngine;
using UnityEngine.UI;


public class Needle : MonoBehaviour
{
    public Transform target;              // The object to point to
    public Camera mainCamera;             // XR rig camera or center eye anchor
    public RectTransform NeedleUI;         // The UI element in a World Space Canvas
    public float distanceFromCamera = 2f; // How far in front of camera to place arrow

    private Image needleImage;

    public float inViewAngle = 30f;
    public float nearFrustumAngle = 45f;


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

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPoint);
            NeedleUI.position = worldPosition;
            // Make arrow face the camera
            NeedleUI.rotation = Quaternion.LookRotation(NeedleUI.position - mainCamera.transform.position);
        }
    }
}