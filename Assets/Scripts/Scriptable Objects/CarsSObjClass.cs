using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(fileName = "Car", menuName = "ScriptableObj/Cars")]
public class CarsSObjClass : ScriptableObject
{
    public GameObject prefab;
    public GameObject rightSpawnPos;
    public GameObject leftSpawnPos;


    public void spawn()
    {
        int moveDir = RandomNumberGenerator.GetInt32(0, 2);
        moveDir = (moveDir == 0) ? -1 : 1;
        
        GameObject carGameObj = Instantiate(prefab, (moveDir < 0)? leftSpawnPos.transform.position :
            rightSpawnPos.transform.position, Quaternion.identity);
        
        carGameObj.GetComponent<car>().setup(moveDir);
        
    }
}
