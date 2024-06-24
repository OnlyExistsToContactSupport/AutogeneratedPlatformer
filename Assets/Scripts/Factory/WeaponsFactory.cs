using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponsFactory : ScriptableObject
{
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
         * 
         */
        
        // O player come�a sempre sem uma arma
        PlayerWeapons.SetActiveWeapon(PlayerWeapons.WeaponType.Punch);

        if (PlayerStats.currentLevel > 1)
        {
            // Lista de pontos onde a arma pode dar spawn
            List<GameObject> weaponSpawnPoints = GameObject.FindGameObjectsWithTag("BuffSpawnPoint").ToList();
            
            int weaponChance = Random.Range(0, 100);
            
            if(weaponChance <= 80)
            {
                return;
            }
            else
            {
                int swordSpawnPoint = Random.Range(0, weaponSpawnPoints.Count);
                if(weaponChance <= 95)
                {
                    Instantiate(Resources.Load("Weapons/Sword"), weaponSpawnPoints[swordSpawnPoint].transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(Resources.Load("Weapons/Gun"), weaponSpawnPoints[swordSpawnPoint].transform.position, Quaternion.identity);
                }
            }
        }
    }
}
