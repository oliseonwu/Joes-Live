using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
// The script will be in charge of keep track of when a situation occurs.
// It send an event when a situation occurs 
public class JoeContextManager : MonoBehaviour
{
    public static event Action LowLikesEvent;
    void Start()
    {
        
        // LowLikes(10);
    }

    // if people are not tapping on their screen after a while 
    // IEnumerator  LowLikes(int waitTime)
    // {
    //     int randomWaitTime = RandomNumberGenerator.GetInt32(10, 40);
    //     yield return new WaitForSeconds(waitTime);
    //     LowLikesEvent.Invoke();
    //
    //     LowLikes(randomWaitTime);
    //
    //
    // }
}
