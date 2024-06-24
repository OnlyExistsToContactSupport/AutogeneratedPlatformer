using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : ScriptableObject 
{
    public GameObject GeneratePlayer()
    {
        // Dar spawn ao jogador num dos cantos
        Vector3 position = new Vector3(Random.Range(0, 100) <= 50 ? 45 : -45,
                                        0,
                                        Random.Range(0, 100) <= 50 ? 45 : -45
                                        );

        GameObject player = Instantiate(Resources.Load("Player/Player") as GameObject, position, Quaternion.identity);
        player.transform.LookAt(new Vector3(0f, 0f, 0f));

        return player;
    }
}
