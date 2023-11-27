using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoesAnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    private String currentAnimation = "";
    private Boolean alertRecieved = false;
    public GiftBatchHandler giftBatchHandler;
    private List<String[]> collectedGiftsIds = new List<string[]>();
    public Boolean displayCollectedGift;
    public JoeAnimationApi JoeAnimationApi;
    private Boolean _inPlayMode;
    private readonly object _inPlayModeLock = new object();
    
    void Start()
    {
        subscribeToEvents();
    }

    // Update is called once per frame
    void Update()
    {
        if (displayCollectedGift)
        {
            displayCollectedGift = false;
            DisplayCollectedGifts();
        }
    }
    
    
    
    

    private void CollectGiftsIds()
    {
        if (!isGiftFinshed())
        {
            // Make a note that we got an
            // alert from the GiftBatchHandler
            alertRecieved = true;
            return;
        }

        alertRecieved = false; // make variable thread safe!!!!!!
        
        collectedGiftsIds = giftBatchHandler.TakeGiftsIds();
        
        Debug.Log($"Gift collected!");

        
        if (!InPlayMode)
        {
            playNextAnimation(0.5f);
        }
    }

    private Boolean isGiftFinshed()
    {
        return collectedGiftsIds.Count <= 1;
    }
    
    private void subscribeToEvents()
    {
        GiftBatchHandler.takeGiftIdsEvent += CollectGiftsIds;
    }

    private void unSubscribeFromEvents()
    {
        GiftBatchHandler.takeGiftIdsEvent-= CollectGiftsIds;
    }

    public void DisplayCollectedGifts()
    {
        String strBuffer = "Collected Gifts = [";
        int intCount = 0;

        if (collectedGiftsIds.Count <= 1)
        {
            Debug.Log("No Gift Collected Yet");
            return;
        }

        strBuffer += $"{TikTokGiftApi.GiftIdToName[collectedGiftsIds[1][0]]}: {collectedGiftsIds[1][1]}";
        
        
        for(int x = 2; x < collectedGiftsIds.Count; x++ ){
            strBuffer += $", {TikTokGiftApi.GiftIdToName[collectedGiftsIds[x][0]]}: {collectedGiftsIds[x][1]}";
        }
        
        Debug.Log(strBuffer+"]");
        
        
    }

    private void OnDestroy()
    {
        unSubscribeFromEvents();
    }

    public void playNextAnimation(float waitTime)
    {
        string nextGiftId = useAGift();
        
        if (nextGiftId != null)
        {
            InPlayMode = true;
            JoeAnimationApi.PlayGiftAnim(nextGiftId, waitTime); 
            return;
        }

        InPlayMode = false;
    }

    private String useAGift()
    {
        String returnedGiftId;
        
        
        if (collectedGiftsIds.Count <= 1)
        {
            return null;
        }

        return removeAGift(1);
        

    }

    private String removeAGift(int giftIndex)
    {
        // return the giftId removed or
        // null if failed to remove
        
        string[] gift = collectedGiftsIds[giftIndex];
        String giftId;
        int giftAmmount = int.Parse(gift[1]);
        
        if (giftIndex <= 0)
        {
            return null;
        }
        
        giftId = gift[0];
        
        if (giftAmmount == 1)
        {
            collectedGiftsIds.RemoveAt(giftIndex); // remove the whole gift itself
            
            return giftId;
        }

        // update the amount of a specific gift
        gift[1] = giftAmmount - 1 + "";

        return gift[0];
    }
    
    public bool InPlayMode
    {
        get
        {
            lock (_inPlayModeLock)
            {
                return _inPlayMode;
            }
        }
        set
        {
            lock (_inPlayModeLock)
            {
                _inPlayMode = value;
            }
        }
    }
    
    

    
    
}
