using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
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
        animator = GetComponent<Animator>();
        playerHealthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerController>();
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {

        // Está a andar ou correr (não consegue atacar e andar ao mesmo tempo)
        if(!isAttacking)
        {
            player.canMove = true;

            // Valores de movimento
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (horizontalInput != 0 || verticalInput != 0)
            {
                // Correr
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
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

        // Atacar
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Não consegue andar enquanto ataca, mas pode cair
            player.canMove = false;
            isAttacking = true;
            string attackAnimationName = "";
            switch (PlayerWeapons.GetActiveWeapon())
            {
                case PlayerWeapons.WeaponType.Punch:
                    animator.SetBool("isPunching", true);
                    attackAnimationName = "RightHookAnimation";
                    break;
                case PlayerWeapons.WeaponType.Sword:
                    animator.SetBool("isSwordAttacking", true);
                    attackAnimationName = "SwordSlashAnimation";
                    break;
                case PlayerWeapons.WeaponType.Gun:
                    animator.SetBool("isShooting", true);
                    attackAnimationName = "ShootGunAnimation";
                    break;
            }
            // Dar reset ao ataque passado o tempo da animação
            if(!string.IsNullOrEmpty(attackAnimationName))
            {
                Invoke(nameof(ResetAttack), GetAnimationTime(attackAnimationName));
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator.SetBool("isPunching", false);
            animator.SetBool("isSwordAttacking", false);
            animator.SetBool("isShooting", false);
        }

        // Morreu
        if (playerHealthBar.VerificarMorte())
        {
            animator.SetBool("isDead", true);
            player.canMove = false;
        }
    }
    private void ResetAttack()
    {
        isAttacking = false;
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
}
