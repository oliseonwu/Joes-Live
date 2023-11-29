using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TikTokGiftApi : MonoBehaviour
{
    // Start is called before the first frame update

    public static Dictionary<String, String> GiftNameToId = new Dictionary<string, string>
    {
        {"Rose","5655"},
        {"Lightning Bolt","6652"},
        {"Hat and Mustache", "6427"},
        // {"Wide Eye Wurstie", "6774"}
    };
    
    public static Dictionary<String, String> GiftIdToName = new Dictionary<string, string>
    {
        {"5655","Rose"},
        {"6652","Lightning Bolt"},
        {"6427", "Hat and Mustache"},
        // {"6774", "Wide Eye Wurstie"}
    };
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
