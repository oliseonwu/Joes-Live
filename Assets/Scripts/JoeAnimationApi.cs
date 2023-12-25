using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class JoeAnimationApi : MonoBehaviour
{
    public Animator _animator;
    private string A_RoseInMouth = "A_rose(mouth)";
    public SpawnManager spawnManager;
    public JoesAnimParameters joesAnimParameters;

    void Start()
    {
        subscribeToEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    public void PlayGiftAnim(String giftId, float waitTime = 0, int option = 1 )
    {
        // Converts gift names to the actual animations
        // Option is used select an animation when a gift has multiple animations.
        // waitTime is the time in seconds to wait before playing the animation
        joesAnimParameters.ClearAllIntSetBool();
        switch (giftId)
        {
            case "5655":
                Invoke(nameof(G_roseAnim_1), waitTime);
                break;
            case "6652":
                Invoke(nameof(G_LightningBolt), waitTime);
                break;
            case "6427":
                Invoke(nameof(G_HatandMustache), waitTime);
                break;
            default:
                break;
        }
    }

    public void playIdelAnimation()
    {
        int randomNum = Random.Range(1, 6);
        joesAnimParameters.ClearAllIntSetBool();

        // _animator.SetInteger(JoesAnimParameters.IdleStatesBool, randomNum);
        switch (randomNum)
        {
            case 2:
                _animator.SetInteger(JoesAnimParameters.IdleStatesBool, 2);
                break;
            case 3:
                _animator.SetInteger(JoesAnimParameters.IdleStatesBool, 3);
                break;
            case 4:
                joesAnimParameters.setIntTrigger( 4, JoesAnimParameters.IdleStatesBool);
                break;
            case 5:
                joesAnimParameters.setIntTrigger( 5, JoesAnimParameters.IdleStatesBool);
                break;
            default:
                break;
        }
    }

    

    private void G_roseAnim_1()
    {
         _animator.SetTrigger(JoesAnimParameters.G_roseTrigger);
    }

    public void G_LightningBolt()
    {
        spawnManager.SpawnAngryCloud();
    }

    public void G_HatandMustache()
    {
        _animator.SetTrigger(JoesAnimParameters.G_HatandMustacheTrigger);
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
        int randomNumber =  Random.Range(0, 2);

        switch (randomNumber)
        {
            case 1:
                _animator.SetTrigger(JoesAnimParameters.G_LightningBolt2Trigger);
                break;
            default:
                _animator.SetTrigger(JoesAnimParameters.G_LightningBolt1Trigger);
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
