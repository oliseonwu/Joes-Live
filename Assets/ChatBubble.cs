using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    public TextMeshPro bubbletext;
    public GameObject backgroudGameObj;

    public Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        SetChatText("hello!!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetChatText(string text)
    {
        showBubble();
        bubbletext.SetText(text);
        bubbletext.ForceMeshUpdate(); // Force the text to update its size(width and height) info.

        Vector2 textSizeInfo = bubbletext.GetRenderedValues(false); // we are saying hey get the
        // size of the most recent visible fame of text

        Vector2 padding =  new Vector2(0.4f, 0.4f);
        
        backgroudGameObj.GetComponent<SpriteRenderer>().size = textSizeInfo/2 + padding;

        
        // Invoke(nameof(hideBubble), 10);
        
    }

    private void hideBubble()
    {
        backgroudGameObj.SetActive(false);
    }
    
    private void showBubble()
    {
        
        backgroudGameObj.SetActive(true);
        _animator.SetTrigger("appear");
    }
}
