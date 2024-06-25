using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlatformPortal : MonoBehaviour
{
    // Quando o player entra no portal final
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            // Aumentar o nível de jogo
            PlayerStats.currentLevel += 1;
            // Ganha 100 pontos por chegar ao fim
            PlayerStats.points += 100;
            GameObject.FindGameObjectWithTag("EventSystem").GetComponent<InitializeGame>().ResetGame();
        }
    }
}
