using UnityEngine;
using UnityEngine.UI;

public class TransitionZone : MonoBehaviour
{
    public Transform target;
    public Image leftIndicator;
    public Image rightIndicator;
    public RectTransform left;
    public RectTransform right;
    public Camera mainCamera;
    public float maxViewAngle = 45f;
    public float distanceFromCamera = 2f;

    void Update()
    {
        Vector3 directionToTarget = target.position - mainCamera.transform.position;
        Vector3 forward = mainCamera.transform.forward;

        float angle = Vector3.SignedAngle(forward, directionToTarget, Vector3.up);

        Vector3 viewPosition = new Vector3(0.2f, 0.5f, distanceFromCamera);
        left.position = mainCamera.ViewportToWorldPoint(viewPosition);
        //NeedleUI.rotation = Quaternion.LookRotation(NeedleUI.position - mainCamera.transform.position);
        left.LookAt(mainCamera.transform);


        Vector3 viewPosition2 = new Vector3(0.8f, 0.5f, distanceFromCamera);
        right.position = mainCamera.ViewportToWorldPoint(viewPosition2);
        //NeedleUI.rotation = Quaternion.LookRotation(NeedleUI.position - mainCamera.transform.position);
        right.LookAt(mainCamera.transform);


        if (Mathf.Abs(angle) < maxViewAngle)
        {
            float normalized = Mathf.InverseLerp(maxViewAngle, 0f, Mathf.Abs(angle));
            Color barColor = new Color(1, 0, 0, normalized);

            if (angle < 0) // target left
            {
                leftIndicator.color = barColor;
                rightIndicator.color = new Color(1, 0, 0, 0);
            }
            else // target right
            {
                rightIndicator.color = barColor;
                leftIndicator.color = new Color(1, 0, 0, 0);
            }
        }
        else
        {
            leftIndicator.color = new Color(1, 0, 0, 0);
            rightIndicator.color = new Color(1, 0, 0, 0);
        }
    }
}