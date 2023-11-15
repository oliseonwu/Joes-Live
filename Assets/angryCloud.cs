using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class angryCloud : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    [Range(1, 100)] public float moveSpeed;
    private enum State{MOVE, IDEL};

    private State cloudState = State.MOVE;
    public Animator animator;
    public static event Action shockAnimEvent;
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
        if (cloudState == State.MOVE)
        {
            MoveLeft();
        }
    }

    private void MoveLeft()
    {
        rb.AddForce(new Vector3(-1.0f * moveSpeed,0.0f, 0.0f) * Time.deltaTime,ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainPlayer")
        {
            cloudState = State.IDEL;
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            
            animator.SetBool("Shock", true);
            shockAnimEvent?.Invoke();
        }
    }
}

