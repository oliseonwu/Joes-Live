using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TikTokAlertTest : MonoBehaviour
{
    public Boolean Rose;

    public Boolean lightning;

    public Boolean CowBoy;

    public Boolean TakeGifts;
    
    public Boolean setIsGiftContainerFunct;

    public GiftBatchHandler giftBatchHandler;

    // Update is called once per frame
    void Update()
    {
        BroadcastTestGifts();
    }

    private void BroadcastTestGifts()
    {
        if (Rose)
        {
            Rose = false;
            
            giftBatchHandler.addToGiftContainer("Rose", 1);
        }
        
        if (lightning)
        {
            lightning = false;
            
            giftBatchHandler.addToGiftContainer("Lightning Bolt", 1);
        }
        
        if (CowBoy)
        {
            CowBoy = false;
            
            giftBatchHandler.addToGiftContainer("Hat and Mustache", 1);
        }

        if (TakeGifts)
        {
            TakeGifts = false;

            giftBatchHandler.TakeGifts();
            
            Debug.Log("Took Gifts!");
        }

        if (setIsGiftContainerFunct != giftBatchHandler.IsGiftContainerClearing)
        {
            giftBatchHandler.IsGiftContainerClearing = setIsGiftContainerFunct;
        }
    }
}
