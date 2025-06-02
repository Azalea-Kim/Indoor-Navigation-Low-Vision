using Meta.XR.BuildingBlocks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testButton : MonoBehaviour
{

    void Update()
    {
        // Button.One = A button on right controller
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            Debug.Log("[INPUT] Button Two pressed - Reload anchors");
            
        }

        // Button.Three = X button on left controller
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            Debug.Log("[INPUT] Button Three pressed - Erase anchors");
        }
    }
}
