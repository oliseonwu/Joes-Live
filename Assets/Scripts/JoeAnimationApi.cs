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
    public SpawnManager spawnManager;

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

    private void G_LightningBolt()
    {
        spawnManager.SpawnAngryCloud();
    }

    private void G_HatandMustache()
    {
        _animator.SetTrigger(JoesAnimParameters.G_HatandMustacheTrigger);
    }

    private void IdleAnimation2()
    {
        setBool(JoesAnimParameters.Idle2);
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
    
    private void setBool(String animationParamName)
    {
      // It sets the checkmark for the animation param  
      // sent in and uncheck every other bool params

      List<String> setParams = JoesAnimParameters.checkedParams;
      
      // set all active params(Check params) disabled
      for (int x = 0; x < setParams.Count; x++)
      {
          _animator.SetBool(setParams[x], false);
      }
      
      _animator.SetBool(animationParamName, true);
    }

    public void PlayGiftAnim(String giftId, float waitTime = 0, int option = 1 )
    {
        // Converts gift names to the actual animations
        // Option is used select an animation when a gift has multiple animations.
        // waitTime is the time in seconds to wait before playing the animation
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
    
    
}
