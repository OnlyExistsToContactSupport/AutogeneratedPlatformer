using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Dá dano ao inimigo
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("AirEnemy") || other.tag.Equals("GroundEnemy") || other.tag.Equals("WaterEnemy"))
        {
            other.GetComponentInChildren<EnemyHealthBar>().TakeDamage(PlayerWeapons.gunDamage);
        }
        Destroy(gameObject);
    }
}
