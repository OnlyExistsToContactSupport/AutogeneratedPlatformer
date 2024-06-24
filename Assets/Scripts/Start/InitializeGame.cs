using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Inicializa os tipos de plataforma para se usar ao criar as plataformas
        PlatformTypes platformTypes = (PlatformTypes)ScriptableObject.CreateInstance(typeof(PlatformTypes));
        platformTypes.LoadPlatforms();

        // Inicializa a vida do jogador
        GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().InitHealthBar(16, 1006);

        // D� reset ao stats do jogador (exceto os pontos)
        PlayerStats.ResetStats();

        // Gera o jogador
        PlayerFactory playerFactory = (PlayerFactory)ScriptableObject.CreateInstance(typeof(PlayerFactory));
        GameObject player = playerFactory.GeneratePlayer();

        // Gera as plataformas
        PlatformFactory platforms = (PlatformFactory)ScriptableObject.CreateInstance(typeof(PlatformFactory));
        platforms.GeneratePlatforms(platformTypes, player);

        // Gera os muros para criar um labirinto no ch�o e apaga o teto para dar espa�o �s plataformas
        MazeFactory maze = (MazeFactory)ScriptableObject.CreateInstance(typeof(MazeFactory));
        maze.GenerateMaze(platforms);

        // Calcula os espa�os em que os inimigos podem caminhar
        BakeryFactory navMeshSurfaceBakery = (BakeryFactory)ScriptableObject.CreateInstance(typeof(BakeryFactory));
        navMeshSurfaceBakery.GenerateBakery().GetComponent<WalkableSpaceBakery>().Bake();

        // Gera os inimigos consoante o n�vel
        EnemiesFactory enemies = (EnemiesFactory)ScriptableObject.CreateInstance(typeof(EnemiesFactory));
        enemies.GenerateEnemies();

        // 50% de chance de gerar um NPC
        NpcFactory npc = (NpcFactory)ScriptableObject.CreateInstance(typeof(NpcFactory));
        npc.GenerateNPC();

        // Gera armas para o jogador poder apanhar
        WeaponsFactory weapons = (WeaponsFactory)ScriptableObject.CreateInstance(typeof(WeaponsFactory));
        weapons.GenerateWeapon();

    }
}
