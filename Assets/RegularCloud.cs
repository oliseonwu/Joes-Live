using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RegularCloud : MonoBehaviour
{
    private Rigidbody rb;
    [Range(1, 100)] public float moveSpeed;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _bannerImageSize;
    [FormerlySerializedAs("restpointAdjuster")] public float resetPointAdjuster = 55.22f;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bannerImageSize = _spriteRenderer.sprite.bounds.size;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move(1);
    }

    private void Move(float direction)
    {
        rb.velocity= new Vector3(direction * moveSpeed,0.0f, 0.0f) * Time.deltaTime;
    }
    
    private void setXPos()
    {
        Vector3 newPos = transform.position;
        newPos.x -=  (_bannerImageSize.x + _bannerImageSize.x);
        newPos.x += resetPointAdjuster + resetPointAdjuster;

        transform.position = newPos;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "View Area")
        {
            return;
        }
        
        Debug.Log("Looping now!");
        setXPos();
    }
}
