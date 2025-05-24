using UnityEngine;
using UnityEngine.XR;
using Unity.XR.Oculus;
using UnityEngine.XR.Management;


public class Track : MonoBehaviour
{
    void Awake()
    {
        OVRPlugin.SetTrackingOriginType(OVRPlugin.TrackingOrigin.FloorLevel);
        
        Debug.Log("[Boot] trackingOriginType set to FloorLevel");
    }
}
