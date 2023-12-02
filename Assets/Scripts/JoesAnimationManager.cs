using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoesAnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    private String currentAnimation = "";
    public GiftBag giftBag;
    public JoeAnimationApi joeAnimationApi;
    public JoesAnimParameters joesAnimParameters;
    private bool _inPlayMode;
    
    void Start()
    {
        joeAnimationApi.playIdelAnimation();
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
        
        if (nextGiftId != null)
        {
            joeAnimationApi.PlayGiftAnim(nextGiftId, waitTime);
            return;
        }
        
        joeAnimationApi.playIdelAnimation();
    }
    
    private void playNextAnimation2()
    {
        playNextAnimation(0f);
        
    }
}
