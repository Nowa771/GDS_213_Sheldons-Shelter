using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    public Material outlineMaterial;
    private Material[] defaultMaterials; // Array to store default materials of all renderers
    private Renderer[] renderers; // Array to store all renderers attached to this GameObject

    void Start()
    {
        // Get all renderers attached to this GameObject
        renderers = GetComponentsInChildren<Renderer>();

        // Store default materials
        defaultMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            defaultMaterials[i] = renderers[i].material;
        }
    }

    public void EnableOutline(bool enable)
    {
        // Apply outline material to all renderers
        for (int i = 0; i < renderers.Length; i++)
        {
            if (enable)
            {
                renderers[i].material = outlineMaterial;
            }
            else
            {
                renderers[i].material = defaultMaterials[i];
            }
        }
    }
}
