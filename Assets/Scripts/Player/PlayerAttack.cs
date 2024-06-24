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
    public float bulletSpeed;




    private Animator animator;
    private HealthBar playerHealthBar;
    private Rigidbody rb;

    private float horizontalInput;
    private float verticalInput;
    private PlayerController player;
    private bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        // Não tem inimigo para atacar no início
        enemy = null;
        enemyTag = string.Empty;
        HasAttacked = false;

        isAttacking = false;

        animator = GetComponent<Animator>();
        playerHealthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();

        // Está a andar ou correr (não consegue atacar e andar ao mesmo tempo)
        if (!isAttacking)
        {
            player.canMove = true;

            // Valores de movimento
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (horizontalInput != 0 || verticalInput != 0)
            {
                // Correr
                if (Input.GetKey(PlayerStats.runKey))
                {
                    animator.SetBool("isRunning", true);
                    animator.SetBool("isWalking", false);
                }
                // Andar
                else
                {
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isRunning", false);
                }
            }
            else
            {
                // Está parado (Idle)
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
        }


        // Morreu
        if (playerHealthBar.VerificarMorte())
        {
            animator.SetBool("isDead", true);
            player.canMove = false;
        }
    }

    public void Attack()
    {
        // Se for melee - OnTriggerEnter no Jogador para identificar o inimigo e tirar-lhe dano
        // Se for ranged - Script para a arma e usar o trigger dela

        // Atacar
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Não consegue andar enquanto ataca, mas pode cair
            player.canMove = false;
            isAttacking = true;
            string attackAnimationName = "";


            // Não pode atacar se já estiver a atacar
            if (!HasAttacked)
            {
                // Arma que o jogador tem
                PlayerWeapons.WeaponType activeWeapon = PlayerWeapons.GetActiveWeapon();

                // Arma melee - punho
                if (activeWeapon.Equals(PlayerWeapons.WeaponType.Punch))
                {
                    animator.SetBool("isPunching", true);
                    attackAnimationName = "RightHookAnimation";

                    // Verificar se está em range do inimigo para dar dano
                    if (enemy)
                    {
                        HasAttacked = true;
                        // Buscar dano da arma
                        int damage = PlayerWeapons.punchDamage;

                        if (enemyTag.Equals("AirEnemy"))
                        {
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
                } // Arma melee - espada
                else if(activeWeapon.Equals(PlayerWeapons.WeaponType.Sword))
                {
                    animator.SetBool("isSwordAttacking", true);
                    attackAnimationName = "SwordSlashAnimation";
                    // Verificar se está em range do inimigo para dar dano
                    if (enemy)
                    {
                        HasAttacked = true;
                        // Buscar dano da arma
                        int damage = PlayerWeapons.swordDamage;

                        if (enemyTag.Equals("AirEnemy"))
                        {
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
                }// Arma ranged - pistola
                else
                {
                    animator.SetBool("isShooting", true);
                    attackAnimationName = "ShootGunAnimation";

                    // Dano está no script da bala, visto que só ocorre quando ela bate num inimigo e pertence a outro objeto
                }
            }
            // Dar reset ao ataque passado o tempo da animação
            if (isAttacking)
            {
                if (!string.IsNullOrEmpty(attackAnimationName))
                {
                    Invoke(nameof(ResetAttack), GetAnimationTime(attackAnimationName));
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator.SetBool("isPunching", false);
            animator.SetBool("isSwordAttacking", false);
            animator.SetBool("isShooting", false);
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
        if(other.tag.Equals("AirEnemy") || other.tag.Equals("GroundEnemy") || other.tag.Equals("WaterEnemy"))
        {
            enemy = null;
            enemyTag = string.Empty;
        }
    }
    // Chamado na animação de disparar
    public void Shoot()
    {
        Transform gunPos = GameObject.FindGameObjectWithTag("Gun").GetComponentInChildren<Transform>();
        GameObject bullet = Instantiate(Resources.Load("Weapons/Bullet", typeof(GameObject)), gunPos.position, gunPos.rotation) as GameObject;
        if(Camera.current != null)
        {
            bullet.GetComponent<Rigidbody>().velocity = Camera.current.transform.forward * bulletSpeed;
        }
        else
        {
            bullet.GetComponent<Rigidbody>().velocity = GameObject.FindGameObjectWithTag("Player").transform.forward * bulletSpeed;
        }        
        
    }
    // Chamado na animação da espada
    public void Swing()
    {

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
    private void ResetAttack()
    {
        isAttacking = false;
    }
}
