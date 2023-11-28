using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GiftBag : MonoBehaviour
{
    // This will take gift from the giftBatchHandler
    // and put it in its bag. It will provide a clean
    // for joe to use the gift by animating to the gift
    
    private List<String[]> _giftBag = new ();
    private readonly object _sendNotificationLock = new ();
    public GiftBatchHandler giftBatchHandler;
    private bool _alertRecieved;
    public bool displayGiftBag;
    public bool getNextGift;
    public bool _sendNotification = true;
    public static event Action GetNextGiftEvent;

    void Start()
    {
        subscribeToEvents();
    }

    // Update is called once per frame
    void Update()
    {
        if (displayGiftBag)
        {
            displayGiftBag = false;
            DisplayGiftBag();
        }

        if (getNextGift)
        {
            getNextGift = false;
            String tempStr = GetNextGift();

            if (tempStr != null)
            {
                print($"Got a {TikTokGiftApi.GiftIdToName[tempStr]}");

            }
            
        }
    }
    private void SendAlert()
    {
        // Sends an alert that there is a gift in the 
        // gift Bag. We only send a notification when
        // an attempt was made to get gift from the giftBag
     
        if (SendNotification)
        {
            SendNotification = false;
            Debug.Log($"{nameof(GiftBag)} --> Sent Alert!");
            GetNextGiftEvent?.Invoke();
        }
    }
    private void CollectMoreGifts()
    {
        List<String[]> tempGiftBag = giftBatchHandler.TakeGiftsIds();
        
    
        if (tempGiftBag != null)
        {
            _giftBag = tempGiftBag;
            Utilities.Print(nameof(GiftBag),"Gift collected!" );
            SendAlert();
        }
        else
        {
            Utilities.Print(nameof(GiftBag),"No gift found. Waiting..." );
        }
    }
    
    
    
    private Boolean isGiftBagEmpty()
    {
        return _giftBag.Count <= 1;
    }

    public String GetNextGift()
    {
        // return the next giftId
        String returnedGiftId;

        returnedGiftId = removeAGift(1);
        
        if (returnedGiftId == null)
        {
            Print("Gift Bag empty. Attempting to get more gifts.");
            
            // since we came to collect gift but no gift available
            // we tell the gift bag to alert us when a gift is available.
            SendNotification = true;
            
            CollectMoreGifts();
        }
        
        return returnedGiftId;
    }
    
    private String removeAGift(int giftIndex)
    {
        // return the giftId removed or
        // null if failed to remove
        
        string[] gift;
        String giftId;
        int giftAmmount;
        int totalGifts; 
        
        if (giftIndex <= 0 || isGiftBagEmpty())
        {
            return null;
        }
        
        gift = _giftBag[giftIndex];
        giftAmmount = int.Parse(gift[1]);
        giftId = gift[0];
        
        // Update the total number of gift left 
        // since we will be removing one gift
        totalGifts = int.Parse(_giftBag[0][1]);
        _giftBag[0][1] = (totalGifts - 1) + "";
        
        if (giftAmmount == 1)
        {
            _giftBag.RemoveAt(giftIndex); // remove the whole gift itself
            
            return giftId;
        }

        // update the amount of a specific gift
        gift[1] = giftAmmount - 1 + "";

        return giftId;
    }
    
    public void DisplayGiftBag()
    {
        String strBuffer = "Gifts Bag = [";
        int intCount = 0;

        if (_giftBag.Count <= 1)
        {
            Debug.Log("No Gift Collected Yet");
            return;
        }

        strBuffer += $"{TikTokGiftApi.GiftIdToName[_giftBag[1][0]]}: {_giftBag[1][1]}";
        
        
        for(int x = 2; x < _giftBag.Count; x++ ){
            strBuffer += $", {TikTokGiftApi.GiftIdToName[_giftBag[x][0]]}: {_giftBag[x][1]}";
        }
        
        Debug.Log(strBuffer+"]");
    }

    private void Print(String message)
    {
        Debug.Log($"{nameof(GiftBag)} --> {message}");
    }
    private void subscribeToEvents()
    {
        GiftBatchHandler.takeGiftIdsEvent += CollectMoreGifts;
    }

    private void unSubscribeFromEvents()
    {
        GiftBatchHandler.takeGiftIdsEvent-= CollectMoreGifts;
    }
    
    private void OnDestroy()
    {
        unSubscribeFromEvents();
    }
    
    private  bool SendNotification
    {
        get
        {
            lock(_sendNotificationLock)
            {
                return _sendNotification;
            }
        }
        set
        {
            lock(_sendNotificationLock)
            {
                _sendNotification = value;
            }
        }
    }
}