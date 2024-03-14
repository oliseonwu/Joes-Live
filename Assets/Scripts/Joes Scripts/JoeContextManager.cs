using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public bool _sendNotification = true;
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
        // We only send a notification when a an attempt was
        // made using hasContexAnimationOnLevel(1) and it returns 
        // false.

        if (SendNotification)
        {
            SendNotification = false;
            Utilities.Print("JoeContextManager", "New contexts animations available" );
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
        
        Invoke(nameof(SendAlert), 0.5f);
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
                return _sendNotification;
            }
        }

        set
        {
            lock (_sendNotificationLock)
            {
                _sendNotification = value;
            }
        }
    }

    public Boolean hasContexAnimationOnLevel(int level)
    {
        bool hasContexAnimation = getContexStorageByLevel(level).Count > 0;

        if (!hasContexAnimation)
        {
            if (level == 1)
            {
                // Once we check the last level and still no animation
                // we ask the this class to alert Joe when there is more animations
                SendNotification = true;
            }
        }
        
        return hasContexAnimation;
    }

    public AnimationDatabase.AnimationKey getNextContexAtLevel(int level)
    {
        AnimationDatabase.AnimationKey animationKey;
        HashSet<AnimationDatabase.AnimationKey> contexAnimationContainer = 
            getContexStorageByLevel(level);

        IsBusy = true;
        
        if (contexAnimationContainer.Count <= 0)
        {
            throw new Exception("No Context animation at this level.");
        }

        animationKey = getContexStorageByLevel(level).FirstOrDefault(); // returns any animationKey stored;
        contexAnimationContainer.Remove(animationKey);

        IsBusy = false;
        
        return animationKey;
    }

    private  HashSet<AnimationDatabase.AnimationKey> getContexStorageByLevel(int level)
    {
        switch (level)
        {
            case 1:
                return _level1Contexts;
            case 2:
                return _level2Contexts;
            case 3:
                return _level3Contexts;
            default:
                throw new KeyNotFoundException("Invalid Contex level");
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
            
            case TTInteractionTrackerEventArgs.
                InteractionTypes.ReachedLikeGoal:
                
                // In the future, we will have multiple
                // celebration animation
                RegisterAnimationForContext(
                    AnimationDatabase.AnimationKey.Celebrate1,
                    2);
                break;
            
            case TTInteractionTrackerEventArgs.
                InteractionTypes.LikeGoalAnnouncement:
                
                Utilities.Print(nameof(JoeContextManager), "Live goal announce!");
                
                RegisterAnimationForContext(
                    AnimationDatabase.AnimationKey.LikeGoalAnnouncement, 
                    1);
                break;
                
        }
    }

    private void subscribeToEvents()
    {
        TikTokInteractionTracker.OnInteraction += TTInteractionEventHandler;
    }
}
