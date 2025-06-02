using UnityEngine;
using UnityEngine.UI;
using Oculus.Haptics;

public class HapticController : MonoBehaviour
{
    public HapticClip hapticClip;
    private HapticClipPlayer hapticPlayer;

    public Transform target;
    public float maxDistance = 20f;

    public enum Hand { Left, Right }
    public Hand hand;

    public Toggle hapticToggle; // 👈 Public toggle in Inspector

    void Start()
    {
        hapticPlayer = new HapticClipPlayer(hapticClip);

        if (hapticToggle != null)
        {
            hapticToggle.onValueChanged.AddListener(SetHapticsEnabledFromToggle);
        }
    }

    void Update()
    {
        if (hapticToggle != null && !hapticToggle.isOn) return;

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
        if (hapticToggle != null && !hapticToggle.isOn) return;

        if (hand == Hand.Right)
        {
            hapticPlayer.Play(Controller.Right);
        }
        else
        {
            hapticPlayer.Play(Controller.Left);
        }
    }

    private void SetHapticsEnabledFromToggle(bool isOn)
    {
        // Optional: log or respond immediately when toggle changes
        Debug.Log("Haptics Enabled: " + isOn);
    }

    private void OnDestroy()
    {
        if (hapticPlayer != null)
        {
            hapticPlayer.Dispose();
        }

        if (hapticToggle != null)
        {
            hapticToggle.onValueChanged.RemoveListener(SetHapticsEnabledFromToggle);
        }
    }
}
