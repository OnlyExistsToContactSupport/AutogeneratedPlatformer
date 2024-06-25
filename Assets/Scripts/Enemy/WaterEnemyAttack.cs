using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEnemyAttack : MonoBehaviour
{
    // Usado pelo collider do ataque para atacar o player
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().TakeDamage(EnemyDamage.waterEnemyDamage);
        }
        else if(!other.tag.Equals("Platform"))
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().TakeDamage(EnemyDamage.waterEnemyDamage);
        }
        else if(!collision.gameObject.tag.Equals("Platform"))
        {
            Destroy(gameObject);
        }
    }
}
