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

    private EnemyHealthBar healthBar;

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

        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.SetMaxHealth(healthBar.waterEnemyMaxHealth);
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
    // Chamado pela animação
    private void SpawnAttack()
    {
        // Mão do inimigo

        Transform handTransform = transform
                                    .GetChild(0)  // Model
                                    .GetChild(0)  // Hips
                                    .GetChild(2)  // Spine
                                    .GetChild(0)  // Spine1
                                    .GetChild(0)  // Spine2
                                    .GetChild(0)  // LeftShoulder
                                    .GetChild(0)  // LeftArm
                                    .GetChild(0)  // LeftForearm
                                    .GetChild(0)  // LeftHand
                                    .GetChild(2);  // LeftHandPinky
                                     

        // Instancia o ataque em direção ao player
        Rigidbody rb = Instantiate(projectile, handTransform.position, Quaternion.identity).GetComponent<Rigidbody>();
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
    // Olhar para o player
    public void ChasePlayer()
    {
        // Calculate the direction to the player
        Vector3 direction = player.position - transform.position;

        // Zero out the Y component of the direction to keep the rotation around the Y axis only
        direction.y = 0;

        // Check if the direction is not zero to avoid NaN results when normalizing
        if (direction != Vector3.zero)
        {
            // Calculate the rotation needed to look at the player
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Apply the rotation, but only affect the Y axis
            transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
    }
    // Chamado pelo ataque do inimigo (script à parte)
    public void DealDamage()
    {
        GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().TakeDamage(EnemyDamage.waterEnemyDamage);
    }


    public void TakeDamage(float damage)
    {
        healthBar.TakeDamage(damage);
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
