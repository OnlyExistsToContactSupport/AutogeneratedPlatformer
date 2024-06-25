using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesFactory : ScriptableObject
{
    private List<GameObject> enemies;
    // S� pode ser utilizado depois das plataformas e o jogador darem spawn,
    //  visto que o WaterEnemy substitui uma das plataformas e o GroundEnemy d� spawn no ponto oposto ao jogador
    public void GenerateEnemies(GameObject player)
    {
        /*
         * O GroundEnemy ir� nascer no ponto oposto ao jogador, no n�vel do ch�o
         * O WaterEnemy tem uma plataforma dele, pelo que ir� substituir uma ou mais das j� existentes
         * O AirEnemy pode voar, pelo que nascer� num ponto no ar
         */

        /*      Podem dar spawn um m�ximo de 4 GroundEnemies      */
        /*      Podem dar spawn um m�ximo de 3 WaterEnemies       */
        /*      Podem dar spawn um m�ximo de 2 AirEnemies       */

        enemies = new List<GameObject>();

        if(PlayerStats.currentLevel == 1)
        {
            // Primeiro n�vel s� d� spawn de 1 inimigo do ch�o, no canto oposto ao jogador
            SpawnGroundEnemy(-player.transform.position, player);
        }
        else if(PlayerStats.currentLevel <= 3)
        {
            // Spawn de 1 inimigo do ch�o, no canto oposto ao jogador
            SpawnGroundEnemy(-player.transform.position, player);

            // Spawn do primeiro inimigo de �gua
            SpawnWaterEnemy(1, player);
        }
        else if (PlayerStats.currentLevel <= 5)
        {
            // Spawn de 3 inimigos do ch�o, um longe do jogador e um no meio do mapa
            SpawnGroundEnemy(-player.transform.position, player);
            SpawnGroundEnemy(new Vector3(0, 0, 0), player);

            // Spawn de um inimigo de �gua
            SpawnWaterEnemy(1, player);
        }
        else if(PlayerStats.currentLevel <= 8)
        {
            // Spawn de 3 inimigos do ch�o, um longe do jogador e um no meio do mapa e um num canto ao lado
            SpawnGroundEnemy(-player.transform.position, player);
            SpawnGroundEnemy(new Vector3(0, 0, 0), player);
            SpawnGroundEnemy(new Vector3(player.transform.position.x, 0, -player.transform.position.z), player);

            // Spawn de dois inimigos de �gua
            SpawnWaterEnemy(2, player);
        }
        else if(PlayerStats.currentLevel <= 11)
        {
            // Spawn de 4 inimigos do ch�o, um longe do jogador e um no meio do mapa e dois num canto ao lado
            SpawnGroundEnemy(-player.transform.position, player);
            SpawnGroundEnemy(new Vector3(0, 0, 0), player);
            SpawnGroundEnemy(new Vector3(player.transform.position.x, 0, -player.transform.position.z), player);
            SpawnGroundEnemy(new Vector3(-player.transform.position.x, 0, player.transform.position.z), player);

            // Spawn de tr�s inimigos de �gua
            SpawnWaterEnemy(3, player);

            // Spawn do primeiro inimigo voador
            SpawnAirEnemy(1);
        }
        else if(PlayerStats.currentLevel > 11)
        {
            // Spawn de tudo
            SpawnGroundEnemy(-player.transform.position, player);
            SpawnGroundEnemy(new Vector3(0, 0, 0), player);
            SpawnGroundEnemy(new Vector3(player.transform.position.x, 0, -player.transform.position.z), player);
            SpawnGroundEnemy(new Vector3(-player.transform.position.x, 0, player.transform.position.z), player);
            SpawnWaterEnemy(3, player);
            SpawnAirEnemy(2);
        }
    }
    private void SpawnGroundEnemy(Vector3 position, GameObject player)
    {
        GameObject groundEnemy = Resources.Load("Enemies/GroundEnemy") as GameObject;
        groundEnemy.GetComponent<GroundEnemy>().SetPlayer(player);

        enemies.Add(Instantiate(groundEnemy, position, Quaternion.identity));
    }
    private void SpawnWaterEnemy(int quantity, GameObject player)
    {
        // Lista as plataformas sem as perto do ch�o ou do teto
        List<GameObject> platforms = GameObject.FindGameObjectsWithTag("Platform")
            .Where(x => x.transform.position.y > 20 || !(x.gameObject.GetComponentInChildren<EndPlatformPortal>() != null)).ToList();

        GameObject waterEnemy = Resources.Load("Platforms/WaterEnemyPlatform") as GameObject;
        waterEnemy.GetComponentInChildren<WaterEnemy>().SetPlayer(player);

        for (int i = 0; i < quantity; i++)
        {
            // Plataforma random
            int platformIndex = Random.Range(0, platforms.Count);
            GameObject platform = platforms[platformIndex];

            Vector3 position = platform.transform.position;

            enemies.Add(Instantiate(waterEnemy, position, Quaternion.identity));

            // Remover a plataforma utilizada
            platforms.RemoveAt(platformIndex);
            Destroy(platform);

        }
    }
    private void SpawnAirEnemy(int quantity)
    {
        GameObject redAirEnemy = Resources.Load("Enemies/RedAirEnemy") as GameObject;
        // H� 2 spawn points
        GameObject[] points = GameObject.FindGameObjectsWithTag("AirEnemySpawnPoint");

        if(quantity == 1)
        {
            enemies.Add(Instantiate(redAirEnemy, points[0].transform.position, Quaternion.identity));
        }
        else
        {
            GameObject blackAirEnemy = Resources.Load("Enemies/BlackAirEnemy") as GameObject;

            enemies.Add(Instantiate(redAirEnemy, points[0].transform.position, Quaternion.identity));
            enemies.Add(Instantiate(blackAirEnemy, points[1].transform.position, Quaternion.identity));
        }
    }
    public void DestroyEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                Destroy(enemies[i]);
            }
        }
    }
}
