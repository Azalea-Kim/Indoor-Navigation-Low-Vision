using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NeedlesToggle : MonoBehaviour
{
    public Toggle toggle;
    public Needles needle;
    public NeedleBG needlebg;

    void Start()
    {
        SetNeedlesEnabled(toggle.isOn);
        toggle.onValueChanged.AddListener(SetNeedlesEnabled);
    }

    void SetNeedlesEnabled(bool isOn)
    {
        if (needle != null)
        {
            needle.enabledByToggle = isOn;

            if (needle.NeedleUI != null)
            {
                var img = needle.NeedleUI.GetComponent<Image>();
                if (img != null)
                {
                    img.color = isOn ? needle.visibleColor : SetAlpha(needle.visibleColor, 0f);
                }
            }
        }

        if (needlebg != null)
        {
            needlebg.enabledByToggle = isOn;

            if (needlebg.NeedleUI != null)
            {
                var img = needlebg.NeedleUI.GetComponent<Image>();
                if (img != null)
                {
                    img.color = isOn ? needlebg.visibleColor : SetAlpha(needlebg.visibleColor, 0f);
                }
            }

            if (needlebg.backgroundUI != null)
            {
                var bg = needlebg.backgroundUI.GetComponent<Image>();
                if (bg != null)
                {
                    bg.color = isOn ? needlebg.backgroundColor : SetAlpha(needlebg.backgroundColor, 0f);
                }
            }
        }

    }

    void SetImageAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
    Color SetAlpha(Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }

}
