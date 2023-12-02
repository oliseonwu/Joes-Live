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
            
            giftBatchHandler.addToGiftIdContainer("5655", 1);
        }
        
        if (lightning)
        {
            // lightning = false;
            
            giftBatchHandler.addToGiftIdContainer("6652", 1);
        }
        
        if (CowBoy)
        {
            CowBoy = false;
            
            giftBatchHandler.addToGiftIdContainer("6427", 1);
        }

        if (TakeGifts)
        {
            TakeGifts = false;

            giftBatchHandler.TakeGiftsIds();
            
            Debug.Log("Took Gifts!");
        }
        
    }
}
