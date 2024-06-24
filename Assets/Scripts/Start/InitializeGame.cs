using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlatformTypes platformTypes = (PlatformTypes)ScriptableObject.CreateInstance(typeof(PlatformTypes));
        platformTypes.LoadPlatforms();

        GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().InitHealthBar(16, 1006);

        PlayerStats.ResetStats();

        PlayerFactory player = (PlayerFactory)ScriptableObject.CreateInstance(typeof(PlayerFactory));
        player.GeneratePlayer();

        PlatformFactory platforms = (PlatformFactory)ScriptableObject.CreateInstance(typeof(PlatformFactory));
        platforms.GeneratePlatforms(platformTypes);

        EnemiesFactory enemies = (EnemiesFactory)ScriptableObject.CreateInstance(typeof(EnemiesFactory));
        enemies.GenerateEnemies();

        NpcFactory npc = (NpcFactory)ScriptableObject.CreateInstance(typeof(NpcFactory));
        npc.GenerateNPC();

        WeaponsFactory weapons = (WeaponsFactory)ScriptableObject.CreateInstance(typeof(WeaponsFactory));
        weapons.GenerateWeapon();

    }
}
