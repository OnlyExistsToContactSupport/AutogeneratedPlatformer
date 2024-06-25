using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcFactory : ScriptableObject
{
    // Só pode ser utilizado depois das plataformas darem spawn,
    //  visto que dá spawn numa
    public GameObject GenerateNPC()
    {
        // Probabilidade de dar spawn a um NPC
        if (Random.Range(0, 100) <= 50)
        {
            // Plataformas as perto do chão
            List<GameObject> platforms = GameObject.FindGameObjectsWithTag("Platform")
                .Where(x => x.transform.position.y < 20).ToList();

            int platform = Random.Range(0, platforms.Count);

            return Instantiate(Resources.Load("NPC/Boss") as GameObject, new Vector3(platforms[platform].transform.position.x,
                                                 platforms[platform].transform.position.y + 0.5f,
                                                 platforms[platform].transform.position.z
                                                 ), Quaternion.identity);
        }
        return null;
    }
}
