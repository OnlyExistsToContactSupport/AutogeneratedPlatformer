using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemy : MonoBehaviour, IEnemyBehaviour
{
    private EnemyHealthBar healthBar;

    public void AttackPlayer()
    {
        throw new System.NotImplementedException();
    }

    public void ChasePlayer()
    {
        throw new System.NotImplementedException();
    }

    public void DealDamage()
    {
        throw new System.NotImplementedException();
    }

    public void Patrol()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        healthBar.TakeDamage(damage);
    }

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.SetMaxHealth(healthBar.airEnemyMaxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
