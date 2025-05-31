using UnityEngine;
using UnityEngine.UI;

public class ToggleControlObject : MonoBehaviour
{
    public Toggle myToggle;
    public GameObject targetObject;

    void Start()
    {
        // 确保初始状态正确
        targetObject.SetActive(myToggle.isOn);

        // 绑定监听器
        myToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        // 当 toggle 被取消，隐藏对象
        targetObject.SetActive(isOn);
    }
}
