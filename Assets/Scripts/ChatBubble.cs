using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{ 
    public TextMeshPro bubbletext;
    public GameObject bgImgGameObj;

    public Animator _animator;

    public void SetChatText(string text, float exitTime = 10f)
    {
        CancelInvoke(nameof(hideBubble));
        showBubble();
        bubbletext.SetText(text);
        bubbletext.ForceMeshUpdate(); // Force the text to update its size(width and height) info.

        // we are saying hey get the size of the
        // most recent visible fame of text
        Vector2 textSizeInfo = bubbletext.GetRenderedValues(false);
        
        Vector2 padding =  new Vector2(2.5f, 1f);
        bgImgGameObj.GetComponent<SpriteRenderer>().size = textSizeInfo + padding;
        Invoke(nameof(hideBubble), exitTime);
    }

    private void hideBubble()
    {
        _animator.SetBool("appear", false);
    }
    
    private void showBubble()
    {
        _animator.SetBool("appear", true);
    }
}
