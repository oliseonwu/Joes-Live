using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class GiftBag : MonoBehaviour
{
    // This will take gift from the giftBatchHandler
    // and put it in its bag. It will provide a clean
    // for joe to use the gift by animating to the gift
    
    private List<String[]> _giftBag = new ();
    private readonly object _sendNotificationLock = new();
    private readonly object _isbusyLock = new();
    public GiftBatchHandler giftBatchHandler;
    private bool _alertRecieved;
    public bool displayGiftBag;
    public bool getNextGift;
    public bool _sendNotification = true;
    private bool _isBusy;
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
            Debug.Log($"{nameof(GiftBag)} --> Gift Available!");
            GetNextGiftEvent?.Invoke();
        }
    }
    
    private void CollectMoreGifts()
    {
        // Used to update the gift bag with gifts from the 
        // Giftbatch handler
        
        List<String[]> tempGiftBag = giftBatchHandler.TakeGiftsIds();
        
    
        if (tempGiftBag != null)
        {
            _giftBag = tempGiftBag;
            SendAlert();
        }
        else
        {
            Utilities.Print(nameof(GiftBag),"No gift found. Waiting..." );
        }
    }

    private void ForceUpdateBag()
    {
        List<String[]> tempGiftBag = giftBatchHandler.TakeGiftsIds();

        if (tempGiftBag == null)
        {
            Utilities.Print(nameof(GiftBag),"No gift found." );
            return;
        }

        IsBusy = true;

        for (int x = 1; x < tempGiftBag.Count; x++)
        {
            IncreaseAGiftAmount(tempGiftBag[x][0], tempGiftBag[x][1] );
        }

        IsBusy = false;
        Utilities.Print(nameof(GiftBag),"Force update Success!" );
        SendAlert();
    }

    
    
    
    private Boolean isGiftBagEmpty()
    {
        return _giftBag.Count <= 1;
    }

    public String GetNextGift()
    {
        // return the next giftId
        String returnedGiftId;
        
        if (IsBusy)
        {
            return null;
        }

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

    public String GetARandomGift()
    {
        // randomly return a gift from the gift bag.

        if (IsBusy)
        {
            // since we came to collect gift but the Giftbag is busy
            // we can tell the gift bag to alert us when the GiftBag class is free.
            SendNotification = true;
            
            return null;
        }
        
        if (isGiftBagEmpty())
        {
            Print("Gift Bag empty. Attempting to get more gifts.");
            
            // since we came to collect gift but no gift available
            // we tell the gift bag to alert us when a gift is available.
            SendNotification = true;
            
            CollectMoreGifts();
            
            return null;
        }
        
        return removeAGift(Random.Range(1, _giftBag.Count));
    }
    
    private void IncreaseAGiftAmount(String giftId, String amount)
    {
        string[] gift = null;
        
        //check for the giftId in the gift bag
        for (int x = 1; x < _giftBag.Count; x++)
        {
            if (_giftBag[x][0] == giftId)
            {
                gift = _giftBag[x];
                x = _giftBag.Count; // break for loop
            }
            
        }

        // if found, increament the Gift amount with amount var
        if (gift != null)
        {
            gift[1] = int.Parse(gift[1]) + int.Parse(amount)+"";
        }
        else
        {
            _giftBag.Add(new []{giftId, amount+""});
        }
        // else we add the gift as a new gift to the bag



    }
    
    private String removeAGift(int giftIndex)
    {
        // returns the giftId removed from the giftbag or
        // null if failed to remove
        
        string[] gift;
        String giftId;
        int giftAmmount;
        int totalGifts; 
        
        // invalid input or empty bag case
        if (giftIndex <= 0 || isGiftBagEmpty())
        {
            return null;
        }
        
        gift = _giftBag[giftIndex]; // the gift --> [giftId, giftAmmount]
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
        GiftBatchHandler.SendActionNotificationEvent += ForceUpdateBag;
    }

    private void unSubscribeFromEvents()
    {
        GiftBatchHandler.takeGiftIdsEvent-= CollectMoreGifts;
        GiftBatchHandler.SendActionNotificationEvent -= ForceUpdateBag;
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
    
    private  bool IsBusy
    {
        get
        {
            lock(_isbusyLock)
            {
                return _isBusy;
            }
        }
        set
        {
            lock(_isbusyLock)
            {
                _isBusy = value;
            }
        }
    }
}
