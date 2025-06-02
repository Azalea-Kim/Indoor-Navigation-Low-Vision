using UnityEngine;
using UnityEngine.UI;

public class LowVisionToggle : MonoBehaviour
{
    public Toggle toggle;
    public Image targetImage;
    public float a;

    void Start()
    {
        if (targetImage == null || toggle == null)
        {
            Debug.LogError("Toggle or Image is not assigned.");
            return;
        }

        // Set toggle off and alpha = 0 on start
        toggle.isOn = false;
        SetAlpha(0f);

        // Listen for toggle changes
        toggle.onValueChanged.AddListener(isOn => SetAlpha(isOn ? a : 0f));
    }

    void SetAlpha(float alpha)
    {
        Color c = targetImage.color;
        c.a = alpha;
        targetImage.color = c;
    }
}
