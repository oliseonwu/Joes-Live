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
    private bool _sendNotification = true;
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
        // Giftbatch handler (Fast)
        
        List<String[]> tempGiftBag = giftBatchHandler.TakeGiftsIds();


        if (tempGiftBag == null)
        {
            Utilities.Print(nameof(GiftBag),"No gift found. Waiting..." );
            return;
        }

        if (isGiftBagEmpty(this))
        {
            _giftBag = tempGiftBag; // Fast Update
            Utilities.Print(nameof(GiftBag), "Updated bag success (Fast method)");
        }
        else
        {
            graduallyUpdateBag(tempGiftBag); // Slow update
        }
        
        SendAlert();
    }

    private void graduallyUpdateBag(List<String[]> tempGiftBag)
    {
        // transfers gifts from the tempGiftBag to the main gift bag.
        // (Slow if we have many types of gifts)
        IsBusy = true;
        
        for (int x = 1; x < tempGiftBag.Count; x++)
        {
            IncreaseAGiftAmount(tempGiftBag[x][0], tempGiftBag[x][1] );
        }
        
        IsBusy = false;
        
        Utilities.Print(nameof(GiftBag), "Updated bag success (Slow method)");
    }

    public Boolean isGiftBagEmpty(object sender)
    {
        bool isEmpty = _giftBag.Count <= 1;

        // If someone else tried to check if we
        // had some gift and we didn't have any
        // me make sure to send an alert when
        // we have some.
        if (isEmpty && sender.GetType() != 
            GetType())
        {
            SendNotification = true;
            Invoke(nameof(CollectMoreGifts), 0.5f);
        }
        
        return isEmpty;
    }

    public String GetNextGift()
    {
        // return the next giftId
        String returnedGiftId;
        
        if (IsBusy)
        {
            // since we came to collect gift but the Giftbag is busy
            // we can tell the gift bag to alert us when the GiftBag class is free.
            SendNotification = true;
            return null;
        }

        returnedGiftId = removeAGift(1);
        
        if (returnedGiftId == null)
        {
            Print("Gift Bag empty. Attempting to get more gifts.");
            
            // since we came to collect gift but no gift available
            // we tell the gift bag to alert us when a gift is available.
            SendNotification = true;
            
            // we wait 0.5 sec to avoid race condition with JoesAnimationManager
            // playNextAnimation() which calls this function. We use this to make
            // sure any previous calls to the playNextAnimation() in the
            // JoesAnimationManager full completes and update its state appropriately
            // before another call is made. Remember CollectMoreGifts() may trigger
            // the playNextAnimation() using events
            Invoke(nameof(CollectMoreGifts), 0.5f); 

        }

        // if giftbag is empty after successfully collecting 
        // some gifts, try to get more gifts.
        if (isGiftBagEmpty(this))
        {
            Invoke(nameof(CollectMoreGifts), 0.5f);
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
            
            Print("Busy, Come back later.");
            return null;
        }
        
        if (isGiftBagEmpty(this))
        {
            Print("Gift Bag empty. Attempting to get more gifts.");
            
            // since we came to collect gift but no gift available
            // we tell the gift bag to alert us when a gift is available.
            SendNotification = true;
            
            // we wait 0.5 sec to avoid race condition with JoesAnimationManager
            // playNextAnimation() which calls this function. We use this to make
            // sure any previous calls to the playNextAnimation() in the
            // JoesAnimationManager full completes and update its state appropriately
            // before another call is made. Remember CollectMoreGifts() may trigger
            // the playNextAnimation() using events
           Invoke(nameof(CollectMoreGifts), 0.5f); 
            
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
        if (giftIndex <= 0 || isGiftBagEmpty(this))
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
        GiftBatchHandler.SendActionNotificationEvent += CollectMoreGifts;
    }

    private void unSubscribeFromEvents()
    {
        GiftBatchHandler.takeGiftIdsEvent-= CollectMoreGifts;
        GiftBatchHandler.SendActionNotificationEvent -= CollectMoreGifts;
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
