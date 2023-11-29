using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoesAnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    private String currentAnimation = "";
    public GiftBag giftBag;
    public JoeAnimationApi JoeAnimationApi;
    private bool _inPlayMode;
    
    void Start()
    {
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
        string nextGiftId = giftBag.GetNextGift();
        Debug.Log("next giftId = "+ nextGiftId);

        if (nextGiftId != null)
        {
            JoeAnimationApi.PlayGiftAnim(nextGiftId, waitTime);
        }
    }
    
    private void playNextAnimation2()
    {
        playNextAnimation(.5f);
        
    }
}
