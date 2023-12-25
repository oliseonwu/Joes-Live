using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    // lets say the next animation is roseInMouth
    public GameObject spawnPoint1;
    public GameObject angryCloud;
    public Boolean spawnAngryCloud;
    public GameObject birdType1;
    public GameObject birdType2;

    public Renderer meshRenderer;
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;
    public float maxSpawnCoolDown = 40;
    public float minSpawnCoolDown = 20;

    public float maxNumOfBirdsOnscreen = 5;
    private static int numOfBirdsOnScreen;
    private static readonly object numOfBirdsOnScreenLock = new ();
    private bool onCoolDown;
    public Prefabs prefabs;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnAngryCloud)
        {
            SpawnAngryCloud();
        }
    }

    public void SpawnAngryCloud()
    {
        spawnAngryCloud = false;
        Instantiate(angryCloud, spawnPoint1.transform.position, Quaternion.identity);
    }
    
    

    private Vector3 getRandomBirdSpawPoint()
    {
        Bounds  skyBounds = meshRenderer.bounds;
        Vector3  halfOfSkyBoundsSize= skyBounds.size/2;
        Vector3 centerOfSkyBoundary = skyBounds.center;
        Vector3 spawnPoint = centerOfSkyBoundary;
        
        int randomInt = Random.Range(0, 2);
        
        // spawn bird at the left edge or right edge of the skyBounds
        spawnPoint.x = (randomInt == 0)? spawnPoint.x - halfOfSkyBoundsSize.x :
            spawnPoint.x + halfOfSkyBoundsSize.x;

        // spawn bird at a random height
        spawnPoint.y = Random.Range((centerOfSkyBoundary.y - halfOfSkyBoundsSize.y),
            (centerOfSkyBoundary.y + halfOfSkyBoundsSize.y));
        
    
        return spawnPoint;
    }
    
    public static int NumOfBirdsOnScreen
    {
        get
        {
            lock(numOfBirdsOnScreenLock)
            {
                return numOfBirdsOnScreen;
            }
        }
        set
        {
            lock(numOfBirdsOnScreenLock)
            {
                numOfBirdsOnScreen = value;
            }
        }
    }

    public ChatBubble SpawnChat(Transform ChatPosTransform)
    {
        
        GameObject chatBubbleGameObj = Instantiate(prefabs.ChatBubble, ChatPosTransform);
        ChatBubble chatBubblePrefab = chatBubbleGameObj.GetComponent<ChatBubble>();
        return chatBubblePrefab;


    }
}
