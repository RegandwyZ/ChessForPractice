using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class LineTweener : MonoBehaviour, IObjectTweener
{

    [SerializeField] private float _speed ;
    public void MoveTo(Transform transF, Vector3 targetPosition)
    {
        float distance = Vector3.Distance(targetPosition, transF.position);
        transF.DOMove(targetPosition, distance / _speed);
    }
}
