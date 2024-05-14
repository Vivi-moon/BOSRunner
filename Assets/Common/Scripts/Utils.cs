using System;
using UnityEngine;

public static class Logger
{
    public static void Log(string sender, string msg)
    {
        Debug.LogFormat("[{0}] {1}", sender, msg);
    }

    public static void LogError(string sender, string msg)
    {
        Debug.LogErrorFormat("[{0}]{1}", sender, msg);
    }

    public static void LogException(string sender, string msg, Exception ex)
    {
        Debug.LogErrorFormat("[{0}] {1} (see exception below)", sender, msg);
        Debug.LogException(ex);
    }
}