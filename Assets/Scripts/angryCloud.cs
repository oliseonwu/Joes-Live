using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class angryCloud : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    public GameObject electricity;
    private Vector3 startPos; 
    [Range(1, 100)] public float moveSpeed;
    public Animator animator;
    public static event Action shockAnimEvent;
    private AudioSource[] _audioSourceList;
    public AudioClip[] SoundList;
    private enum State{MOVE_LEFT, IDEL, MOVE_RIGHT};
    private State cloudState = State.MOVE_LEFT;
    void Start()
    {
        startPos = gameObject.transform.position;
        rb = GetComponent<Rigidbody>();
        _audioSourceList = GetComponents<AudioSource>();
        _audioSourceList[0].loop = true;
        _audioSourceList[0].PlayOneShot(SoundList[0]);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (cloudState == State.MOVE_LEFT)
        {
            MoveLeft();
        }
        if (cloudState == State.MOVE_RIGHT)
        {
            MoveRight();
        }
        
    }

    private void MoveLeft()
    {
        rb.AddForce(new Vector3(-1.0f * moveSpeed,0.0f, 0.0f) * Time.deltaTime,ForceMode.Force);
    }
    
    private void MoveRight()
    {
        rb.AddForce(new Vector3(1.0f * moveSpeed,0.0f, 0.0f) * Time.deltaTime,ForceMode.Force);
        if (startPos.x < gameObject.transform.position.x)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainPlayer")
        {
            cloudState = State.IDEL;
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            
            animator.SetBool("Shock", true);
            shockAnimEvent?.Invoke();
            electricity.gameObject.SetActive(true);
            StartCoroutine(leave());
            SoundManager.Instance.PlaySound(SoundList[1],1);
            // _audioSourceList[1].PlayOneShot(SoundList[1]);

        }
    }

    IEnumerator leave()
    {
        yield return new WaitForSeconds(2);
        electricity.gameObject.SetActive(false);
        // yield return new WaitForSeconds(0.5f);
        cloudState = State.MOVE_RIGHT;
        animator.SetBool("Shock", false);
        animator.SetBool("smirk & look left", true);

    }
}

