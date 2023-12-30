using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class JoesAnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GiftBag giftBag;
    public JoeAnimationApi joeAnimationApi;
    private bool _inPlayMode;
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
            CancelInvoke(nameof(playIdleAnimation));
            CancelInvoke(nameof(ChangeIdlePose));
            joeAnimationApi.OriginalIdleState(); // set to Idle state
            
            InPlayMode = true;
            InIdleState = false;
            joeAnimationApi.PlayGiftAnim(nextGiftId, waitTime);
            return;
        }

        if (!InIdleState)
        {
            Invoke(nameof(playIdleAnimation), 5f);
        }
    }

    private void playIdleAnimation()
    {
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
            // Debug.Log("Idle Pose change");
        }
        
        Invoke(nameof(ChangeIdlePose), _idlePoseChangeFrequency);
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
