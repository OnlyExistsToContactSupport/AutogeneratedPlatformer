using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    /* Melee attacks
     * 
     * Punch - 5% damage
     * Sword - 25% damage
     * Gun - 50% damage
     * 
     */

    private GameObject enemy;
    private string enemyTag;
    private bool HasAttacked;

    // Start is called before the first frame update
    void Start()
    {
        enemy = null;
        enemyTag = string.Empty;
        HasAttacked = false;
    }

    // Update is called once per frame
    void Update()
    {
        HasAttacked = false;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    public void Attack()
    {
        // Se for melee - OnTriggerEnter no Jogador para identificar o inimigo e tirar-lhe dano
        // Se for ranged - Script para a arma e usar o trigger dela


        // AttackAnimation

        if (enemy && !HasAttacked)
        {
            Debug.Log("ataque");
            HasAttacked = true;
            // Buscar dano da arma
            float damage = 5;

            if(enemyTag.Equals("AirEnemy")) {
                enemy.GetComponent<AirEnemy>().TakeDamage(damage);
            }
            else if (enemyTag.Equals("GroundEnemy"))
            {
                enemy.GetComponent<GroundEnemy>().TakeDamage(damage);
            }
            else if (enemyTag.Equals("WaterEnemy"))
            {
                enemy.GetComponent<WaterEnemy>().TakeDamage(damage);
            }
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "AirEnemy":
                enemy = other.gameObject;
                enemyTag = "AirEnemy";
                break;
            case "GroundEnemy":
                enemy = other.gameObject;
                enemyTag = "GroundEnemy";
                break;
            case "WaterEnemy":
                enemy = other.gameObject;
                enemyTag = "WaterEnemy";
                break;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "AirEnemy":
                enemy = null;
                enemyTag = string.Empty;
                break;
            case "GroundEnemy":
                enemy = null;
                enemyTag = string.Empty;
                break;
            case "WaterEnemy":
                enemy = null;
                enemyTag = string.Empty;
                break;
        }
    }

}
