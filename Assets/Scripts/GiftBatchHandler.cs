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

    private ConcurrentDictionary<String, int> _giftIdContainer = new ConcurrentDictionary<string, int>();
    
    
    private ConcurrentDictionary<String, int> Stash = new ConcurrentDictionary<string, int>();

    private List<String> _allowedGiftsIds = new List<string>();
    
        // 1- ROSE = 5655
        // 1- Lightning Bolt = 6652
        // 99- Hat and Mustache = 6427
        // 5- Wide Eye Wurstie = 6774
    

    public Boolean _isGiftIdContainerClearing;
    
    private readonly object _stateLock = new object();
    private readonly object _sentAlertLock = new object();
    public static event Action takeGiftIdsEvent;
    public Boolean showGiftIdContainer;
    public Boolean showStash;
    private Boolean sentAlert;
    public float updateFromStashDelay = 2f; // Delay before the first call (in seconds)
    public float updateFromStashInterval = 5f; // Interval between subsequent calls (in seconds)

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
     // Sends an alert that there is a gift in the 
     // gift Id container
     
     if (!SentAlert)
     {
         SentAlert = true;
         takeGiftIdsEvent?.Invoke();
         Debug.Log("We have some gifts come get them!");
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

        // Add the total number of gift we are taking to the 
        // first Index of avaliable Gifts
        avaliableGifts[0][1] = giftCount + "";

        IsGiftIdContainerClearing = true;
        _giftIdContainer.Clear();
        IsGiftIdContainerClearing = false;

        SentAlert = false; // reset
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
    
    public bool SentAlert
    {
        get
        {
            lock(_sentAlertLock)
            {
                return sentAlert;
            }
        }
        set
        {
            lock(_sentAlertLock)
            {
                sentAlert = value;
            }
        }
    }
    
    void StopUpdatingGiftContainerFromStash()
    {
        CancelInvoke(nameof(updateGiftIdContainerFromStash));
    }
}
