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
        distance = 5f;
    }
    // Código para interagir com múltiplos objetos, implementados em cada um deles
    void Update()
    {
        Ray ray = new Ray(source.position, source.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

        if (Input.GetKeyDown(PlayerStats.interactKey))
        {
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
