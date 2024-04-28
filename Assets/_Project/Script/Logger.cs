using System;
using UnityEngine;

[Serializable]
public class Logger
{
    public bool showLog;

    public void Log(string message)
    {
        if (!showLog) return;
        
        Debug.Log(message);
    }
}
