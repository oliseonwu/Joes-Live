using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FireWorks : MonoBehaviour
{
    public VisualEffect fireworksVfx;
    public float fireWorksDurationInSec = 20;

    // Start is called before the first frame update
   // https://gamedev.stackexchange.com/questions/173203/how-can-i-play-and-stop-a-visual-effect-graph-effect-through-script
   private void Awake()
   {
       StopFireWorks();
   }

    public void StartFireWorks()
    {
        fireworksVfx.Play();
        Invoke(nameof(StopFireWorks), fireWorksDurationInSec);
    }
    
    public void StopFireWorks()
    {
        fireworksVfx.Stop();
    }
}
