using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    // --- Controls ---
    public Boolean spawnAngryCloud;
    public Boolean spawnBird;
    
    // --- Spawn points ---
    public GameObject spawnPoint1;
    public GameObject skyArea;

    // --- Prefabs ---
    public GameObject angryCloud;
    public GameObject birdType1;
    public GameObject birdType2;

    public Renderer meshRenderer;
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;
    void Start()
    {
        Invoke(nameof(SpawnBirdWithDelay), Random.Range(minSpawnDelay, maxSpawnDelay));
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnAngryCloud)
        {
            SpawnAngryCloud();
        }
        
        if (spawnBird)
        {
            SpawnBird();
        }
    }

    public void SpawnAngryCloud()
    {
        spawnAngryCloud = false;
        Instantiate(angryCloud, spawnPoint1.transform.position, Quaternion.identity);
    }

    private void SpawnBirdWithDelay()
    {
        SpawnBird();

        // Invoke this method again with a new random delay
        Invoke("SpawnBirdWithDelay", Random.Range(minSpawnDelay, maxSpawnDelay));
    }
    
    private void SpawnBird()
    {
        
        GameObject birdGameObject = (Random.Range(1, 11) <= 2) ? birdType2 : birdType1;
        Vector3 spawnPoint = getRandomBirdSpawPoint();
        Vector3 centerOfSkyViewArea = meshRenderer.bounds.center;
        
        // 1 means left -1 means right
        int birdDirection = (spawnPoint.x < centerOfSkyViewArea.x) ? 1 : -1; 
        GameObject spawnedObject;
        
        
        

        spawnBird = false;
        spawnedObject = Instantiate(birdGameObject, spawnPoint, Quaternion.identity);

        spawnedObject.GetComponent<Bird>().SetBirdDirection(birdDirection);
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
}
