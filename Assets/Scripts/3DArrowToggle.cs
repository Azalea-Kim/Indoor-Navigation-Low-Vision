using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowToggle : MonoBehaviour
{
    public Toggle arrowToggle; // UI Toggle
    public List<ArrowPointer3D> arrowsToControl; // ������Ҫ���Ƶļ�ͷ�ű�

    void Start()
    {
        // ��ʼ��ͬ��һ��
        SetArrowVisibility(arrowToggle.isOn);

        // ��Ӽ�����
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
