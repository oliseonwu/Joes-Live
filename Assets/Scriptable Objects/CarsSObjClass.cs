using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(fileName = "Car", menuName = "ScriptableObj/Cars")]
public class CarsSObjClass : ScriptableObject
{
    public GameObject  prefab;
    public List<Sprite> sprites;
    public static float coolDown = 10;
    public static int numOfCarsOnScreen = 0;
    private static readonly object numOfCarsOnScreenLock = new ();

    public void spawn(Transform leftSpawnPos, Transform rightSpawnPos )
    {
        int moveDir = RandomNumberGenerator.GetInt32(0, 2);
        int carSpriteIndex = RandomNumberGenerator.GetInt32(0, sprites.Count);

        Sprite carSprite = sprites[carSpriteIndex];
        
        moveDir = (moveDir == 0) ? -1 : 1;
        
        // Instantiate the car
        GameObject carGameObj = Instantiate(prefab, ((moveDir < 0)? rightSpawnPos.transform.position :
            leftSpawnPos.transform.position), Quaternion.identity);
        
        // Build the car
        carGameObj.GetComponent<car>().setup(moveDir,  
            getSpeedByCarName(carSprite.name),carSprite );
    }
    
    public static int NumOfBirdsOnScreen
    {
        get
        {
            lock(numOfCarsOnScreenLock)
            {
                return numOfCarsOnScreen;
            }
        }
        set
        {
            lock(numOfCarsOnScreenLock)
            {
                numOfCarsOnScreen = value;
            }
        }
    }
    
    private float getSpeedByCarName(String name)
    {
        float speed;
        name = name.Split("_")[0];

        Debug.Log(name);
        
        switch(name)
        {
            case "Bus":
                speed = 20;
                break;
            case "Sport Car":
                speed = 100;
                break;
            default:
                speed = 30;
                break;
        }

        return speed;
    }
    
    
    
}
