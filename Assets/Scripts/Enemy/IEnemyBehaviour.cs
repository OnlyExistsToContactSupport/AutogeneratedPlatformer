using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IEnemyBehaviour
{
    public void Patrol();
    public void ChasePlayer();
    public void AttackPlayer();
    public void TakeDamage(float damage);
    public void DealDamage();
}