using System;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;
using UnityEngine;

public class TikTokListener : MonoBehaviour
{
    private WebSocket ws;
    public GiftBatchHandler giftBatchHandler;
    private int reconnectAttempts = 0;
    private const int MaxReconnectAttempts = 10;
    private float reconnectDelay = 1f; // Delay in seconds
    public UnityMainThreadDispatcher unityMainThreadDispatcher;
    void Start()
    {
        ConnectToWebSocket();
    }
    
    private void ConnectToWebSocket()
    {
        ws = new WebSocket("ws://localhost:8080");
        ws.OnMessage += OnMessageReceived;
        ws.OnClose += OnWebSocketClosed;
        ws.Connect();
    }

    void OnMessageReceived(object Serverws,  MessageEventArgs e)
    {
         String[] data = e.Data.Split(",");

        if (data[0].Trim().Equals("gift") )
        {
            UnityMainThreadDispatcher.Enqueue(()=>giftBatchHandler.addToGiftIdContainer(data[3], 1));
            

        }
        
        
        // {"type":"gift","uniqueId":"1rabbitcatcher","giftName":"Rose"}
    }

    private void OnWebSocketClosed(object sender, CloseEventArgs e)
    {
        Debug.Log("Disconnected from socket server. Attempting to reconnect...");
        if (reconnectAttempts < MaxReconnectAttempts)
        {
            UnityMainThreadDispatcher.Enqueue(()=> Invoke(nameof(Reconnect), reconnectDelay));
            reconnectDelay *= 2; // Exponential backoff
            reconnectAttempts++;
        }
        else
        {
            Debug.LogError("Maximum reconnect attempts reached. Stopping reconnection attempts.");
        }
    }

    private void Reconnect()
    {
        Debug.Log("Reconnecting to server");
        ConnectToWebSocket();
    }

    
}
