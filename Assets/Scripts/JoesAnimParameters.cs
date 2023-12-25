using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoesAnimParameters : MonoBehaviour
{
    // This class just holds the name of parameters
    // for joes the animation
    public Animator _animator;
    public  List<String> checkedParams = new List<string>();
    public static String G_roseTrigger = "G_Rose_1";
    public static String G_LightningBolt1Trigger = "G_Lightning";
    public static String G_LightningBolt2Trigger = "G_Lightning 2";
    public static String G_HatandMustacheTrigger = "G_Cow boy";

    public static String Idle2 = "handsOnHipLookAround";
    public static String Idle3 = "handsOnHipLookAround(One Leg)";

    public static String IdleStatesBool = "Idle States";


    private void Start()
    {
        _animator.GetComponent<Animator>();
    }
    
    /// <summary>
    /// Sets a specified bool param in the animator  
    /// and unchecks every other bool params
    /// <param name="boolParamName"> the animation
    /// param name of type bool </param>
    /// </summary>
    public void setOneBool(String boolParamName)
    {
        // set all active params(Check params) disabled
        checkedParams.Clear();
        
        _animator.SetBool(boolParamName, true);
      
        // add the newly set bool to the list
        checkedParams.Add(boolParamName);
    }

    /// <summary>
    /// Unchecks every bool params in the
    /// animator.
    /// </summary>
    public void ClearAllIntSetBool()
    {
        _animator.SetInteger(IdleStatesBool, 0);
    }

    public void setIntTrigger(int stateId, string intTriggerName)
    {
        _animator.SetInteger(intTriggerName, stateId);

        StartCoroutine(resetIntTrigger(intTriggerName));
    }

    private IEnumerator resetIntTrigger(string intTriggerName)
    {
        yield return new WaitForSeconds(0.1f); // Wait for a short duration
        _animator.SetInteger(intTriggerName, 0);
    }
    
    
    

}
