using UnityEngine;
using UnityEngine.UI;

public class ToggleControlObject : MonoBehaviour
{
    public Toggle myToggle;
    public GameObject targetObject;

    void Start()
    {
        // ȷ����ʼ״̬��ȷ
        targetObject.SetActive(myToggle.isOn);

        // �󶨼�����
        myToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        // �� toggle ��ȡ�������ض���
        targetObject.SetActive(isOn);
    }
}
