using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class joesAditionalAnim : MonoBehaviour
{
    private Animator _animator;
    private string A_RoseInMouth = "A_rose(mouth)";
    public GameObject spawnPoint1;
    private int randomNumber;
    private AudioSource _audioSource;
    public AudioClip[] _sounds;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        subscribeToEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void sad()
    {
        _animator.SetBool("Sad", true); 
    }
    public void happy()
    {
        _animator.SetBool("Sad", false); 
    }

    public void lipLayerBlendAmmount(float ammount)
    {
        _animator.SetLayerWeight(2, ammount);
    }
    public void wearRose()
    {
        _animator.SetBool(A_RoseInMouth, true);
    }

    public void ayySound1()
    {
        _audioSource.PlayOneShot(_sounds[0]);
    }

    
    
    public void removeRose()
    {
        _animator.SetBool(A_RoseInMouth, false);
    }

    private void shockAnimation()
    {
        randomNumber =  Random.Range(0, 2);

        switch (randomNumber)
        {
            case 1:
                _animator.SetTrigger("shock2");
                break;
            default:
                _animator.SetTrigger("shock");
                break;
        }
        
    }

    private void subscribeToEvents()
    {
        angryCloud.shockAnimEvent += shockAnimation;
    }

    private void unSubscribeFromEvents()
    {
        angryCloud.shockAnimEvent -= shockAnimation;
    }

    private void OnDestroy()
    {
        unSubscribeFromEvents();
    }
}
