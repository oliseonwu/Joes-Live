using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[RequireComponent(typeof(VisualEffect))]
public class FireworksAudio : VFXOutputEventAbstractHandler
{

    public override bool canExecuteInEditor { get; }
    public AudioSource audioSource;

    public override void OnVFXOutputEvent(VFXEventAttribute eventAttribute)
    {
        SoundManager.Instance.PlaySound(audioSource.clip, 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
