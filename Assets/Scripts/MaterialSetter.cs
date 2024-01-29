using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
public class MaterialSetter : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private MeshRenderer MeshRenderer
    {
        get
        {
            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();
            return _meshRenderer;
        }
    }

    public void SetSingleMaterial(Material material)
    {
        MeshRenderer.material = material;
    }
}
