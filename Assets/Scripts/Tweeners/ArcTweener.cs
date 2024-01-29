using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ArcTweener : MonoBehaviour, IObjectTweener
{
    [SerializeField] private float _speed ;
    [SerializeField] private float _height ;
    
    public void MoveTo(Transform transF, Vector3 targetPosition)
    {
        float distance = Vector3.Distance(targetPosition, transF.position);
        transF.DOJump(targetPosition, _height, 1, distance / _speed);
    }
}
