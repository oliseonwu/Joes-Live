using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllJoeStates : MonoBehaviour
{
    public enum JoeStates { Idle, Tapping }
    
    private static Dictionary<JoeStates, int> stateToId = new Dictionary<JoeStates, int>
    {
        { JoeStates.Idle, 0 },
        { JoeStates.Tapping, 7 },
    };
    
    public static int GetStateId(JoeStates joeStatesEnum)
    {
        return stateToId[joeStatesEnum];
    }
}
