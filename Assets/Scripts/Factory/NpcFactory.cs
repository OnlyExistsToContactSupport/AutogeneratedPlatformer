using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcFactory : ScriptableObject
{
    // Só pode ser utilizado depois das plataformas darem spawn,
    //  visto que dá spawn numa
    public void GenerateNPC()
    {
        // Probabilidade de dar spawn a um NPC
        if (Random.Range(0, 100) <= 50)
        {
            GameObject npc = Resources.Load("NPC/Boss") as GameObject;

            // Plataformas as perto do chão
            List<GameObject> platforms = GameObject.FindGameObjectsWithTag("Platform")
                .Where(x => x.transform.position.y < 20).ToList();

            int platform = Random.Range(0, platforms.Count);

            GameObject npcObj = Instantiate(npc, platforms[platform].transform.position, Quaternion.identity);
            npcObj.transform.position = new Vector3(platforms[platform].transform.position.x,
                                                    platforms[platform].transform.position.y + 0.5f,
                                                    platforms[platform].transform.position.z
                                                    );
        }
    }
}
