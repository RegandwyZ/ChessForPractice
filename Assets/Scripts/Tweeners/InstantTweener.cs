using UnityEngine;

public class InstantTweener : MonoBehaviour, IObjectTweener
{
    public void MoveTo(Transform transF, Vector3 targetPosition)
    {
        transF.position = targetPosition;
    }
}
