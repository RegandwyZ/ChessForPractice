using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSelectorCreator : MonoBehaviour
{
    [SerializeField] private Material _freeSquareMaterial;
    [SerializeField] private Material _opponentSquareMaterial;
    [SerializeField] private GameObject _selectorPrefab;
    private readonly List<GameObject> _instantiatedSelectors = new List<GameObject>();

    public void ShowSelection(Dictionary<Vector3, bool> squareData)
    {
        ClearSelection();
        foreach (var data in squareData)
        {
            var pos = data.Key;
            pos.x += 0.73f;
            pos.y += 1;
            pos.z += 0.34f;
            
            GameObject selector = Instantiate(_selectorPrefab, pos, Quaternion.identity);
            _instantiatedSelectors.Add(selector);
            foreach (var setter in selector.GetComponentsInChildren<MaterialSetter>())
            {
                setter.SetSingleMaterial(data.Value ? _freeSquareMaterial : _opponentSquareMaterial);
            }
        }
    }

    public void ClearSelection()
    {
        foreach (var selector in _instantiatedSelectors)
        {
            Destroy(selector.gameObject);
        }
        _instantiatedSelectors.Clear();
    }
}
