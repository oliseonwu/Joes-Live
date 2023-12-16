using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    [Range(0, 10)] public float moveSpeed;
    public SpriteRenderer _spriteRenderer;
    
    [Tooltip("direction the bird will face and fly towards." +
             "For left = -1, for right = 1 for no movement = 0")]
    private float direction = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    public void SetBirdDirection(int direction)
    {
        this.direction = direction;

        if (direction < 0)
        {
            // flip the direction the bird faces when flying
            _spriteRenderer.flipX = false;
        }
    } 
    private void Move()
        {
            rb.velocity= new Vector3(direction * moveSpeed,0.0f, 0.0f) * Time.deltaTime;
        }
    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag != "SkyArea")
        {
          return;  
        }
        
        Debug.Log("Bird left the view!");
        Destroy(gameObject, 5);

    }
}
