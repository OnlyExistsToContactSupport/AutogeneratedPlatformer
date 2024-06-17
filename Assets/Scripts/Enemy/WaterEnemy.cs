using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEnemy : MonoBehaviour, IEnemyBehaviour
{
    // Player
    private Transform player;
    public bool isPlayerInRange;
    private int fovAngle;
    private float range;

    //Attack
    public GameObject projectile;
    private bool hasAttacked;
    private bool hasStoodUp;


    // Animações
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        isPlayerInRange = false;

        hasAttacked = false;
        hasStoodUp = false;
        range = 50f;
        // Consegue ver em todos os angulos
        fovAngle = 360;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayerInRange)
        {
            Patrol();
        }
        else
        {
            ChasePlayer();
            AttackPlayer();
        }
    }

    // O Patrol do WaterEnemy é uma meditação que o faz ver tudo à sua volta

    public void Patrol()
    {
        isPlayerInRange = CanSeePlayer();
        hasStoodUp = false;

        animator.SetBool("isSeeingPlayer", isPlayerInRange);
        animator.SetBool("isAttacking", false);
    }

    public void AttackPlayer()
    {
        isPlayerInRange = CanSeePlayer();
        if (hasStoodUp)
        {
            if (!hasAttacked && isPlayerInRange)
            {
                animator.SetBool("isSeeingPlayer", true);
                animator.SetBool("isAttacking", true);

                Invoke("SpawnAttack", GetAnimationTime("AttackAnimation"));

                // Voltar a atacar passado algum tempo(2 segundos)
                Invoke(nameof(ResetAttack), 2f);
            }
        }
        else
        {
            animator.Play("StandUp");

            
            //Esperar que se levante antes de atacar
            StartCoroutine(
                WaitForStandUp(
                    GetAnimationTime("StandUpAnimation")));
        }
    }
    private void SpawnAttack()
    {
        Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50f, ForceMode.Impulse);

        hasAttacked = true;

        Destroy(rb.gameObject, 3f);

    }
    private float GetAnimationTime(string animationName)
    {
        float standUpAnimationDuration = 0;
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Equals(animationName))
            {
                standUpAnimationDuration = clip.length;
                break;
            }
        }
        return standUpAnimationDuration;
    }
    private IEnumerator WaitForStandUp(float standUpAnimationDuration)
    {

        yield return new WaitForSeconds(standUpAnimationDuration);

        hasStoodUp = true;
    }
    private void ResetAttack()
    {
        hasAttacked = false;
        animator.SetBool("isAttacking", false);
    }
    // Olhar para o player (cabeça only para ser mais creepy)
    public void ChasePlayer()
    {
        // WaterEnemy - Model - Hips - Spine - Spine1 - Spine2 - Neck - Head
        Transform headTransform = transform
                                    .GetChild(0)  // Model
                                    .GetChild(0)  // Hips
                                    .GetChild(2)  // Spine
                                    .GetChild(0)  // Spine1
                                    .GetChild(0)  // Spine2
                                    .GetChild(1)  // Neck
                                    .GetChild(0); // Head

        headTransform.LookAt(player);
    }

    public void DealDamage()
    {
        throw new System.NotImplementedException();
    }


    public void TakeDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
    private bool CanSeePlayer()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, range);
        bool playerFound = false;

        foreach (Collider collider in rangeChecks)
        {
            if (collider.transform.tag.Equals("Player"))
            {
                Vector3 directionToTarget = (collider.transform.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, collider.transform.position);

                    if (distanceToTarget < range)
                    {
                        playerFound = true;
                        break;
                    }
                }
            }

        }

        return playerFound;
    }
}
