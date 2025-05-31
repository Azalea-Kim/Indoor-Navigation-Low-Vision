using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ToggleRayWithKey : MonoBehaviour
{
    public Transform rayOrigin;
    public GameObject controllerRayVisual;

    void Update()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Screen.width / 2f, Screen.height / 2f)
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        bool hitOVRUI = false;
        foreach (var result in results)
        {
            if (result.module is OVRRaycaster)
            {
                hitOVRUI = true;
                break;
            }
        }

        controllerRayVisual.SetActive(hitOVRUI);
    }
}