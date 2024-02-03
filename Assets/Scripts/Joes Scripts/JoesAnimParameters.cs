using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoesAnimParameters : MonoBehaviour
{
    // This class just holds the name of parameters
    // for joes the animation
    public Animator _animator;
    public  List<String> checkedParams = new();
    public static String G_roseTrigger = "G_Rose_1";
    public static String G_LightningBolt1Trigger = "G_Lightning";
    public static String G_LightningBolt2Trigger = "G_Lightning 2";
    public static String G_HatandMustacheTrigger = "G_Cow boy";
    


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
    public void ClearAllSetBool()
    {
        if (checkedParams.Count == 0)
        {
            return; // stop early
        }
        
        // set all active params(Check params) disabled
        for (int x = 0; x < checkedParams.Count; x++)
        {
            _animator.SetBool(checkedParams[x], false);
        }
        checkedParams.Clear();
    }
    
    

    /// <summary>
    /// Used to simulate the types of param the animator uses
    /// <param name="stateId"> the animation Id state to
    /// set Joe's animation state to
    /// (Check AnimationDatabase class) </param>
    /// <param name="intParamName"> the name of the int param</param>
    /// <param name="isTypeBool"> marks if this stateId is of
    /// type bool</param>
    /// </summary>
    public void setIntParam(int stateId,  bool isTypeBool, string intParamName = "Animation State")
    {
        // If type bool, leave int state when checked
        // else, reset int state after 0.5 sec
        
        _animator.SetInteger(intParamName, stateId);

        // used to simulate a trigger param or a bool with a timer
        if (!isTypeBool)
        {
            StartCoroutine(resetIntParam(intParamName, 0.5f));
        }
    }

    private IEnumerator resetIntParam(string intParamName, float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // Wait for a short duration
        _animator.SetInteger(intParamName, 0);
    }
}
