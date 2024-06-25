using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float groundDrag;

    public float jumpCooldown;
    public float airMultiplier;
    private bool isRunning;
    private bool jumpedOffWall;

    [Header("Ground Check")]
    public float playerHeight;

    public Transform orientation;

    public bool canMove;

    private CharacterController characterController;
    private Vector3 gravity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        characterController = GetComponent<CharacterController>();

        canMove = true;
        isRunning = false;
        jumpedOffWall = false;
    }

    private void Update()
    {
        // Se não estiver a falar com npc ou não estiver em pausa
        if (!DialogueController.isDialogue || Time.timeScale > 0f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Efeito da gravidade
            gravity.y -= 9.8f * Time.deltaTime;
            characterController.Move(gravity * Time.deltaTime);

            MovePlayer();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


    private void MovePlayer()
    {
        if(canMove)
        {
            Vector3 playerMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxis("Vertical")); ;
            Vector3 playerPosition = transform.TransformDirection(playerMovement);

            if (characterController.isGrounded)
            {
                // Quando está no chão, já pode voltar a saltar da parede
                jumpedOffWall = false;

                // Correr
                if (Input.GetKeyDown(PlayerStats.runKey))
                {
                    isRunning = true;

                }
                else if (Input.GetKeyUp(PlayerStats.runKey))
                {
                    isRunning = false;
                }

                // Saltar
                if (Input.GetKey(PlayerStats.jumpKey))
                {
                    gravity.y = PlayerStats.jumpForce;
                }
            }


            characterController.Move(playerPosition * (isRunning? PlayerStats.runSpeed : PlayerStats.walkSpeed ) * Time.deltaTime);

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Wall"))
        {
            if (Input.GetKey(PlayerStats.jumpKey))
            {
                if(!jumpedOffWall)
                {
                    gravity.y = PlayerStats.jumpForce;
                    jumpedOffWall = true;
                }
            }
        }
    }
}
