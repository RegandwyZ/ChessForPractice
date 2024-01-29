using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIInputReceiver : InputReceiver
{

    [SerializeField] private UnityEvent _clickEvent;
    
    
    public override void OnInputReceived()
    {
        foreach (var handler in InputHandlers)
        {
           handler.ProcessInput(Input.mousePosition, gameObject, () => _clickEvent.Invoke());
        }
    }
}
