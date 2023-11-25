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
    private List<String[]> collectedGifts = new List<string[]>();
    public Boolean displayCollectedGift;
    
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
    
    
    
    

    private void CollectGifts()
    {
        if (collectedGifts.Count > 0)
        {
            // Make a note that we got an
            // alert from the GiftBatchHandler
            alertRecieved = true;
            return;
        }

        alertRecieved = false;
        
        collectedGifts = giftBatchHandler.TakeGifts();
        
        Debug.Log($"Gift collected!");
    }
    
    private void subscribeToEvents()
    {
        GiftBatchHandler.takeGiftEvent += CollectGifts;
    }

    private void unSubscribeFromEvents()
    {
        GiftBatchHandler.takeGiftEvent-= CollectGifts;
    }

    public void DisplayCollectedGifts()
    {
        String strBuffer = "Collected Gifts = [";
        int intCount = 0;

        if (collectedGifts.Count <= 1)
        {
            Debug.Log("No Gift Collected Yet");
            return;
        }

        strBuffer += $"{collectedGifts[1][0]}: {collectedGifts[1][1]}";
        
        
        for(int x = 2; x < collectedGifts.Count; x++ ){
            strBuffer += $", {collectedGifts[x][0]}: {collectedGifts[x][1]}";
        }
        
        Debug.Log(strBuffer+"]");
        
        
    }

    private void OnDestroy()
    {
        unSubscribeFromEvents();
    }
}
