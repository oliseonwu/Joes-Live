using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    // lets say the next animation is roseInMouth
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
