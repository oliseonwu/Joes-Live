using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class JoeAnimationApi : MonoBehaviour
{
    private Animator _animator;
    private string A_RoseInMouth = "A_rose(mouth)";
    private int randomNumber;

    void Start()
    {
        _animator = GetComponent<Animator>();
        subscribeToEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    private void G_roseAnim_1()
    {
        _animator.SetTrigger(JoesAnimParameters.G_roseTrigger);
    }
    
    public void sad()
    {
        _animator.SetBool("Sad", true); 
    }
    public void happy()
    {
        _animator.SetBool("Sad", false); 
    }
    public void Afraid1()
    {
        _animator.SetTrigger("Afraid (1)"); 
    }

    public void lipLayerBlendAmmount(float ammount)
    {
        _animator.SetLayerWeight(2, ammount);
    }
    public void wearRose()
    {
        _animator.SetBool(A_RoseInMouth, true);
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
                _animator.SetTrigger("G_Shock2");
                break;
            default:
                _animator.SetTrigger("G_Shock");
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

    private void PlayGiftAnim(String giftName, int option = 1)
    {
        // Converts gift names to the actual animations
        switch (giftName)
        {
            case "Rose":
                G_roseAnim_1();
                break;
            default:
                break;
        }
    }
}
