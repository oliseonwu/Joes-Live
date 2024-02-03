using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDatabase: MonoBehaviour
{
    public enum AnimationKey { Idle, TapTapTap }
    
    private static Dictionary<AnimationKey, int> animationDic = new Dictionary<AnimationKey, int>
    {
        { AnimationKey.Idle, 0 },
        { AnimationKey.TapTapTap, 2000},
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
