using UnityEngine;
using UnityEngine.UI;

public class TransitionZoneGuidance : MonoBehaviour
{
    public Transform target;
    public Image leftIndicator;
    public Image rightIndicator;
    public Camera mainCamera; 
    public float maxViewAngle = 45f;

    void Update()
    {
        Vector3 directionToTarget = target.position - mainCamera.transform.position;
        Vector3 forward = mainCamera.transform.forward;

        float angle = Vector3.SignedAngle(forward, directionToTarget, Vector3.up);

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