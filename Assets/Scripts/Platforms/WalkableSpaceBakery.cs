using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class WalkableSpaceBakery : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;
    public void Bake()
    {
        gameObject.AddComponent<NavMeshSurface>();
        navMeshSurface = gameObject.GetComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
    }
    public void UnBake()
    {
        if (navMeshSurface != null)
        {
            gameObject.GetComponent<NavMeshSurface>().RemoveData();
        }
    }
}
