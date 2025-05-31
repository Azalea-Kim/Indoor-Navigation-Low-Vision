using UnityEngine;
using UnityEngine.UI;

public class AudioToggleController : MonoBehaviour
{
    public Toggle toggle;
    public OutOfViewGuidance guidance;

    void Start()
    {
        if (toggle == null || guidance == null)
        {
            Debug.LogError("Toggle or Guidance script not assigned.");
            return;
        }

        // ��ʼ��ͬ��
        guidance.audioEnabledByToggle = toggle.isOn;

        toggle.onValueChanged.AddListener((isOn) =>
        {
            guidance.audioEnabledByToggle = isOn;
        });
    }
}
