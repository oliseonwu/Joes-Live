using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car : MonoBehaviour
{
    [SerializeField]private Rigidbody rb;
    [Range(0, 200)] public float moveSpeed;
    
    [Tooltip("direction the bird will face and fly towards." +
             "For left = -1, for right = 1 for no movement = 0")]
    private float direction = 0;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setup(int direction)
    {
        this.direction = direction;
    }
    
    private void Move()
    {
        rb.velocity= new Vector3(direction * moveSpeed,0.0f, 0.0f) * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Move();
    }
}
