using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car : MonoBehaviour
{
    [SerializeField]private Rigidbody rb;
    [Range(0, 200)] public float moveSpeed;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [Tooltip("direction the bird will face and fly towards." +
             "For left = -1, for right = 1 for no movement = 0")]
    private float direction = 0;

    private float RIGHT_FACING_CAR_SIZE = 0.4385996f;


    private void Start()
    {

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setup(int direction, float speed,  Sprite sprite)
    {
        this.direction = direction;
        moveSpeed = speed;
        _spriteRenderer.sprite = sprite; // Set image of car
        
        if (direction > 0)
        {
            _spriteRenderer.flipX = true;
            transform.localScale = new Vector3(RIGHT_FACING_CAR_SIZE, 
                RIGHT_FACING_CAR_SIZE, RIGHT_FACING_CAR_SIZE);
        }
    }

    private void Move()
    {
        rb.velocity= new Vector3(direction * moveSpeed,0.0f, 0.0f) * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "View Area")
        {
            Debug.Log("Car left the View Area");
            Destroy(gameObject, 3);
        }
    }
}
