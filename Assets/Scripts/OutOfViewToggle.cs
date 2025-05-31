using UnityEngine;
using UnityEngine.UI;

public class OutOfViewToggle : MonoBehaviour
{
    public Toggle toggle;
    public Image targetImage;

    private Color originalColor;

    void Start()
    {
        if (targetImage == null || toggle == null)
        {
            Debug.LogError("Toggle or Image is not assigned.");
            return;
        }

        originalColor = targetImage.color;

        // Sync on start
        SetImageVisible(toggle.isOn);

        // Listen for toggle changes
        toggle.onValueChanged.AddListener(SetImageVisible);
    }

    void SetImageVisible(bool isOn)
    {
        Color c = originalColor;
        c.a = isOn ? originalColor.a : 0f;
        targetImage.color = c;
    }
}
