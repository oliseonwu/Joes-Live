using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoesAnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    private String currentAnimation = "";
    public GiftBag giftBag;
    public JoeAnimationApi joeAnimationApi;
    private bool _inPlayMode;
    private bool _inIdleState = true;
    
    private readonly object _inPlayModeLock = new ();
    private readonly object _inIdleStateLock = new ();

    void Start()
    {
        Invoke(nameof(playIdleAnimation), 5f);

        subscribeToEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void subscribeToEvents()
    {
        GiftBag.GetNextGiftEvent += playNextAnimation2;
    }
    
    private void unSubscribeFromEvents()
    {
        GiftBag.GetNextGiftEvent-= playNextAnimation2;
    }
    
    private void OnDestroy()
    {
        unSubscribeFromEvents();
    }

    public void playNextAnimation(float waitTime)
    {
        string nextGiftId = giftBag.GetARandomGift();
        InPlayMode = false;

        if (nextGiftId != null)
        {
            InPlayMode = true;
            InIdleState = false;
            joeAnimationApi.PlayGiftAnim(nextGiftId, waitTime);
            Debug.Log("we play");
            return;
        }

        if (!InIdleState)
        {
            Invoke(nameof(playIdleAnimation), 5f);
        }
    }

    private void playIdleAnimation()
    {
        // Five seconds later I want to still check if
        // am not in playmode and if there is a chance that
        // this was called while in idle state, I want to 
        // make sure not to trigger another idle state
        if (!InPlayMode && !InIdleState)
        {
            InIdleState = true;
            joeAnimationApi.playIdelAnimation();
            Debug.Log("herr");
        }
        


    }
    private void playNextAnimation2()
    {
        InPlayMode = false;

        playNextAnimation(0f);
        
    }
    
    public bool InPlayMode
    {
        get
        {
            lock(_inPlayModeLock)
            {
                return _inPlayMode;
            }
        }
        set
        {
            lock(_inPlayModeLock)
            {
                _inPlayMode = value;
            }
        }
    }
    
    public bool InIdleState
    {
        get
        {
            lock(_inIdleStateLock)
            {
                return _inIdleState;
            }
        }
        set
        {
            lock(_inIdleStateLock)
            {
                _inIdleState = value;
            }
        }
    }
}
