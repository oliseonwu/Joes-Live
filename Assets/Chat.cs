using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat : MonoBehaviour
{
    // Start is called before the first frame update
    public SpawnManager _spawnManager;
    public GameObject chatPos;
    [SerializeField] private Transform chatPosTransform;
    void Start()
    {
        type("hello youuuuu!!");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void type(string text)
    {
        ChatBubble spawnedChatBubble = _spawnManager.SpawnChat(chatPosTransform);
        spawnedChatBubble.SetChatText(text);
    }
}
