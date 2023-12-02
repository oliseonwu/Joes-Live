using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GiftBatchHandler : MonoBehaviour
{

    private ConcurrentDictionary<String, int> _giftIdContainer = new ();
    private ConcurrentDictionary<String, int> Stash = new ();
    private List<String> _allowedGiftsIds = new ();
    private readonly object _stateLock = new ();
    private readonly object _sentAlertLock = new ();
    private readonly object _giftCountLock = new ();
    
    public bool _isGiftIdContainerClearing;
    public bool showGiftIdContainer;
    public bool showStash;
    private bool _sendNotification = true;
    
    [Tooltip("Total number of gifts we have so far.")]
    public int giftCount; 
    public static event Action takeGiftIdsEvent;
    
    [Tooltip("Delay before the first attempts to update the "+
             "_giftIdContainer from the stash (in seconds).")]
    public float updateFromStashDelay = 2f; 
    
    [Tooltip("Interval between subsequent attempts to update"+
             " _giftIdContainer from the stash (in seconds).")]
    public float updateFromStashInterval = 5f;
    
    [Tooltip("wait time before this class sends a unity event"+
             " that forces the GiftBag class to update itself"+ 
             " with more gifts (in seconds).")]
    public float actionNotificationFrequency = 20f; 
    
    [Tooltip("Number of gift that must be received b4 sending"+
             "an action Notification")]
    public float numOfGiftB4SendingActionNotification = 1;

    private void Start()
    {
        populateAllowedGiftIdsContainer();
        
        InvokeRepeating(nameof(updateGiftIdContainerFromStash),
            updateFromStashDelay, updateFromStashInterval);
    }

    private void populateAllowedGiftIdsContainer()
    {
        foreach (string key in TikTokGiftApi.GiftIdToName.Keys)
        {
            _allowedGiftsIds.Add(key);
        }
    }

    private void sendAlert()
    {
     // Sends an alert that there is a gift in the gift Id
     // container. We only send a notification if GiftBag
     // class attempts to get gifts from the
     // giftIdContainer using TakeGiftsIds()
     
     if (SendNotification)
     {
         SendNotification = false;
         Debug.Log($"{nameof(GiftBatchHandler)} --> We have some gifts come get them!");
         takeGiftIdsEvent?.Invoke();
     }
    }

    private void Update()
    {
        
        if (showGiftIdContainer)
        {
            showGiftIdContainer = false;
            DisplayContainer(_giftIdContainer, "GiftID Container");
        }

        if (showStash)
        {
            showStash = false;
            DisplayContainer(Stash, "Stash");
        }
    }

    public void addToGiftIdContainer(String TikTokGiftID, int amount)
    {
        
        if (_allowedGiftsIds.Contains(TikTokGiftID))
        {
            
            if (IsGiftIdContainerClearing)
            { 
                SafelyAddOrUpdateGiftIdDic(TikTokGiftID, amount, Stash, "Stash"); // add to stach
                return;
            }
            
            SafelyAddOrUpdateGiftIdDic(TikTokGiftID, amount, _giftIdContainer);
            
            // we send an alert that we have some gifts.
            sendAlert();
        }
    }
    

    private void updateGiftIdContainerFromStash()
    { 
        if (Stash.Count <= 0 || IsGiftIdContainerClearing)
        {
            return;
        }
        
        foreach (KeyValuePair<String, int> kvp in Stash)
        {
            SafelyAddOrUpdateGiftIdDic(kvp.Key, kvp.Value, _giftIdContainer);
            
            
        }
        
        Stash.Clear();
        Debug.Log("Stash -> Gift Container SUCCESS");
        
        // we send an alert that we have some gifts.
        sendAlert();
    }
    
    private void SafelyAddOrUpdateGiftIdDic(String TikTokGiftID, int amount,
        ConcurrentDictionary<String, int> dic, String containerName = "giftId")
    {
        Boolean updated = false;

        if (!dic.ContainsKey(TikTokGiftID))
        {
            dic.TryAdd(TikTokGiftID, amount);
            
                Debug.Log("Added "+amount+" "+TikTokGiftApi.GiftIdToName[TikTokGiftID] +
                          $" to the {containerName} container" );
        }
        else
        {
            while (!updated)
            {
                int tempInt = dic[TikTokGiftID];
                
                updated = dic.TryUpdate(TikTokGiftID, amount + tempInt 
                    , tempInt);
            }

            
            Debug.Log("We now have "+dic[TikTokGiftID]+" "
                      +TikTokGiftApi.GiftIdToName[TikTokGiftID] + $" in the {containerName} Container" );
        }

        // we only want to increment the amount of gifts if 
        // we added the gift to the giftId container
        if (containerName == "giftId")
        {
            GiftCount+= amount;
        }

    }
    
    public List<String[]> TakeGiftsIds()
    {
        List<String[]> avaliableGifts = new List<String[]>();
        int giftCount = 0;

        //First Index is the total number of gift we are taking
        avaliableGifts.Insert(0, new String[]{"GiftCount", ""});
        
        foreach (KeyValuePair<String, int> kvp in _giftIdContainer)
        {
            if (kvp.Value >= 1)
            {
                avaliableGifts.Add(new []{kvp.Key, kvp.Value+""});

                giftCount += kvp.Value;
            }
        }
        
        // means GiftBag tried to take some gift but there were no gifts
        if (giftCount == 0)
        {
            SendNotification = true;
            return null;
        }

        // Add the total number of gift we are taking to the 
        // first Index of avaliable Gifts
        avaliableGifts[0][1] = giftCount + "";

        IsGiftIdContainerClearing = true;
        _giftIdContainer.Clear();
        GiftCount = 0;
        IsGiftIdContainerClearing = false;
        
        return avaliableGifts;
    }

    

    public void DisplayContainer(ConcurrentDictionary<String, int> container, String containerName )
    {
        String tempString=$"{containerName} = [";
        
        foreach (KeyValuePair<string, int> kvp in container)
        {
            tempString += $"{TikTokGiftApi.GiftIdToName[kvp.Key]}({kvp.Key}): {kvp.Value}, ";
        }
        
        tempString += "]";
        
        Debug.Log(tempString);

        
    }
    
    // helps us make sure the _isGiftContainerClearing
    // var is thread safe(Only one thread can view 
    // or update this var at a time. Others wait)
    public bool IsGiftIdContainerClearing
    {
        get
        {
            lock(_stateLock)
            {
                return _isGiftIdContainerClearing;
            }
        }
        set
        {
            lock(_stateLock)
            {
                _isGiftIdContainerClearing = value;
            }
        }
    }
    
    public bool SendNotification
    {
        get
        {
            lock(_sentAlertLock)
            {
                return _sendNotification;
            }
        }
        set
        {
            lock(_sentAlertLock)
            {
                _sendNotification = value;
            }
        }
    }
    
    public int GiftCount
    {
        get
        {
            lock(_giftCountLock)
            {
                return giftCount;
            }
        }
        set
        {
            lock(_giftCountLock)
            {
                giftCount = value;
            }
        }
    }
    
    void StopUpdatingGiftContainerFromStash()
    {
        CancelInvoke(nameof(updateGiftIdContainerFromStash));
    }
}
