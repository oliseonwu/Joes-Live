using System;
using WebSocketSharp;
using UnityEngine;

public class TikTokListener : MonoBehaviour
{
    private WebSocket ws; 
    void Start()
    {
        ws = new WebSocket("ws://localhost:8080");
        
        ws.OnMessage += OnMessageReceived;
        ws.Connect();
        
    }

    void OnMessageReceived(object Serverws,  MessageEventArgs e) 
    {
        Debug.Log(e.Data);
    }

    
}
