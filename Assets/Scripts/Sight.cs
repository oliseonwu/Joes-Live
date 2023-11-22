using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Sight : MonoBehaviour
{
    [FormerlySerializedAs("JoeAnimationController")] public JoeAnimationApi joeAnimationApi;
    [FormerlySerializedAs("JoeSoundController")] public JoeSoundApi joeSoundApi;
    private int randomNumber;
    private int randomNumber2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        randomNumber =  Random.Range(0, 2);
        randomNumber2 = Random.Range(0, 2);

        Debug.Log(randomNumber);
        if (other.tag == "AngryCloud")
        {
            // makes Joe afraid
            if (randomNumber == 1)
            {
                joeAnimationApi.Afraid1();
                
                // Long aufffff sound
                joeSoundApi.LonguuffSound();
            }
            else
            {
                if (randomNumber2 == 0)
                {
                    joeSoundApi.UffSound();
                }
            }

            
            
        }
    }
}
