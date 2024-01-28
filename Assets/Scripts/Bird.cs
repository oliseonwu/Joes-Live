using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    [Range(0, 10)] public float moveSpeed;
    public SpriteRenderer _spriteRenderer;
    
    private float SMALL_BIRD_SIZE = 0.01445251f;
    private float BIG_BIRD_SIZE = 0.01990703f;
    private float BIG_BIRD_SPEED = 2;
    private float SMALL_BIRD_SPEED = 1.2f;
    
    public static int BIG_BIRD_TYPE = 1;
    public static int SMALL_BIRD_TYPE = 2;


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

    public void BirdSetUp(int direction, int birdSizeType)
    {
        setBirdType(birdSizeType);
        SetBirdDirection(direction);
    }

    private void setBirdType(int birdSizeType)
    {
        float size;
        
        size = SMALL_BIRD_SIZE;
        moveSpeed = BIG_BIRD_SPEED;
        
        // switch (birdSizeType)
        // {
        //     case 1:
        //         size = SMALL_BIRD_SIZE;
        //         moveSpeed = SMALL_BIRD_SPEED;
        //         break;
        //     default:
        //         size = BIG_BIRD_SIZE;
        //         moveSpeed = BIG_BIRD_SPEED;
        //         break;
        // }
        
        transform.localScale = new Vector3(size, size, size);
    }

    private void SetBirdDirection(int direction)
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
        if (other.tag != "SkyArea")
        {
          return;  
        }
        
        SpawnManager.NumOfBirdsOnScreen--;
        
        Destroy(gameObject, 5);

    }
}
