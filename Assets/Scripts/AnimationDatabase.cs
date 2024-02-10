using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDatabase: MonoBehaviour
{
    public enum AnimationKey { Idle, TapTapTap, LikeGoalAnnouncement, Celebrate1 }
    
    private static Dictionary<AnimationKey, int> animationDic = new Dictionary<AnimationKey, int>
    {
        { AnimationKey.Idle, 0 },
        { AnimationKey.TapTapTap, 2000},
        { AnimationKey.LikeGoalAnnouncement, 2001},
        { AnimationKey.Celebrate1, 2002}
    };
    
    public static int GetStateId(AnimationKey animationKey)
    {
        if (animationDic.TryGetValue(animationKey, out int animationId))
        {
            return animationId;
        }
        else
        {
            throw new KeyNotFoundException($"Animation '{animationKey}' not found.");
        }
    }
}
