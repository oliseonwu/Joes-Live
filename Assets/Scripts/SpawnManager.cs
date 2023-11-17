using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    // lets say the next animation is roseInMouth
    public GameObject spawnPoint1;
    public GameObject angryCloud;
    public Boolean spawnAngryCloud;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnAngryCloud)
        {
            SpawnAngryCloud();
        }
    }

    void SpawnAngryCloud()
    {
        spawnAngryCloud = false;
        Instantiate(angryCloud, spawnPoint1.transform.position, Quaternion.identity);
    }
    
    
}
