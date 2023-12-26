using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{ 
    public TextMeshPro bubbletext;
    public GameObject bgImgGameObj;

    public Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        SetChatText("hello what up how are you ?");
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

        Vector2 padding =  new Vector2(2.5f, 1f);
        // Vector2 padding =  new Vector2(0f, 0f);

        
        bgImgGameObj.GetComponent<SpriteRenderer>().size = textSizeInfo + padding;

        
        // Invoke(nameof(hideBubble), 10);
        
    }

    private void hideBubble()
    {
        bgImgGameObj.SetActive(false);
        bubbletext.alpha = 0;
    }
    
    private void showBubble()
    {
        
        bgImgGameObj.SetActive(true);
        bubbletext.alpha = 1;
        _animator.SetTrigger("appear");
    }
}
