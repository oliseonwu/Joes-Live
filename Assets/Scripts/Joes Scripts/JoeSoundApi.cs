using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoeSoundApi : MonoBehaviour
{
    private AudioSource[] _audioSourceList;
    public AudioClip[] _sounds;
    void Start()
    {
        _audioSourceList = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void RizzSound()
    {
        _audioSourceList[0].PlayOneShot(_sounds[15]);
    }
    public void PopSound()
    {
        _audioSourceList[0].PlayOneShot(_sounds[16]);
    }
    public void Ayy1Sound()
    {
        _audioSourceList[0].PlayOneShot(_sounds[0]);
    }
    public void WhooshSoundLowPitch()
    {
        _audioSourceList[0].PlayOneShot(_sounds[1]);
    }
    public void WhooshSoundHighPitch()
    {
        _audioSourceList[0].PlayOneShot(_sounds[3]);
    }
    public void WhooshSoundMidPitch()
    {
        _audioSourceList[0].PlayOneShot(_sounds[8]);
    }
    public void GroundImpact()
    {
        _audioSourceList[0].PlayOneShot(_sounds[2]);
    }
    public void FallSound()
    {
        _audioSourceList[1].PlayOneShot(_sounds[4]);
    }
    public void LonguuffSound()
    {
        _audioSourceList[0].PlayOneShot(_sounds[5]);
    }
    public void UffSound()
    {
        _audioSourceList[0].PlayOneShot(_sounds[6]);
    }
    public void PickUpAttachment()
    {
        _audioSourceList[0].PlayOneShot(_sounds[7]);
    }

    public void WhipSound()
    {
        _audioSourceList[0].PlayOneShot(_sounds[9]);
    }

    public void KnockSound(int knockType = 1) // only 4 knock types [1-4]
    {
        _audioSourceList[0].PlayOneShot(_sounds[9+ knockType]);
    }

    public void SoundsSource1(int audioClipIndex)
    {
        _audioSourceList[0].PlayOneShot(_sounds[audioClipIndex]);
    }
    
    public void SoundsSource2(int audioClipIndex)
    {
        _audioSourceList[1].PlayOneShot(_sounds[audioClipIndex]);
    }
}
