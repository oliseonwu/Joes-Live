using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class JoesAnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool pause;
    public bool _inPlayMode;
    public GiftBag giftBag;
    public JoeAnimationApi joeAnimationApi;
    public JoeContextManager ContextManager;
    public float contexAnimDelay = 2;
    
    private bool _inIdleState = false;
    

    // Since joe has multiple default animations
    // this keeps track of the amount of time 
    // in sec that joe has to keep a type of
    // idle pose before switching to another idle pose.
    private float _idlePoseChangeFrequency;
    private float minimumTimeForIdelPoseChange = 5;
    private float maximumTimeForIdelPoseChange = 12;
    
    private readonly object _inPlayModeLock = new ();
    private readonly object _inIdleStateLock = new ();

    void Start()
    {
       Invoke(nameof(playIdleAnimation), 5f);
        subscribeToEvents();
    }
    

    private void subscribeToEvents()
    {
        GiftBag.GetNextGiftEvent += playNextAnimation2;
        JoeContextManager.GetNewContextEvent += playNextAnimation2;
    }
    
    private void unSubscribeFromEvents()
    {
        GiftBag.GetNextGiftEvent-= playNextAnimation2;
        JoeContextManager.GetNewContextEvent -= playNextAnimation2;
    }

    private void OnDestroy()
    {
        unSubscribeFromEvents();
    }

    private void SetToPlayMode()
    {
        CancelInvoke(nameof(playIdleAnimation));
        CancelInvoke(nameof(ChangeIdlePose));
        InPlayMode = true;
        InIdleState = false;
    }
    
    public void playNextAnimation(float delay)
    {
        int choiceBetweenGiftAndContext;
        if (pause)
        {
            return;
        }

        if (ContextManager.hasContexAnimationOnLevel(3))
        {
            SetToPlayMode();
            joeAnimationApi.PAnimByAnimKeyWrapper(
                ContextManager.getNextContexAtLevel(3), contexAnimDelay);
            return;
        }

        if (!giftBag.isGiftBagEmpty(this) &&
            ContextManager.hasContexAnimationOnLevel(2))
        {
            Debug.Log("We in the middle");
            playGiftOrContextLevel2(delay);
            return;
        }

        
        
        if (!giftBag.isGiftBagEmpty(this))
        {
            SetToPlayMode();
            joeAnimationApi.PlayGiftAnim(giftBag.GetARandomGift(), delay);
            return;
        }
        
        if (ContextManager.hasContexAnimationOnLevel(2))
        {
            SetToPlayMode();
            joeAnimationApi.PAnimByAnimKeyWrapper(
                ContextManager.getNextContexAtLevel(2), contexAnimDelay);  
            return;
        }
        
        if (ContextManager.hasContexAnimationOnLevel(1))
        {
            SetToPlayMode();
            joeAnimationApi.PAnimByAnimKeyWrapper(
                ContextManager.getNextContexAtLevel(1), contexAnimDelay);
            return;
        }

        InPlayMode = false;
        
        if (!InIdleState)
        {
            Invoke(nameof(playIdleAnimation), 5f);
        }
        
    }

    private void playGiftOrContextLevel2(float animationDelay)
    {
        int choiceBetweenGiftAndContext = RandomNumberGenerator.GetInt32(0, 2);
        // 1 -- Gift
        // 0 -- context Level 2

        SetToPlayMode();
        
        switch (choiceBetweenGiftAndContext)
        {
            case 0:
                joeAnimationApi.PAnimByAnimKeyWrapper(
                    ContextManager.getNextContexAtLevel(2), contexAnimDelay);  
                break;
            case 1: 
                joeAnimationApi.PlayGiftAnim(giftBag.GetARandomGift(), animationDelay);
                break;
            default: // play gift by default
                joeAnimationApi.PlayGiftAnim(giftBag.GetARandomGift(), animationDelay);
                break;
        }
    }
    
    private void playNextAnimation2()
    {
        if (!InPlayMode)
        {
            playNextAnimation(0f);
        }
    }
    
    private void playIdleAnimation()
    {
        if (pause)
        {
            return;
        }
        
        _idlePoseChangeFrequency = Random.Range(
            minimumTimeForIdelPoseChange, 
            maximumTimeForIdelPoseChange);
        
        // Five seconds later I want to still check if
        // am not in playmode and if there is a chance that
        // this function was called while again while in
        // idle state, I want to make sure not to trigger
        // another idle state
        if (!InPlayMode && !InIdleState)
        {
            Utilities.Print(nameof(JoesAnimationManager), "In Idle State.");
            InIdleState = true;
            joeAnimationApi.playIdelAnimation();
            
            Invoke(nameof(ChangeIdlePose), _idlePoseChangeFrequency);
        }

    }

    private void ChangeIdlePose()
    {
        _idlePoseChangeFrequency = Random.Range(
            minimumTimeForIdelPoseChange, 
            maximumTimeForIdelPoseChange);
        
        if (!InPlayMode )
        {
            InIdleState = true;
            joeAnimationApi.playIdelAnimation();
        }
        
        Invoke(nameof(ChangeIdlePose), _idlePoseChangeFrequency);
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
