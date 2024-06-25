using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponsFactory : ScriptableObject
{
    GameObject spawnedWeapon;
    // S� pode ser utilizado depois das plataformas darem spawn,
    //  visto que destroem alguns dos pontos de spawn
    public void GenerateWeapon()
    {
        // Considerar o n�vel 
        /*
         * Se n�vel 1 - apenas punho
         * Caso contr�rio - 80% chance de n�o dar spawn a armas
         *                - 15% chance de dar spawn a espada
         *                - 5% chance de dar spawn a arma
         *                      - Random de 10 a 30 e esse ser� o n�mero de balas
         * Caso o n�vel seja maior que 10 - 50% chance cada arma
         */
        
        // O player come�a sempre sem uma arma
        PlayerWeapons.SetActiveWeapon(PlayerWeapons.WeaponType.Punch);

        if (PlayerStats.currentLevel > 1)
        {
            int weaponChance = Random.Range(0, 100);

            if(PlayerStats.currentLevel >= 10)
            {
                // 50% chance cada
                SpawnWeapon(weaponChance, 50);
            }
            else
            {
                if (weaponChance <= 80)
                {
                    return;
                }
                else
                {
                    // 80 + 15 = 95
                    SpawnWeapon(weaponChance, 95);
                }
            }
        }
    }
    private void SpawnWeapon(int weaponChance, int probability)
    {
        // Lista de pontos onde a arma pode dar spawn
        List<GameObject> weaponSpawnPoints = GameObject.FindGameObjectsWithTag("BuffSpawnPoint").ToList();
        int swordSpawnPoint = Random.Range(0, weaponSpawnPoints.Count);
        if (weaponChance <= probability)
        {
            spawnedWeapon = Instantiate(Resources.Load("Weapons/Sword"), weaponSpawnPoints[swordSpawnPoint].transform.position, Quaternion.identity) as GameObject;
            spawnedWeapon.GetComponent<GrabWeapon>().weaponType = PlayerWeapons.WeaponType.Sword;
        }
        else
        {
            spawnedWeapon = Instantiate(Resources.Load("Weapons/Gun"), weaponSpawnPoints[swordSpawnPoint].transform.position, Quaternion.identity) as GameObject;
            spawnedWeapon.GetComponent<GrabWeapon>().weaponType = PlayerWeapons.WeaponType.Sword;
        }
    }
    public void DestroySpawnedWeapon()
    {
        if(spawnedWeapon != null)
            Destroy(spawnedWeapon);
    }
}
