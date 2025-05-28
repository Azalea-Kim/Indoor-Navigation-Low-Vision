using UnityEngine;
using Oculus.Haptics;

public class HapticController : MonoBehaviour
{
    public HapticClip hapticClip;
    private HapticClipPlayer hapticPlayer;

    public Transform target;
    public float maxDistance = 20f;

    // Enum to select which hand this script is on
    public enum Hand { Left, Right }
    public Hand hand;

    void Start()
    {
        hapticPlayer = new HapticClipPlayer(hapticClip);
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.transform == target)
            {
                PlayHaptics();
            }
        }
    }

    public void PlayHaptics()
    {
        if (hand == Hand.Right)
        {
            hapticPlayer.Play(Controller.Right);
        }
        else
        {
            hapticPlayer.Play(Controller.Left);
        }
    }

    private void OnDestroy()
    {
        hapticPlayer?.Dispose();
    }
}
