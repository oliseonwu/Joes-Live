using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource[] audioSourceList;
    void Start()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        audioSourceList = GetComponents<AudioSource>();
    }
    
    public void PlaySound(AudioClip clip, int audioSourceIndex)
    {
        AudioSource audioSource;
        
        // invalid audioSource index
        if (audioSourceList.Length < audioSourceIndex)
        {
           return; 
        }

        audioSource = audioSourceList[audioSourceIndex];
        
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        
        audioSource.PlayOneShot(clip);
    }
    
    // public void LoopSound(AudioClip clip, int audioSourceIndex)
    // {
    //     if (audioSourceList.Length < audioSourceIndex)
    //     {
    //         return; 
    //     }
    //     
    //     PlaySound();
    // }
}
