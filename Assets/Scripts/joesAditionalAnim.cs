using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joesAditionalAnim : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _animator;
    
    //Params
    private string A_RoseInMouth = "A_rose(mouth)";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _animator = GetComponent<Animator>();
    }

    public void wearRose()
    {
        _animator.SetBool(A_RoseInMouth, true);
    }
    
    public void removeRose()
    {
        _animator.SetBool(A_RoseInMouth, false);
    }
   
}
