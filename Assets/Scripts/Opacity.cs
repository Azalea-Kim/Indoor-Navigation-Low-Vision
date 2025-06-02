using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageOpacityController : MonoBehaviour
{
    public Transform target;
    public Image leftIndicator;
    public Camera mainCamera;
    public float maxViewAngle = 45f;
    public float inViewAngle = 30f;
    public float opacity = 0.8f;
    public Color baseColor = Color.black;

    void Update()
    {
        Vector3 directionToTarget = target.position - mainCamera.transform.position;
        Vector3 forward = mainCamera.transform.forward;

        float angle = Vector3.SignedAngle(forward, directionToTarget, Vector3.up);
        if (inViewAngle < Mathf.Abs(angle) && Mathf.Abs(angle) < maxViewAngle)
        {
            Color barColor = new Color(baseColor.r, baseColor.g, baseColor.b, opacity);
            leftIndicator.color = barColor;
        }
        else
        {
            leftIndicator.color = new Color(1, 0, 0, 0);
        }
    }
}