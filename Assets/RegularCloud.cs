using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularCloud : MonoBehaviour
{
    private Rigidbody rb;
    [Range(1, 100)] public float moveSpeed;

    private const String BIG_CLOUD_TAG = "Big Clouds";
    private const String MEDIUM_CLOUD_TAG = "Medium Clouds";
    private const String SMALL_CLOUD_TAG = "Small Clouds";
    private SpriteRenderer _spriteRenderer;
    private Vector3 gameObjectWidth;
    public float restpointAdjuster = 55.22f;
    
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        gameObjectWidth = _spriteRenderer.sprite.bounds.size;
        rb = GetComponent<Rigidbody>();
        Debug.Log(gameObjectWidth);
        
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 bannerViewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        
    }

    private void FixedUpdate()
    {
        Move(1);
    }

    private void Move(float direction)
    {
        Vector3 velocity = rb.velocity;
        rb.velocity= new Vector3(direction * moveSpeed,0.0f, 0.0f) * Time.deltaTime;
    }
    
    private void loopOver()
    {
        String tag = gameObject.tag;


        
        
    }

    private void setXPos()
    {
        Vector3 newPos = gameObject.transform.position;
        newPos.x -=  (gameObjectWidth.x + gameObjectWidth.x);
        newPos.x += restpointAdjuster + restpointAdjuster;
        

        gameObject.transform.position = newPos;
    }

    private void OnTriggerExit(Collider other)
    {
        Vector3 tempTransform;
        if (other.tag != "View Area")
        {
            return;
        }
        
        Debug.Log("Looping now!");
        setXPos();
        
        // switch (gameObject.tag)
        // {
        //     case BIG_CLOUD_TAG:
        //         
        //         setXPos(5f);
        //         break;
        //     default:
        //         break;
        //     
        // }
    }
}
