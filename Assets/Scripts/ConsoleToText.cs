using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ConsoleToText : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_Text debugText;
    string output = "";
    string stack = "";

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
        Debug.Log("Log enabled!");
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
        Clearlog();
    }

    void HandleLog(string logString, string stackTrace, LogType type) { 
       
    output = logString + "\n" + output;
        stack = stackTrace;
    }

    private void OnGUI()
    {
        debugText.text = output;
    }

    public void Clearlog()
    {

        output = "";
    }
    
}
