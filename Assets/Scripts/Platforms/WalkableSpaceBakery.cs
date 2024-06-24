using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class WalkableSpaceBakery : MonoBehaviour
{
    public void Bake()
    {
        gameObject.AddComponent<NavMeshSurface>();
        gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
