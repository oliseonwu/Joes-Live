using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool showLogs = false;
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Print(String funcName, String message)
    {
        if (showLogs)
        {
           Debug.Log($"{funcName} --> {message}"); 
        }
    }
}
