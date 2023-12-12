using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularCloud : MonoBehaviour
{
    private Rigidbody rb;
    [Range(1, 100)] public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MoveLeft();
    }

    private void MoveLeft()
    {
        Vector3 velocity = rb.velocity;
        rb.velocity= new Vector3(-1.0f * moveSpeed,0.0f, 0.0f) * Time.deltaTime;
    }
    
    private void MoveRight()
    {
        rb.AddForce(new Vector3(1.0f * moveSpeed,0.0f, 0.0f) * Time.deltaTime,ForceMode.Force);
        // if (startPos.x < gameObject.transform.position.x)
        // {
        //     Destroy(gameObject);
        // }
        
    }
}
