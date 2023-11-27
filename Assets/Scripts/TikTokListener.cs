using System;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;
using UnityEngine;

public class TikTokListener : MonoBehaviour
{
    private WebSocket ws;
    public GiftBatchHandler giftBatchHandler;
    void Start()
    {
        ws = new WebSocket("ws://localhost:8080");
        
        ws.OnMessage += OnMessageReceived;
        ws.Connect();
        
    }

    void OnMessageReceived(object Serverws,  MessageEventArgs e)
    {
         String[] data = e.Data.Split(",");

             Debug.Log($"{data[0]}, {data[1]}, {data[2]}, {data[3]}");
        
        
        if (data[0].Trim().Equals("gift") )
        {
            giftBatchHandler.addToGiftIdContainer(data[3], 1);
        }
        
        
        // {"type":"gift","uniqueId":"1rabbitcatcher","giftName":"Rose"}
    }

    
}
