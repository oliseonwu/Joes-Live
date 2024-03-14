using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TikTokGoals : MonoBehaviour
{
    // Start is called before the first frame update
    public int _likeGoal;
    private int previousLikeGoal = 0;
    public TikTokInteractionTracker ttInteractionTracker;
    [Range(0, 2)] public  float LIKE_GOAL_INCREASE_FACTOR = 1.2f;
    private int DEFUALT_LIKE_GOAL = 200;
    public int likeGoalIncreaseFactor = 200;
    
    void Start()
    {
        Invoke(nameof(UpdateLikeGoal), 4f);
        SubscribeToEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetLikeGoal()
    {
        return _likeGoal;
    }
    public int GetPreviousLikeGoal()
    {
        return previousLikeGoal;
    }
    private void UpdateLikeGoal()
    {
       // based on the number of likes, increase the like goal to a bigger goal 
       int numOfLikesOnTTLive = ttInteractionTracker.getNumberOfLikesOnTTLive();

       if (numOfLikesOnTTLive == 0)
       {
           _likeGoal = DEFUALT_LIKE_GOAL;
           return;
       }

       _likeGoal = numOfLikesOnTTLive + likeGoalIncreaseFactor;
       // _likeGoal = Mathf.FloorToInt(numOfLikesOnTTLive 
       //                              * LIKE_GOAL_INCREASE_FACTOR);
    }

    private void TTInteractionEventHandler(object sender,
        TTInteractionTrackerEventArgs eventArg)
    {
        switch (eventArg.type)
        {
           case TTInteractionTrackerEventArgs.
               InteractionTypes.ReachedLikeGoal:
               
               previousLikeGoal = _likeGoal;
               UpdateLikeGoal();
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
