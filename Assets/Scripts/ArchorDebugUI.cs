using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ArchorDebugUI : MonoBehaviour
{
    public TMP_Text statusText;

    public void SetStatus(string msg)
    {
        if (statusText != null)
        {
            statusText.text = msg;
        }
    }
}
