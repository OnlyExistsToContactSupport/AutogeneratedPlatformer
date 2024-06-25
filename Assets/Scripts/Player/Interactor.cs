using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Interactable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform source;
    
    private float distance;

    private void Start()
    {
        distance = 10f;
    }
    // C�digo para interagir com m�ltiplos objetos, implementados em cada um deles
    void Update()
    {
        if (Input.GetKeyDown(PlayerStats.interactKey))
        {
            Ray ray = new Ray(source.position, source.forward);
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

            if(Physics.Raycast(ray, out RaycastHit hit, distance))
            {
                if(hit.collider.gameObject.TryGetComponent(out Interactable obj))
                {
                    Debug.Log("Interagiu com: " + hit.collider.gameObject.name);
                    obj.Interact();
                }
            }
        }    
    }
}
