using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    public Material outlineMaterial;
    private Material[] defaultMaterials;
    private Renderer[] renderers;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        defaultMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            defaultMaterials[i] = renderers[i].material;
        }
    }

    public void EnableOutline(bool enable)
    {
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
