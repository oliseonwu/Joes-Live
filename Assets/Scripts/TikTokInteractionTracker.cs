using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


// This is an extention of our TikTok listener. It tracks
// Interactions that our the ticktok api can't track.
// Once a Interaction happens, it sends an event
public class TikTokInteractionTracker : MonoBehaviour
{
    public static event EventHandler<TTInteractionTrackerEventArgs> OnInteraction;
    
    // If this LIKE_THRESHOLD is met, Joe's will
    // say thanks for the likes with more excitement.
    // But if we get some likes but if the LIKE_THRESHOLD
    // is not met, joe will just say thanks
    // for the likes but not with too much excitement. 
    private static float LIKE_THRESHOLD = 2;
    private static float LIKE_CHECK_INTERVAL_IN_SEC = 30;
    private float likeGoalAnnouncementIntervalInSec = 120; 
    public int _numOfLikesOnTTLive;
    public TikTokGoals tikTokGoals;
    

    private void Start()
    {
        StartCoroutine(TrackLikesOverTime());
        StartCoroutine(TrackLikeGoalAnnouncement());
    }

    public void updateNumOfLikes(int numOfLikes)
    {
        // At this point, I dont care about concurency issue.
        _numOfLikesOnTTLive = numOfLikes;
    }

    public int getNumberOfLikesOnTTLive()
    {
        return _numOfLikesOnTTLive;
    }
    
    private IEnumerator TrackLikesOverTime()
    {
        while (true)
        {
            int likesAtStartTime = _numOfLikesOnTTLive;

            yield return new WaitForSeconds(LIKE_CHECK_INTERVAL_IN_SEC);

            if (_numOfLikesOnTTLive >= tikTokGoals.GetLikeGoal()) // if we hit our like goal
            {
                OnInteraction?.Invoke(this, new TTInteractionTrackerEventArgs(
                    TTInteractionTrackerEventArgs.InteractionTypes.ReachedLikeGoal)); 
            }
            else if ((_numOfLikesOnTTLive - likesAtStartTime) < LIKE_THRESHOLD)
            {
                OnInteraction?.Invoke(this, new TTInteractionTrackerEventArgs(
                    TTInteractionTrackerEventArgs.InteractionTypes.LowLikes));
            }
            // else(when we dont hit our goal or like threshold),
            // joe should say "thanks for the likes guys"
        }
    }

    
    private IEnumerator TrackLikeGoalAnnouncement()
    {
        // Tracks when to sends an event after a wait time
        // which makes joe state is like goal.

        while (true)
        {
           likeGoalAnnouncementIntervalInSec =  Random.Range(0, 100);
           
           yield return new WaitForSeconds(likeGoalAnnouncementIntervalInSec);
           
           OnInteraction?.Invoke(this, new TTInteractionTrackerEventArgs(
               TTInteractionTrackerEventArgs.InteractionTypes.LikeGoalAnnouncement));
        }
        
        
    }
}
