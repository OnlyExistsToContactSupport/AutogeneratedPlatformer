using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform FirstPersonCameraPosition;
    public Transform ThirdPersonCameraPosition;

    public float sensitivity;

    private float xRotation;
    private float yRotation;

    private bool firstPerson;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        firstPerson = false;

    }

    void Update()
    {
        // Se não estiver a falar com um npc ou se não estiver em pausa
        if (!DialogueController.isDialogue || Time.timeScale > 0)
        {
            MoveCamera();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void MoveCamera()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

        if (firstPerson)
        {
            yRotation += mouseX;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            Camera.main.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

            player.rotation = Quaternion.Euler(0f, yRotation, 0f);

            Camera.main.transform.position = FirstPersonCameraPosition.position;
        }
        else
        {
            yRotation += mouseX;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            Vector3 Direction = new Vector3(0, 0, -10);
            Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);
            Camera.main.transform.position = player.position + rotation * Direction;

            Camera.main.transform.LookAt(player.position);

            player.rotation = Quaternion.Euler(0f, yRotation, 0f);

        }
    }
}
