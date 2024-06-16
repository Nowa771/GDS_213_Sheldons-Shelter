using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavMeshBaker : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;

    [SerializeField]
    private float delayBeforeBake = 3.0f; 

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
        StartCoroutine(DelayedBakeNavMesh());
    }

    IEnumerator DelayedBakeNavMesh()
    {
        yield return new WaitForSeconds(delayBeforeBake);

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