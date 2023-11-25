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

    private ConcurrentDictionary<String, int> _giftContainer = new ConcurrentDictionary<string, int>();
    
    
    private ConcurrentDictionary<String, int> Stash = new ConcurrentDictionary<string, int>();

    private List<String> _allowedGifts = new List<string>()
    {
        "Rose","Lightning Bolt","Hat and Mustache", "Wide Eye Wurstie"
    };

    public Boolean _isGiftContainerClearing;
    
    private readonly object _stateLock = new object();
    private readonly object _sentAlertLock = new object();
    public static event Action takeGiftEvent;
    public Boolean showGiftContainer;
    public Boolean showStash;
    private Boolean sentAlert;
    public float updateFromStashDelay = 2f; // Delay before the first call (in seconds)
    public float updateFromStashInterval = 5f; // Interval between subsequent calls (in seconds)

    private void Start()
    {
        InvokeRepeating(nameof(updateGiftContainerFromStash),
            updateFromStashDelay, updateFromStashInterval);
    }

    private void sendAlert()
    {
     // Sends an alert that there is a gift in the 
     // gift container
     
     if (!SentAlert)
     {
         SentAlert = true;
         takeGiftEvent?.Invoke();
         Debug.Log("We have some gifts come get them!");
     }
    }

    private void Update()
    {
        if (showGiftContainer)
        {
            showGiftContainer = false;
            DisplayContainer(_giftContainer, "Gift Container");
        }

        if (showStash)
        {
            showStash = false;
            DisplayContainer(Stash, "Stash");
            
        }
        
    }

    public void addToGiftContainer(String TikTokGiftName, int amount)
    {
        
        if (_allowedGifts.Contains(TikTokGiftName))
        {
            
            if (IsGiftContainerClearing)
            { 
                SafelyAddOrUpdateGiftDic(TikTokGiftName, amount, Stash, "Stash"); // add to stach
                return;
            }
            
            SafelyAddOrUpdateGiftDic(TikTokGiftName, amount, _giftContainer);
            
            // we send an alert that we have some gifts.
            sendAlert();
        }
    }
    

    private void updateGiftContainerFromStash()
    { 
        if (Stash.Count <= 0 || IsGiftContainerClearing)
        {
            return;
        }
        
        foreach (KeyValuePair<String, int> kvp in Stash)
        {
            SafelyAddOrUpdateGiftDic(kvp.Key, kvp.Value, _giftContainer);
            
            
        }
        
        Stash.Clear();
        Debug.Log("Stash -> Gift Container SUCCESS");
        
        // we send an alert that we have some gifts.
        sendAlert();
    }
    private void SafelyAddOrUpdateGiftDic(String TikTokGiftName, int amount,
        ConcurrentDictionary<String, int> dic, String containerName = "gift")
    {
        Boolean updated = false;

        if (!dic.ContainsKey(TikTokGiftName))
        {
            dic.TryAdd(TikTokGiftName, amount);

                Debug.Log("Added "+amount+" "+TikTokGiftName +
                          $" to the {containerName} container" );
        }
        else
        {
            while (!updated)
            {
                int tempInt = dic[TikTokGiftName];
                
                updated = dic.TryUpdate(TikTokGiftName, amount + tempInt 
                    , tempInt);
            }

            Debug.Log("We now have "+dic[TikTokGiftName]+" "
                          +TikTokGiftName + $" in the {containerName} Container" );
        }

    }
    
    public List<String[]> TakeGifts()
    {
        List<String[]> avaliableGifts = new List<String[]>();
        int giftCount = 0;

        //First Index is the total number of gift we are taking
        avaliableGifts.Insert(0, new String[]{"GiftCount", ""});
        
        foreach (KeyValuePair<String, int> kvp in _giftContainer)
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

        IsGiftContainerClearing = true;
        _giftContainer.Clear();
        IsGiftContainerClearing = false;

        SentAlert = false; // reset
        return avaliableGifts;
    }

    

    public void DisplayContainer(ConcurrentDictionary<String, int> container, String containerName )
    {
        String tempString=$"{containerName} = [";
        
        foreach (KeyValuePair<string, int> kvp in container)
        {
            tempString += $"{kvp.Key}, Ammount: {kvp.Value},";
        }
        
        tempString += "]";
        
        Debug.Log(tempString);

        
    }
    
    // helps us make sure the _isGiftContainerClearing
    // var is thread safe(Only one thread can view 
    // or update this var at a time. Others wait)
    public bool IsGiftContainerClearing
    {
        get
        {
            lock(_stateLock)
            {
                return _isGiftContainerClearing;
            }
        }
        set
        {
            lock(_stateLock)
            {
                _isGiftContainerClearing = value;
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
        CancelInvoke(nameof(updateGiftContainerFromStash));
    }
}
