using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InitializeGame : MonoBehaviour
{
    private PlatformFactory platforms;
    private List<Vector3> platformPositions;
    private PlayerFactory playerFactory;
    private GameObject player;
    private MazeFactory maze;
    private GameObject bakery;
    private BakeryFactory navMeshSurfaceBakery;
    private EnemiesFactory enemies;
    private GameObject npc;
    private WeaponsFactory weapons;
    private List<GameObject> ceilings;
    private WalkableSpaceBakery walkableSpaceBakery;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }
    public void StartGame()
    {
        // Inicializa os tipos de plataforma para se usar ao criar as plataformas
        PlatformTypes platformTypes = (PlatformTypes)ScriptableObject.CreateInstance(typeof(PlatformTypes));
        platformTypes.LoadPlatforms();

        // Inicializa a vida do jogador
        GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().InitHealthBar(16, 1006);

        // Dá reset ao stats do jogador (exceto os pontos)
        PlayerStats.ResetStats();

        // Gera as plataformas
        platforms = (PlatformFactory)ScriptableObject.CreateInstance(typeof(PlatformFactory));
        platformPositions = platforms.GeneratePlatforms(platformTypes);

        // Gera o jogador
        playerFactory = (PlayerFactory)ScriptableObject.CreateInstance(typeof(PlayerFactory));
        player = playerFactory.GeneratePlayer(platformPositions);

        // Gera os muros para criar um labirinto no chão e apaga o teto para dar espaço às plataformas
        maze = (MazeFactory)ScriptableObject.CreateInstance(typeof(MazeFactory));
        maze.GenerateMaze(platforms);

        // Calcula os espaços em que os inimigos podem caminhar
        navMeshSurfaceBakery = (BakeryFactory)ScriptableObject.CreateInstance(typeof(BakeryFactory));
        bakery = navMeshSurfaceBakery.GenerateBakery();
        walkableSpaceBakery = bakery.GetComponent<WalkableSpaceBakery>();
        walkableSpaceBakery.Bake();

        // Gera os inimigos consoante o nível
        enemies = (EnemiesFactory)ScriptableObject.CreateInstance(typeof(EnemiesFactory));
        enemies.GenerateEnemies(player);

        // 50% de chance de gerar um NPC
        NpcFactory npcFactory = (NpcFactory)ScriptableObject.CreateInstance(typeof(NpcFactory));
        npc = npcFactory.GenerateNPC();

        // Gera armas para o jogador poder apanhar
        weapons = (WeaponsFactory)ScriptableObject.CreateInstance(typeof(WeaponsFactory));
        weapons.GenerateWeapon();


        // Nome do nível no canvas
        GameObject.FindGameObjectWithTag("LevelText").GetComponent<TextMeshProUGUI>().text = "Level: " + PlayerStats.currentLevel;
        GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>().text = "Score: " + PlayerStats.points;
    }
    public void ResetGame()
    {
        if (platforms != null)
        {
            platforms.DestroyPlatforms();
            platforms.ResetCeilings();
        }
                
        if(maze != null)
            maze.DestroyMaze();

        if (bakery != null)
        {
            if(walkableSpaceBakery != null)
            {
                walkableSpaceBakery.UnBake();
                Destroy(walkableSpaceBakery.gameObject);
            }
            Destroy(bakery);
        }

        if(enemies != null)
            enemies.DestroyEnemies();

        if (npc != null)
            Destroy(npc);

        if(weapons != null)
            weapons.DestroySpawnedWeapon();

        if (player != null)
            Destroy(player);

        if(ceilings != null && ceilings.Count > 0)
            for (int i = 0; i < ceilings.Count; i++)
            {
                ceilings[i].SetActive(true);
            }

        StartGame();
    }
}
