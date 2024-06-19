using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class BakeWalkableSpace : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        NavMeshSurface navMeshSurface = GetComponent<NavMeshSurface>();
        if (navMeshSurface != null)
        {
            // Build the NavMesh at runtime
            navMeshSurface.BuildNavMesh();
            Debug.Log("NavMesh baked at runtime.");
        }
        else
        {
            Debug.LogError("No NavMeshSurface component found on this GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
