using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float groundDrag;

    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    private bool isRunning;

    [Header("Ground Check")]
    public float playerHeight;
    private bool canJump;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    private Animator animator;

    public bool canMove;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;

        readyToJump = true;
        canMove = true;
        isRunning = false;
    }

    private void Update()
    {
        // Se não estiver a falar com npc ou não estiver em pausa
        if (!DialogueController.isDialogue || Time.timeScale > 0f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if(canMove) {
                ProcessInput();
                SpeedControl();
                MovePlayer();

                // handle drag
                if (canJump)
                    rb.drag = groundDrag;
                else
                    rb.drag = 0; 
            }

        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Toca no chão
        if (other.tag.Equals("Ground") || other.tag.Equals("Platform"))
        {
            canJump = true;
        }
    }

    // Não toca no chão
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Ground") || other.tag.Equals("Platform"))
        {
            canJump = false;
        }
    }


    private void ProcessInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Correr
        if (Input.GetKeyDown(PlayerStats.runKey))
            isRunning = true;
        else if(Input.GetKeyUp(PlayerStats.runKey))
            isRunning = false;

        // when to jump
        if (Input.GetKey(PlayerStats.jumpKey) && readyToJump && canJump)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(isRunning)
        {
            // on ground
            if (canJump)
                rb.AddForce(moveDirection.normalized * PlayerStats.runSpeed * 10f, ForceMode.Force);

            // in air
            else if (!canJump)
                rb.AddForce(moveDirection.normalized * PlayerStats.runSpeed * 10f * airMultiplier, ForceMode.Force);

        }
        else
        {
            // on ground
            if (canJump)
                rb.AddForce(moveDirection.normalized * PlayerStats.walkSpeed * 10f, ForceMode.Force);

            // in air
            else if (!canJump)
                rb.AddForce(moveDirection.normalized * PlayerStats.walkSpeed * 10f * airMultiplier, ForceMode.Force);

        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (isRunning)
        {
            // limit velocity if needed
            if (flatVel.magnitude > PlayerStats.runSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * PlayerStats.runSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
        else
        {
            // limit velocity if needed
            if (flatVel.magnitude > PlayerStats.walkSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * PlayerStats.walkSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * PlayerStats.jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

}
