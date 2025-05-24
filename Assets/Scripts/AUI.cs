using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class AUI : MonoBehaviour
{
    public Transform cube;
    public TMP_Text statusText;

    void Update()
    {
        if (cube != null && statusText != null)
        {
            statusText.text = "Cube Position:\n" + cube.position.ToString("F3");
        }
    }
}
