using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
// The script will be in charge of keep track of when a situation occurs.
// It send an event when a situation occurs 
public class JoeContextManager : MonoBehaviour
{
    private HashSet<AnimationDatabase.AnimationKey> _level1Contexts = new();
    private HashSet<AnimationDatabase.AnimationKey> _level2Contexts = new();
    private HashSet<AnimationDatabase.AnimationKey> _level3Contexts = new();
    private bool _isBusy;
    private bool _sentNotification;
    private readonly object _isBusyLock = new();
    private readonly object _sendNotificationLock = new();
    public static event Action GetNewContextEvent;
    void Start()
    {
        subscribeToEvents();
    }

    private void SendAlert()
    {
        // Sends an alert that there is a new context animation.
        // We only send a notification when a failed attempt
        // was made when getting some animations.

        if (SendNotification)
        {
            SendNotification = false;
            Debug.Log("JoeContextManager -> New contexts animations available");
            GetNewContextEvent?.Invoke();
        }
    }

    

    /// <summary>
    /// Stores a reference to an animation in the corresponding level (1-3) 
    /// of the Contexts hashmap. This method is used to specify which animation 
    /// should be played in response to a particular situation.
    /// </summary>
    /// <remarks>
    /// The reference is stored as an instance of the AnimationDatabase class, 
    /// categorizing the animation based on its importance level.
    /// </remarks>
    private void RegisterAnimationForContext( AnimationDatabase.AnimationKey animationKey, int importanceLevel )
    {
        IsBusy = true;
        
        HashSet<AnimationDatabase.AnimationKey> chosenContextLevel;
        
        switch (importanceLevel)
        {
            case 1:
                chosenContextLevel = _level1Contexts;
                Debug.Log("Added contex animation to level 1");
                break;
            case 2:
                chosenContextLevel = _level2Contexts;
                break;
            case 3:
                chosenContextLevel = _level3Contexts;
                break;
            default:
                chosenContextLevel = _level1Contexts;
                break;
        }

        chosenContextLevel.Add(animationKey);
        IsBusy = false;
    }

    private void LowLikesDetected()
    {
        // if people are not tapping on their screen after a while
        RegisterAnimationForContext(AnimationDatabase.AnimationKey.TapTapTap,
            1);
    }
    
    private bool IsBusy
    {
        get
        {
            lock (_isBusyLock)
            {
                return _isBusy;
                
            }
        }
        set
        {
            lock (_isBusyLock)
            {
                _isBusy = value;
            }
        }
    }
    
    public bool SendNotification
    {
        get
        {
            lock (_sendNotificationLock)
            {
                return _sentNotification;
            }
        }

        set
        {
            lock (_sendNotificationLock)
            {
                _sentNotification = value;
            }
        }
    }

    private void TTInteractionEventHandler(object sender, 
        TTInteractionTrackerEventArgs eventArgs)
    {
        // maps a tracked tiktok live interaction to
        // Joe's responds animation

        switch (eventArgs.type)
        {
            case TTInteractionTrackerEventArgs.
                InteractionTypes.LowLikes:
                
                RegisterAnimationForContext(
                    AnimationDatabase.AnimationKey.TapTapTap, 
                    1);
                break;
        }
        
    }

    private void subscribeToEvents()
    {
        TikTokInteractionTracker.OnInteraction += TTInteractionEventHandler;
    }
}
