using UnityEngine;
using UnityEngine.Android;

public class AnchorPermissionRequester : MonoBehaviour
{
    void Start()

    {
        Debug.Log("AnchorPermissionRequester running...");
#if UNITY_ANDROID && !UNITY_EDITOR
        // Request ACCESS_ANCHOR_STORAGE permission if not already granted
        if (!Permission.HasUserAuthorizedPermission("com.oculus.permission.ACCESS_ANCHOR_STORAGE"))
        {
            Debug.Log("Requesting ACCESS_ANCHOR_STORAGE...");
            Permission.RequestUserPermission("com.oculus.permission.ACCESS_ANCHOR_STORAGE");
        }
        else
        {
            Debug.Log("ACCESS_ANCHOR_STORAGE already granted.");
        }

        // Request ACCESS_SPACE_STORAGE permission if not already granted
        if (!Permission.HasUserAuthorizedPermission("com.oculus.permission.ACCESS_SPACE_STORAGE"))
        {
            Debug.Log("Requesting ACCESS_SPACE_STORAGE...");
            Permission.RequestUserPermission("com.oculus.permission.ACCESS_SPACE_STORAGE");
        }
        else
        {
            Debug.Log("ACCESS_SPACE_STORAGE already granted.");
        }
#endif
    }
}
