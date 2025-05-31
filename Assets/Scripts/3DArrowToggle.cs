using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowToggle : MonoBehaviour
{
    public Toggle arrowToggle; // UI Toggle
    public List<ArrowPointer3D> arrowsToControl; // 所有需要控制的箭头脚本

    void Start()
    {
        // 初始化同步一次
        SetArrowVisibility(arrowToggle.isOn);

        // 添加监听器
        arrowToggle.onValueChanged.AddListener(SetArrowVisibility);
    }

    void SetArrowVisibility(bool isOn)
    {
        foreach (var arrow in arrowsToControl)
        {
            if (arrow != null)
            {
                arrow.arrowVisible = isOn;
            }
        }
    }
}
