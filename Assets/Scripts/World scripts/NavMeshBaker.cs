using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;

    void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        if (navMeshSurface == null)
        {
            Debug.LogError("NavMeshSurface component is missing on this GameObject.");
            return;
        }

        // Initial bake
        BakeNavMesh();
    }

    public void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BakeNavMesh();
        }
    }
}