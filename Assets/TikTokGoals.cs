using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TikTokGoals : MonoBehaviour
{
    // Start is called before the first frame update
    private int _likeGoal = 100;
    [Range(0, 2)] public  float LIKE_GOAL_INCREASE_FACTOR = 1.5f;
    
    void Start()
    {
        SubscribeToEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getLikeGoal()
    {
        return _likeGoal;
    }
    private void updateLikeGoal()
    {
       // based on the previous like increase the like goal to a bigger goal 
       _likeGoal = Mathf.FloorToInt(_likeGoal * LIKE_GOAL_INCREASE_FACTOR);
    }

    private void TTInteractionEventHandler(object sender,
        TTInteractionTrackerEventArgs eventArg)
    {
        switch (eventArg.type)
        {
           case TTInteractionTrackerEventArgs.
               InteractionTypes.ReachedLikeGoal: 
               updateLikeGoal();
               break;
        }
        
    }

    private void SubscribeToEvents()
    {
        TikTokInteractionTracker.OnInteraction += TTInteractionEventHandler ;
    }

    private void UnsubscribeEvents()
    {
        TikTokInteractionTracker.OnInteraction -= TTInteractionEventHandler ;
    }
    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
}
