using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    // lets say the next animation is roseInMouth
    public GameObject spawnPoint1;
    public GameObject angryCloud;

    void Start()
    {
        spawnAngryCloud();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnAngryCloud()
    {
        Instantiate(angryCloud, spawnPoint1.transform.position, Quaternion.identity);
    }
    
    
}
