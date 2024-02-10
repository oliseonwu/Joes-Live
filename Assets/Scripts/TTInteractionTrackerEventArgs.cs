using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an event for the TikTokInteractionTracker
// It allows us to pass information about the interaction
public class TTInteractionTrackerEventArgs : EventArgs
{
    public enum  InteractionTypes{LowLikes, ReachedLikeGoal, 
        LikeGoalAnnouncement };
    public InteractionTypes type;
    

    public TTInteractionTrackerEventArgs(InteractionTypes type)
    {
        this.type = type;
    }
}
