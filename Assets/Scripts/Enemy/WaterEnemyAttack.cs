using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEnemyAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GameObject.FindGameObjectWithTag("WaterEnemy").GetComponent<WaterEnemy>().DealDamage();
        }
    }
}
