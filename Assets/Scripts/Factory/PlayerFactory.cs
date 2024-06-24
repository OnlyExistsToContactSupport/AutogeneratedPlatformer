using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerFactory : ScriptableObject 
{
    public GameObject GeneratePlayer(List<Vector3> platformList)
    {

        Vector3 lowestPlatform = platformList[platformList.Count-1];
        Vector3 playerSpawn = new Vector3(45 * - Math.Sign(lowestPlatform.x), 0, 45 * - Math.Sign(lowestPlatform.z));
        

        GameObject player = Instantiate(Resources.Load("Player/Player") as GameObject, playerSpawn, Quaternion.identity);
        player.transform.LookAt(new Vector3(0f, 0f, 0f));

        return player;
    }
}
