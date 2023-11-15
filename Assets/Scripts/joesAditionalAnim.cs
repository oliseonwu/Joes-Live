using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joesAditionalAnim : MonoBehaviour
{
    private Animator _animator;
    private string A_RoseInMouth = "A_rose(mouth)";
    public GameObject spawnPoint1; 

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
