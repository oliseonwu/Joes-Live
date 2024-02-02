using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This is an extention of our TikTok listener. It tracks
// Interactions that our the ticktok api can't track.
// Once a Interaction happens, it sends an event
public class TikTokInteractionTracker : MonoBehaviour
{
    public static event EventHandler<TTInteractionTrackerEventArgs> OnInteraction;
    private static float LIKE_THRESHOLD = 2;
    private static float LIKE_CHECK_INTERVAL_IN_SEC = 30;
    public int _numOfLikesOnTTLive;
    

    private void Start()
    {
        StartCoroutine(TrackLikesOverTime());
    }

    public void updateNumOfLikes(int numOfLikes)
    {
        // At this point, I dont care about concurency issue.
        _numOfLikesOnTTLive = numOfLikes;
    }
    
    private IEnumerator TrackLikesOverTime()
    {
        while (true)
        {
            int likesAtStartTime = _numOfLikesOnTTLive;

            yield return new WaitForSeconds(LIKE_CHECK_INTERVAL_IN_SEC);
            if ((_numOfLikesOnTTLive - likesAtStartTime) < LIKE_THRESHOLD)
            {
                OnInteraction?.Invoke(this, new TTInteractionTrackerEventArgs(
                    TTInteractionTrackerEventArgs.InteractionTypes.LowLikes));
            }
        }
    }
    
    
}
