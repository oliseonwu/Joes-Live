using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class JoeAnimationApi : MonoBehaviour
{
    public Animator _animator;
    private string A_RoseInMouth = "A_rose(mouth)";
    public SpawnManager spawnManager;
    public JoesAnimParameters joesAnimParameters;
    [SerializeField] private ChatBubble _chatBubble;
    public int stateIdOveride = 7;

    void Start()
    {
        // _animator = GetComponent<Animator>();
        subscribeToEvents();
    }

    public void PlayGiftAnim(String giftId, float waitTime = 0, int option = 1 )
    {
        // Converts gift names to the actual animations
        // Option is used select an animation when a gift has multiple animations.
        // waitTime is the time in seconds to wait before playing the animation
        joesAnimParameters.ClearAllSetBool();
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
        }
    }

    public void playIdelAnimation(int scriptChosenStateId = -1)
    {
        int stateId = scriptChosenStateId;
        
        if (stateId == -1) // means script didn't chose an idle state
        {
            stateId = RandomNumberGenerator.GetInt32(0, 9); // set to a random idle state
        }
        
        stateId = (stateIdOveride == 0)? stateId: stateIdOveride; // overide the state when applicable

        stateIdOveride = 0;
        
        joesAnimParameters.ClearAllSetBool();

        switch (stateId)
        {
            case 0: // Idle Bounce
                OriginalIdleState();
                break;
            case 1: // Hands on heaps look left and right
                joesAnimParameters.setIntParam(1, JoesAnimParameters.AnimState1);
                break;
            case 2: // Hands on heaps look left and right(one leg)
                joesAnimParameters.setIntParam(2, JoesAnimParameters.AnimState1);
                break;
            case 3: // Hi
                Hi();
                _chatBubble.SetChatText("Hi!!", 4);
                break;
            case 4: // bend left
                joesAnimParameters.setIntParam(4, JoesAnimParameters.AnimState1, 5);
                break;
            case 5:
                Hi();
                _chatBubble.SetChatText("Welcome to my live!", 6f);
                break;
            case 6:
                Hi();
                _chatBubble.SetChatText("Surprise me with <sprite name=Rose>," +
                                        "<sprite name=Lightning>,<sprite name=Cowboy> gifts and Watch me react"
                    , 6f);
                break;
            case 7: // Tap Tap Tap 1
                joesAnimParameters.setIntParam(7, JoesAnimParameters.AnimState1, 0.5f);
                break;
            case 8: // Head scratch
                joesAnimParameters.setIntParam(8, JoesAnimParameters.AnimState1, 0.5f);
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

    private void Hi()
    {
        joesAnimParameters.setIntParam(3, JoesAnimParameters.AnimState1, 0.5f);
    }

    public void OriginalIdleState()
    {
        joesAnimParameters.setIntParam(0, JoesAnimParameters.AnimState1);
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
    
    public void eyeLayerBlendAmmount(float ammount)
        {
            _animator.SetLayerWeight(3, ammount);
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
